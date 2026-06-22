using Microsoft.Extensions.Logging;
using Payment.Application.DTOs;
using Payment.Application.Services.Interface;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Payment.Infrastructure.HttpClients;

public class OrdersHttpClient : IOrdersHttpClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<OrdersHttpClient> _logger;

    public OrdersHttpClient(HttpClient httpClient, ILogger<OrdersHttpClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task NotifyOrderAsync(string orderId, OrderWebhookDto dto)
    {
        try
        {
            var json = JsonSerializer.Serialize(dto, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            var content = new StringContent(json, Encoding.UTF8);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            _logger.LogInformation(
                "Notificando Orders. OrderId: {OrderId}, Status: {Status}",
                orderId, dto.Status);

            var response = await _httpClient.PostAsync("/api/webhook", content);

            if (!response.IsSuccessStatusCode)
                _logger.LogWarning(
                    "Falha ao notificar Orders. OrderId: {OrderId}, StatusCode: {Code}",
                    orderId, response.StatusCode);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao notificar Orders. OrderId: {OrderId}", orderId);
        }
    }
}