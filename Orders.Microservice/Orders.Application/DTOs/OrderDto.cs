using Orders.Domain.Enums;
using System;
using System.Collections.Generic;

namespace Orders.Application.DTOs
{
    public record OrderDto(
        Guid Id,
        Guid? CustomerId,
        OrderStatusEnum Status,
        string? Observation,
        int Number,
        string? PaymentId,
        string? QrCode,
        PaymentStatusEnum PaymentStatus,
        decimal Total,
        DateTime CreatedAt,
        DateTime UpdatedAt,
        List<OrderItemDto> Items
    );
}