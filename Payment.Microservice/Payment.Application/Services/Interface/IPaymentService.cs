using Payment.Application.DTOs;

namespace Payment.Application.Services.Interface;

public interface IPaymentService
{
    Task<PaymentDto> CreateAsync(CreatePaymentDto dto);
    Task<PaymentDto?> GetByIdAsync(string id);
    Task<PaymentDto?> GetByOrderIdAsync(string orderId);
    Task<IEnumerable<PaymentDto>> GetAllAsync();
    Task<WebhookResponseDto> ProcessWebhookAsync(string paymentId, WebhookDto dto);
}