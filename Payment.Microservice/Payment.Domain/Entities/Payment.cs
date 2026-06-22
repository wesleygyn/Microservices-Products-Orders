using Payment.Domain.Enums;

namespace Payment.Domain.Entities;

public class Payment
{
    public string Id { get; set; } = string.Empty;
    public string OrderId { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public PaymentStatusEnum Status { get; set; } = PaymentStatusEnum.PENDING;
    public string QrCode { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    public void Approve()
    {
        if (Status != PaymentStatusEnum.PENDING)
            throw new InvalidOperationException($"Não é possível aprovar um pagamento com status {Status}.");

        Status = PaymentStatusEnum.PAID;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Refuse()
    {
        if (Status != PaymentStatusEnum.PENDING)
            throw new InvalidOperationException($"Não é possível recusar um pagamento com status {Status}.");

        Status = PaymentStatusEnum.REFUSED;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Cancel()
    {
        if (Status == PaymentStatusEnum.PAID)
            throw new InvalidOperationException("Não é possível cancelar um pagamento já aprovado.");

        Status = PaymentStatusEnum.CANCELLED;
        UpdatedAt = DateTime.UtcNow;
    }
}