namespace Payment.Application.DTOs;

public record CreatePaymentDto(
    string OrderId,
    decimal TotalAmount
);