using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Customer.Infrastructure.Data;
using Customer.Infrastructure.Repositories;
using Microsoft.Data.Sqlite;

namespace Customer.Tests.Infrastructure.Repositories
{
    public class CustomerRepositoryTests : IDisposable
    {
        private readonly CustomerDbContext _context;
        private readonly CustomerRepository _repository;
        private readonly SqliteConnection _connection;

        public CustomerRepositoryTests()
        {
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();

            var options = new DbContextOptionsBuilder<CustomerDbContext>()
                .UseSqlite(_connection)
                .Options;

            _context = new CustomerDbContext(options);
            _context.Database.EnsureCreated();
            _repository = new CustomerRepository(_context);
        }

        public void Dispose()
        {
            _context.Dispose();
            _connection.Close();
            _connection.Dispose();
        }

        #region AddAsync Tests

        [Fact]
        public async Task AddAsync_WithValidCustomer_SavesAndReturnsCustomer()
        {
            var customer = CreateSampleCustomer("João Silva", "joao@email.com", "12345678901");

            var result = await _repository.AddAsync(customer);

            result.Should().NotBeNull();
            result.Id.Should().NotBe(Guid.Empty);
            result.Name.Should().Be("João Silva");
            result.Cpf.Should().Be("12345678901");

            var savedCustomer = await _context.Customers.FindAsync(result.Id);
            savedCustomer.Should().NotBeNull();
            savedCustomer!.Cpf.Should().Be("12345678901");
        }

        #endregion

        #region GetByIdAsync Tests

        [Fact]
        public async Task GetByIdAsync_WithExistingCustomer_ReturnsCustomer()
        {
            var customer = CreateSampleCustomer("Maria", "maria@email.com", "11122233344");
            await _context.Customers.AddAsync(customer);
            await _context.SaveChangesAsync();
            _context.ChangeTracker.Clear();

            var result = await _repository.GetByIdAsync(customer.Id);

            result.Should().NotBeNull();
            result!.Id.Should().Be(customer.Id);
            result.Cpf.Should().Be("11122233344");
        }

        [Fact]
        public async Task GetByIdAsync_WithNonExistingCustomer_ReturnsNull()
        {
            var result = await _repository.GetByIdAsync(Guid.NewGuid());

            result.Should().BeNull();
        }

        #endregion

        #region GetByCpfAsync Tests

        [Fact]
        public async Task GetByCpfAsync_WithExistingCpf_ReturnsCustomer()
        {
            var customer = CreateSampleCustomer("Pedro", "pedro@email.com", "55566677788");
            await _context.Customers.AddAsync(customer);
            await _context.SaveChangesAsync();
            _context.ChangeTracker.Clear();

            var result = await _repository.GetByCpfAsync("55566677788");

            result.Should().NotBeNull();
            result!.Cpf.Should().Be("55566677788");
        }

        [Fact]
        public async Task GetByCpfAsync_WithNonExistingCpf_ReturnsNull()
        {
            var result = await _repository.GetByCpfAsync("99988877766");

            result.Should().BeNull();
        }

        #endregion

        #region GetAllAsync Tests

        [Fact]
        public async Task GetAllAsync_WithMultipleCustomers_ReturnsAllCustomers()
        {
            var customer1 = CreateSampleCustomer("Ana", "ana@email.com", "12312312311");
            var customer2 = CreateSampleCustomer("Bruno", "bruno@email.com", "45645645622");
            await _context.Customers.AddRangeAsync(customer1, customer2);
            await _context.SaveChangesAsync();
            _context.ChangeTracker.Clear();

            var result = await _repository.GetAllAsync();

            result.Should().HaveCount(2);
            result.Should().Contain(c => c.Cpf == "12312312311");
            result.Should().Contain(c => c.Cpf == "45645645622");
        }

        [Fact]
        public async Task GetAllAsync_WithNoCustomers_ReturnsEmptyList()
        {
            var result = await _repository.GetAllAsync();

            result.Should().BeEmpty();
        }

        #endregion

        #region GetActiveAsync Tests

        [Fact]
        public async Task GetActiveAsync_ReturnsOnlyActiveCustomers()
        {
            var activeCustomer = CreateSampleCustomer("Carlos", "carlos@email.com", "78978978933", true);
            var inactiveCustomer = CreateSampleCustomer("Daniel", "daniel@email.com", "10110110144", false);
            await _context.Customers.AddRangeAsync(activeCustomer, inactiveCustomer);
            await _context.SaveChangesAsync();
            _context.ChangeTracker.Clear();

            var result = await _repository.GetActiveAsync();

            result.Should().HaveCount(1);
            result.First().Cpf.Should().Be("78978978933");
            result.First().Active.Should().BeTrue();
        }

        [Fact]
        public async Task GetActiveAsync_WithNoActiveCustomers_ReturnsEmptyList()
        {
            var inactiveCustomer = CreateSampleCustomer("Daniel", "daniel@email.com", "10110110144", false);
            await _context.Customers.AddAsync(inactiveCustomer);
            await _context.SaveChangesAsync();
            _context.ChangeTracker.Clear();

            var result = await _repository.GetActiveAsync();

            result.Should().BeEmpty();
        }

        #endregion

        #region UpdateAsync Tests

        [Fact]
        public async Task UpdateAsync_WithExistingCustomer_UpdatesAndReturnsCustomer()
        {
            var customer = CreateSampleCustomer("Eduardo", "eduardo@email.com", "22233344455");
            await _context.Customers.AddAsync(customer);
            await _context.SaveChangesAsync();
            _context.ChangeTracker.Clear();

            var customerToUpdate = await _context.Customers.FirstAsync(c => c.Id == customer.Id);
            customerToUpdate.Name = "Eduardo Atualizado";
            customerToUpdate.Email = "eduardo.atualizado@email.com";
            customerToUpdate.Active = false;

            var result = await _repository.UpdateAsync(customerToUpdate);

            result.Should().NotBeNull();
            result.Name.Should().Be("Eduardo Atualizado");
            result.Email.Should().Be("eduardo.atualizado@email.com");
            result.Active.Should().BeFalse();

            var updatedCustomer = await _context.Customers.FindAsync(customer.Id);
            updatedCustomer!.Name.Should().Be("Eduardo Atualizado");
            updatedCustomer.Email.Should().Be("eduardo.atualizado@email.com");
            updatedCustomer.Active.Should().BeFalse();
        }

        #endregion

        #region DeleteAsync Tests

        [Fact]
        public async Task DeleteAsync_WithExistingCustomer_DeletesAndReturnsTrue()
        {
            var customer = CreateSampleCustomer("Fernanda", "fernanda@email.com", "66677788899");
            await _context.Customers.AddAsync(customer);
            await _context.SaveChangesAsync();
            _context.ChangeTracker.Clear();

            var result = await _repository.DeleteAsync(customer.Id);

            result.Should().BeTrue();
            var deletedCustomer = await _context.Customers.FindAsync(customer.Id);
            deletedCustomer.Should().BeNull();
        }

        [Fact]
        public async Task DeleteAsync_WithNonExistingCustomer_ReturnsFalse()
        {
            var result = await _repository.DeleteAsync(Guid.NewGuid());

            result.Should().BeFalse();
        }

        #endregion

        #region Helper Methods

        private static Customer.Domain.Entities.Customer CreateSampleCustomer(
            string name, string email, string cpf, bool active = true, Guid? id = null)
        {
            return new Customer.Domain.Entities.Customer
            {
                Id = id ?? Guid.NewGuid(),
                Name = name,
                Email = email,
                Cpf = cpf,
                Active = active,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
        }

        #endregion
    }
}