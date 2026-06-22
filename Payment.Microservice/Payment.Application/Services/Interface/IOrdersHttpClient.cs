using Payment.Application.DTOs;

namespace Payment.Application.Services.Interface;

public interface IOrdersHttpClient
{
    Task NotifyOrderAsync(string orderId, OrderWebhookDto dto);
}