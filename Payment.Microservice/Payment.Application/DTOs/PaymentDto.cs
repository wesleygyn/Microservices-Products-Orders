using Payment.Domain.Enums;

namespace Payment.Application.DTOs;

public record PaymentDto(
    string Id,
    string OrderId,
    decimal TotalAmount,
    PaymentStatusEnum Status,
    string QrCode,
    DateTime CreatedAt,
    DateTime UpdatedAt
);