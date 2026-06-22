using Customer.Application.DTOs;
using Customer.Application.Services.Interface;
using Customer.Domain.Interfaces.Repository;

namespace Customer.Application.Services.Service;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _repository;

    public CustomerService(ICustomerRepository repository)
    {
        _repository = repository;
    }

    public async Task<CustomerDto?> GetByIdAsync(Guid id)
    {
        var customer = await _repository.GetByIdAsync(id);
        return customer != null ? MapToDto(customer) : null;
    }

    public async Task<CustomerDto?> GetByCpfAsync(string cpf)
    {
        var cpfClean = cpf.Replace(".", "").Replace("-", "").Replace(" ", "");
        var customer = await _repository.GetByCpfAsync(cpfClean);
        return customer != null ? MapToDto(customer) : null;
    }

    public async Task<IEnumerable<CustomerDto>> GetAllAsync()
    {
        var customers = await _repository.GetAllAsync();
        return customers.Select(MapToDto);
    }

    public async Task<IEnumerable<CustomerDto>> GetActiveAsync()
    {
        var customers = await _repository.GetActiveAsync();
        return customers.Select(MapToDto);
    }

    public async Task<CustomerDto> CreateAsync(CreateCustomerDto dto)
    {
        ValidateCpf(dto.Cpf);
        ValidateEmail(dto.Email);

        var cpfClean = dto.Cpf.Replace(".", "").Replace("-", "").Replace(" ", "");

        var existing = await _repository.GetByCpfAsync(cpfClean);
        if (existing != null)
            throw new InvalidOperationException($"Já existe um cliente com o CPF {dto.Cpf}.");

        var customer = new Customer.Domain.Entities.Customer
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            Email = dto.Email,
            Cpf = cpfClean,
            Active = true
        };

        var created = await _repository.AddAsync(customer);
        return MapToDto(created);
    }

    public async Task<CustomerDto> UpdateAsync(Guid id, UpdateCustomerDto dto)
    {
        var customer = await _repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Cliente com ID {id} não encontrado.");

        ValidateEmail(dto.Email);

        customer.Name = dto.Name;
        customer.Email = dto.Email;
        customer.Active = dto.Active;
        customer.UpdatedAt = DateTime.UtcNow;

        var updated = await _repository.UpdateAsync(customer);
        return MapToDto(updated);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        return await _repository.DeleteAsync(id);
    }

    private static void ValidateCpf(string cpf)
    {
        var digits = cpf.Replace(".", "").Replace("-", "").Replace(" ", "");
        if (digits.Length != 11 || !digits.All(char.IsDigit))
            throw new ArgumentException("CPF inválido. Informe 11 dígitos numéricos.");
    }

    private static void ValidateEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email) || !email.Contains('@') || !email.Contains('.'))
            throw new ArgumentException("E-mail inválido.");
    }

    private static CustomerDto MapToDto(Customer.Domain.Entities.Customer c) => new(
        c.Id,
        c.Name,
        c.Email,
        c.Cpf,
        c.Active,
        c.CreatedAt,
        c.UpdatedAt
    );
}