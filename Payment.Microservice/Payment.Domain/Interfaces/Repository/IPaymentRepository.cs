namespace Payment.Domain.Interfaces.Repository;

public interface IPaymentRepository
{
    Task<Payment.Domain.Entities.Payment?> GetByIdAsync(string id);
    Task<Payment.Domain.Entities.Payment?> GetByOrderIdAsync(string orderId);
    Task<IEnumerable<Payment.Domain.Entities.Payment>> GetAllAsync();
    Task<Payment.Domain.Entities.Payment> AddAsync(Payment.Domain.Entities.Payment payment);
    Task<Payment.Domain.Entities.Payment> UpdateAsync(Payment.Domain.Entities.Payment payment);
}