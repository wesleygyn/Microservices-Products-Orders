namespace Customer.Domain.Interfaces.Repository;

public interface ICustomerRepository
{
    Task<Customer.Domain.Entities.Customer?> GetByIdAsync(Guid id);
    Task<Customer.Domain.Entities.Customer?> GetByCpfAsync(string cpf);
    Task<IEnumerable<Customer.Domain.Entities.Customer>> GetAllAsync();
    Task<IEnumerable<Customer.Domain.Entities.Customer>> GetActiveAsync();
    Task<Customer.Domain.Entities.Customer> AddAsync(Customer.Domain.Entities.Customer customer);
    Task<Customer.Domain.Entities.Customer> UpdateAsync(Customer.Domain.Entities.Customer customer);
    Task<bool> DeleteAsync(Guid id);
    Task<bool> ExistsAsync(Guid id);
}