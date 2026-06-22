using Microsoft.Extensions.Logging;
using Payment.Application.DTOs;
using Payment.Application.Services.Interface;
using Payment.Domain.Enums;
using Payment.Domain.Interfaces.Repository;
using Payment.Application.Settings;
using Microsoft.Extensions.Options;

namespace Payment.Application.Services.Service;

public class PaymentService : IPaymentService
{
    private readonly IPaymentRepository _repository;
    private readonly IOrdersHttpClient _ordersHttpClient;
    private readonly ILogger<PaymentService> _logger;
    private readonly AutoApproveSettings _autoApprove;

    public PaymentService(
        IPaymentRepository repository,
        IOrdersHttpClient ordersHttpClient,
        ILogger<PaymentService> logger,
        IOptions<AutoApproveSettings> autoApproveOptions)
    {
        _repository = repository;
        _ordersHttpClient = ordersHttpClient;
        _logger = logger;
        _autoApprove = autoApproveOptions.Value;
    }

    public async Task<PaymentDto> CreateAsync(CreatePaymentDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.OrderId))
            throw new ArgumentException("OrderId é obrigatório.");

        if (dto.TotalAmount <= 0)
            throw new ArgumentException("O valor do pagamento deve ser maior que zero.");

        var existing = await _repository.GetByOrderIdAsync(dto.OrderId);
        if (existing != null && existing.Status == PaymentStatusEnum.PENDING)
            return MapToDto(existing);

        var qrCode = GenerateQrCode(dto.OrderId, dto.TotalAmount);

        var payment = new Domain.Entities.Payment
        {
            Id = Guid.NewGuid().ToString(),
            OrderId = dto.OrderId,
            TotalAmount = dto.TotalAmount,
            Status = PaymentStatusEnum.PENDING,
            QrCode = qrCode
        };

        var created = await _repository.AddAsync(payment);

        _logger.LogInformation(
            "Pagamento criado. PaymentId: {PaymentId}, OrderId: {OrderId}, Valor: {Total}",
            created.Id, created.OrderId, created.TotalAmount);

        // Auto-aprovação em ambiente de desenvolvimento
        if (_autoApprove.Enabled)
        {
            var paymentId = created.Id;
            var orderId = created.OrderId;
            var delay = _autoApprove.DelaySeconds;

            _ = Task.Run(async () =>
            {
                await Task.Delay(TimeSpan.FromSeconds(delay));

                _logger.LogInformation(
                    "Auto-aprovação disparada. PaymentId: {PaymentId}, OrderId: {OrderId}",
                    paymentId, orderId);

                await ProcessWebhookAsync(paymentId, new WebhookDto(paymentId, "PAID"));
            });
        }

        return MapToDto(created);
    }

    public async Task<PaymentDto?> GetByIdAsync(string id)
    {
        var payment = await _repository.GetByIdAsync(id);
        return payment != null ? MapToDto(payment) : null;
    }

    public async Task<PaymentDto?> GetByOrderIdAsync(string orderId)
    {
        var payment = await _repository.GetByOrderIdAsync(orderId);
        return payment != null ? MapToDto(payment) : null;
    }

    public async Task<IEnumerable<PaymentDto>> GetAllAsync()
    {
        var payments = await _repository.GetAllAsync();
        return payments.Select(MapToDto);
    }

    public async Task<WebhookResponseDto> ProcessWebhookAsync(string paymentId, WebhookDto dto)
    {
        try
        {
            var payment = await _repository.GetByIdAsync(paymentId);
            if (payment == null)
            {
                _logger.LogWarning("Pagamento não encontrado. PaymentId: {PaymentId}", paymentId);
                return new WebhookResponseDto(false, $"Pagamento {paymentId} não encontrado.");
            }

            var newStatus = dto.Status.ToUpper() switch
            {
                "PAID" => PaymentStatusEnum.PAID,
                "REFUSED" => PaymentStatusEnum.REFUSED,
                "CANCELLED" => PaymentStatusEnum.CANCELLED,
                _ => throw new ArgumentException($"Status inválido: {dto.Status}")
            };

            switch (newStatus)
            {
                case PaymentStatusEnum.PAID:
                    payment.Approve();
                    break;
                case PaymentStatusEnum.REFUSED:
                    payment.Refuse();
                    break;
                case PaymentStatusEnum.CANCELLED:
                    payment.Cancel();
                    break;
            }

            await _repository.UpdateAsync(payment);

            _logger.LogInformation(
                "Status do pagamento atualizado. PaymentId: {PaymentId}, Status: {Status}",
                paymentId, newStatus);

            await _ordersHttpClient.NotifyOrderAsync(payment.OrderId, new OrderWebhookDto(
                Status: newStatus.ToString(),
                OrderId: payment.OrderId,
                PaymentId: payment.Id
            ));

            return new WebhookResponseDto(true, $"Pagamento {newStatus} processado com sucesso.");
        }
        catch (InvalidOperationException ex)
        {
            return new WebhookResponseDto(false, ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao processar webhook. PaymentId: {PaymentId}", paymentId);
            return new WebhookResponseDto(false, "Erro ao processar webhook.");
        }
    }

    private static string GenerateQrCode(string orderId, decimal totalAmount)
    {
        return $"FIAP-LANCHONETE|{orderId}|{totalAmount:F2}|{Guid.NewGuid()}";
    }

    private static PaymentDto MapToDto(Domain.Entities.Payment p) => new(
        p.Id,
        p.OrderId,
        p.TotalAmount,
        p.Status,
        p.QrCode,
        p.CreatedAt,
        p.UpdatedAt
    );
}