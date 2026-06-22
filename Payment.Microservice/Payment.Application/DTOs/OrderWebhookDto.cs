namespace Payment.Application.DTOs;

public record OrderWebhookDto(
    string Status,
    string OrderId,
    string PaymentId
);