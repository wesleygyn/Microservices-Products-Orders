using System;

namespace Orders.Infrastructure.HttpClients
{
    public record PaymentResponse(
        string Id,
        string OrderId,
        decimal TotalAmount,
        string Status,
        string QrCode,
        DateTime CreatedAt
    );
}