namespace Payment.Application.DTOs;

public record WebhookDto(
    string PaymentId,
    string Status  // "PAID", "REFUSED", "CANCELLED"
);