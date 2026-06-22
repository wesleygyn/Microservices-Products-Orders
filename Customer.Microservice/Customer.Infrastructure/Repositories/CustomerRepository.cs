using Customer.Domain.Interfaces.Repository;
using Customer.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Customer.Infrastructure.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly CustomerDbContext _context;

    public CustomerRepository(CustomerDbContext context)
    {
        _context = context;
    }

    public async Task<Customer.Domain.Entities.Customer?> GetByIdAsync(Guid id)
        => await _context.Customers.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);

    public async Task<Customer.Domain.Entities.Customer?> GetByCpfAsync(string cpf)
        => await _context.Customers.AsNoTracking().FirstOrDefaultAsync(c => c.Cpf == cpf);

    public async Task<IEnumerable<Customer.Domain.Entities.Customer>> GetAllAsync()
        => await _context.Customers.AsNoTracking().OrderBy(c => c.Name).ToListAsync();

    public async Task<IEnumerable<Customer.Domain.Entities.Customer>> GetActiveAsync()
        => await _context.Customers.AsNoTracking().Where(c => c.Active).OrderBy(c => c.Name).ToListAsync();

    public async Task<Customer.Domain.Entities.Customer> AddAsync(Customer.Domain.Entities.Customer customer)
    {
        await _context.Customers.AddAsync(customer);
        await _context.SaveChangesAsync();
        return customer;
    }

    public async Task<Customer.Domain.Entities.Customer> UpdateAsync(Customer.Domain.Entities.Customer customer)
    {
        _context.Customers.Update(customer);
        await _context.SaveChangesAsync();
        return customer;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var customer = await _context.Customers.FindAsync(id);
        if (customer == null) return false;
        _context.Customers.Remove(customer);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExistsAsync(Guid id)
        => await _context.Customers.AnyAsync(c => c.Id == id);
}