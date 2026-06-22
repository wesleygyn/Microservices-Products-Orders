using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Payment.Application.DTOs;
using Payment.Application.Services.Interface;
using System;
using System.Threading.Tasks;

namespace Payment.API.Controllers;

[ApiController]
[Route("paymentservice/v1/payments")]
public class PaymentsController : ControllerBase
{
    private readonly IPaymentService _service;
    private readonly ILogger<PaymentsController> _logger;

    public PaymentsController(IPaymentService service, ILogger<PaymentsController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAllAsync();
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var result = await _service.GetByIdAsync(id);
        if (result is null) return NotFound(new { message = $"Pagamento {id} não encontrado." });
        return Ok(result);
    }

    [HttpGet("order/{orderId}")]
    public async Task<IActionResult> GetByOrderId(string orderId)
    {
        var result = await _service.GetByOrderIdAsync(orderId);
        if (result is null) return NotFound(new { message = $"Pagamento para o pedido {orderId} não encontrado." });
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePaymentDto dto)
    {
        try
        {
            var result = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar pagamento");
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("{id}/webhook")]
    public async Task<IActionResult> ProcessWebhook(string id, [FromBody] WebhookDto dto)
    {
        var result = await _service.ProcessWebhookAsync(id, dto);
        if (!result.Success) return BadRequest(result);
        return Ok(result);
    }
}