using Customer.Application.DTOs;

namespace Customer.Application.Services.Interface;

public interface ICustomerService
{
    Task<CustomerDto?> GetByIdAsync(Guid id);
    Task<CustomerDto?> GetByCpfAsync(string cpf);
    Task<IEnumerable<CustomerDto>> GetAllAsync();
    Task<IEnumerable<CustomerDto>> GetActiveAsync();
    Task<CustomerDto> CreateAsync(CreateCustomerDto dto);
    Task<CustomerDto> UpdateAsync(Guid id, UpdateCustomerDto dto);
    Task<bool> DeleteAsync(Guid id);
}