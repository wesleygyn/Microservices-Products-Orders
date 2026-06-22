using FluentAssertions;
using Payment.Domain.Enums;

namespace Payment.Tests.Domain.Entities
{
    public class PaymentEntityTests
    {
        [Fact]
        public void Should_Create_Payment_With_Pending_Status()
        {
            var payment = new Payment.Domain.Entities.Payment
            {
                Id = Guid.NewGuid().ToString(),
                OrderId = "order-123",
                TotalAmount = 99.90m,
                Status = PaymentStatusEnum.PENDING
            };

            payment.Status.Should().Be(PaymentStatusEnum.PENDING);
            payment.OrderId.Should().Be("order-123");
        }

        [Fact]
        public void Approve_Should_Change_Status_To_Paid()
        {
            var payment = new Payment.Domain.Entities.Payment { Status = PaymentStatusEnum.PENDING };
            payment.Approve();
            payment.Status.Should().Be(PaymentStatusEnum.PAID);
        }

        [Fact]
        public void Approve_Should_Throw_When_Not_Pending()
        {
            var payment = new Payment.Domain.Entities.Payment { Status = PaymentStatusEnum.REFUSED };
            Assert.Throws<InvalidOperationException>(() => payment.Approve());
        }

        [Fact]
        public void Refuse_Should_Change_Status_To_Refused()
        {
            var payment = new Payment.Domain.Entities.Payment { Status = PaymentStatusEnum.PENDING };
            payment.Refuse();
            payment.Status.Should().Be(PaymentStatusEnum.REFUSED);
        }

        [Fact]
        public void Cancel_Should_Throw_When_Already_Paid()
        {
            var payment = new Payment.Domain.Entities.Payment { Status = PaymentStatusEnum.PAID };
            Assert.Throws<InvalidOperationException>(() => payment.Cancel());
        }
    }
}