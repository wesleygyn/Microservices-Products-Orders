using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Payment.Application.DTOs;
using Payment.Application.Services.Interface;
using Payment.Application.Services.Service;
using Payment.Application.Settings;
using Payment.Domain.Enums;
using Payment.Domain.Interfaces.Repository;

namespace Payment.Tests.Services;

public class PaymentServiceTests
{
    private readonly Mock<IPaymentRepository> _repositoryMock;
    private readonly Mock<IOrdersHttpClient> _ordersHttpClientMock;
    private readonly Mock<ILogger<PaymentService>> _loggerMock;
    private readonly PaymentService _service;

    public PaymentServiceTests()
    {
        _repositoryMock = new Mock<IPaymentRepository>();
        _ordersHttpClientMock = new Mock<IOrdersHttpClient>();
        _loggerMock = new Mock<ILogger<PaymentService>>();

        var autoApproveOptions = Options.Create(new AutoApproveSettings
        {
            Enabled = false,
            DelaySeconds = 0
        });

        _service = new PaymentService(
            _repositoryMock.Object,
            _ordersHttpClientMock.Object,
            _loggerMock.Object,
            autoApproveOptions);
    }

    [Fact]
    public async Task CreateAsync_Should_Create_Payment_Successfully()
    {
        _repositoryMock.Setup(r => r.GetByOrderIdAsync(It.IsAny<string>()))
            .ReturnsAsync((Payment.Domain.Entities.Payment?)null);

        _repositoryMock.Setup(r => r.AddAsync(It.IsAny<Payment.Domain.Entities.Payment>()))
            .ReturnsAsync((Payment.Domain.Entities.Payment p) => p);

        var dto = new CreatePaymentDto("order-123", 99.90m);
        var result = await _service.CreateAsync(dto);

        result.Should().NotBeNull();
        result.OrderId.Should().Be("order-123");
        result.Status.Should().Be(PaymentStatusEnum.PENDING);
        result.QrCode.Should().NotBeNullOrEmpty();
        _repositoryMock.Verify(r => r.AddAsync(It.IsAny<Payment.Domain.Entities.Payment>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_Should_Throw_When_OrderId_Is_Empty()
    {
        var dto = new CreatePaymentDto("", 99.90m);
        await Assert.ThrowsAsync<ArgumentException>(() => _service.CreateAsync(dto));
    }

    [Fact]
    public async Task CreateAsync_Should_Throw_When_Amount_Is_Zero()
    {
        var dto = new CreatePaymentDto("order-123", 0);
        await Assert.ThrowsAsync<ArgumentException>(() => _service.CreateAsync(dto));
    }

    [Fact]
    public async Task CreateAsync_Should_Return_Existing_When_Already_Pending()
    {
        var existing = new Payment.Domain.Entities.Payment
        {
            Id = "pay-1",
            OrderId = "order-123",
            TotalAmount = 99.90m,
            Status = PaymentStatusEnum.PENDING,
            QrCode = "QR123"
        };

        _repositoryMock.Setup(r => r.GetByOrderIdAsync("order-123")).ReturnsAsync(existing);

        var dto = new CreatePaymentDto("order-123", 99.90m);
        var result = await _service.CreateAsync(dto);

        result.Id.Should().Be("pay-1");
        _repositoryMock.Verify(r => r.AddAsync(It.IsAny<Payment.Domain.Entities.Payment>()), Times.Never);
    }

    [Fact]
    public async Task ProcessWebhookAsync_Should_Approve_Payment()
    {
        var payment = new Payment.Domain.Entities.Payment
        {
            Id = "pay-1",
            OrderId = "order-123",
            TotalAmount = 99.90m,
            Status = PaymentStatusEnum.PENDING,
            QrCode = "QR123"
        };

        _repositoryMock.Setup(r => r.GetByIdAsync("pay-1")).ReturnsAsync(payment);
        _repositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Payment.Domain.Entities.Payment>()))
            .ReturnsAsync((Payment.Domain.Entities.Payment p) => p);
        _ordersHttpClientMock.Setup(c => c.NotifyOrderAsync(It.IsAny<string>(), It.IsAny<OrderWebhookDto>()))
            .Returns(Task.CompletedTask);

        var result = await _service.ProcessWebhookAsync("pay-1", new WebhookDto("pay-1", "PAID"));

        result.Success.Should().BeTrue();
        payment.Status.Should().Be(PaymentStatusEnum.PAID);
        _ordersHttpClientMock.Verify(
            c => c.NotifyOrderAsync(It.IsAny<string>(), It.IsAny<OrderWebhookDto>()), Times.Once);
    }

    [Fact]
    public async Task ProcessWebhookAsync_Should_Return_False_When_Payment_Not_Found()
    {
        _repositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<string>()))
            .ReturnsAsync((Payment.Domain.Entities.Payment?)null);

        var result = await _service.ProcessWebhookAsync("inexistente", new WebhookDto("inexistente", "PAID"));

        result.Success.Should().BeFalse();
    }

    [Fact]
    public async Task ProcessWebhookAsync_Should_Refuse_Payment()
    {
        var payment = new Payment.Domain.Entities.Payment
        {
            Id = "pay-1",
            OrderId = "order-123",
            TotalAmount = 50m,
            Status = PaymentStatusEnum.PENDING,
            QrCode = "QR123"
        };

        _repositoryMock.Setup(r => r.GetByIdAsync("pay-1")).ReturnsAsync(payment);
        _repositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Payment.Domain.Entities.Payment>()))
            .ReturnsAsync((Payment.Domain.Entities.Payment p) => p);
        _ordersHttpClientMock.Setup(c => c.NotifyOrderAsync(It.IsAny<string>(), It.IsAny<OrderWebhookDto>()))
            .Returns(Task.CompletedTask);

        var result = await _service.ProcessWebhookAsync("pay-1", new WebhookDto("pay-1", "REFUSED"));

        result.Success.Should().BeTrue();
        payment.Status.Should().Be(PaymentStatusEnum.REFUSED);
    }
}