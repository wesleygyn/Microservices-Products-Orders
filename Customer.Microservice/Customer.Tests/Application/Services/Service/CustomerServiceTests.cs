using Customer.Application.DTOs;
using Customer.Application.Services.Service;
using Customer.Domain.Interfaces.Repository;
using Moq;
using FluentAssertions;

namespace Customer.Tests.Application.Services.Service
{
    public class CustomerServiceTests
    {
        private readonly Mock<ICustomerRepository> _repositoryMock;
        private readonly CustomerService _service;

        public CustomerServiceTests()
        {
            _repositoryMock = new Mock<ICustomerRepository>();
            _service = new CustomerService(_repositoryMock.Object);
        }

        [Fact]
        public async Task CreateAsync_Should_Create_Customer_Successfully()
        {
            _repositoryMock.Setup(r => r.GetByCpfAsync(It.IsAny<string>()))
                .ReturnsAsync((Customer.Domain.Entities.Customer?)null);

            _repositoryMock.Setup(r => r.AddAsync(It.IsAny<Customer.Domain.Entities.Customer>()))
                .ReturnsAsync((Customer.Domain.Entities.Customer c) => c);

            var dto = new CreateCustomerDto("João Silva", "joao@email.com", "12345678901");
            var result = await _service.CreateAsync(dto);

            result.Should().NotBeNull();
            result.Name.Should().Be("João Silva");
            result.Cpf.Should().Be("12345678901");
            _repositoryMock.Verify(r => r.AddAsync(It.IsAny<Customer.Domain.Entities.Customer>()), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_Should_Throw_When_Cpf_Already_Exists()
        {
            var existing = new Customer.Domain.Entities.Customer
            {
                Id = Guid.NewGuid(),
                Name = "Outro",
                Email = "outro@email.com",
                Cpf = "12345678901"
            };

            _repositoryMock.Setup(r => r.GetByCpfAsync("12345678901")).ReturnsAsync(existing);

            var dto = new CreateCustomerDto("João Silva", "joao@email.com", "12345678901");

            await Assert.ThrowsAsync<InvalidOperationException>(() => _service.CreateAsync(dto));
        }

        [Fact]
        public async Task CreateAsync_Should_Throw_When_Cpf_Is_Invalid()
        {
            var dto = new CreateCustomerDto("João Silva", "joao@email.com", "123");

            await Assert.ThrowsAsync<ArgumentException>(() => _service.CreateAsync(dto));
        }

        [Fact]
        public async Task CreateAsync_Should_Throw_When_Email_Is_Invalid()
        {
            var dto = new CreateCustomerDto("João Silva", "emailinvalido", "12345678901");

            await Assert.ThrowsAsync<ArgumentException>(() => _service.CreateAsync(dto));
        }

        [Fact]
        public async Task GetByCpfAsync_Should_Return_Customer_When_Found()
        {
            var customer = new Customer.Domain.Entities.Customer
            {
                Id = Guid.NewGuid(),
                Name = "João",
                Email = "joao@email.com",
                Cpf = "12345678901",
                Active = true
            };

            _repositoryMock.Setup(r => r.GetByCpfAsync("12345678901")).ReturnsAsync(customer);

            var result = await _service.GetByCpfAsync("12345678901");

            result.Should().NotBeNull();
            result!.Cpf.Should().Be("12345678901");
        }

        [Fact]
        public async Task GetByCpfAsync_Should_Return_Null_When_Not_Found()
        {
            _repositoryMock.Setup(r => r.GetByCpfAsync(It.IsAny<string>()))
                .ReturnsAsync((Customer.Domain.Entities.Customer?)null);

            var result = await _service.GetByCpfAsync("00000000000");

            result.Should().BeNull();
        }

        [Fact]
        public async Task UpdateAsync_Should_Throw_When_Customer_Not_Found()
        {
            _repositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Customer.Domain.Entities.Customer?)null);

            var dto = new UpdateCustomerDto("Novo Nome", "novo@email.com", true);

            await Assert.ThrowsAsync<KeyNotFoundException>(
                () => _service.UpdateAsync(Guid.NewGuid(), dto));
        }

        [Fact]
        public async Task UpdateAsync_Should_Update_Successfully()
        {
            var customerId = Guid.NewGuid();

            var customer = new Customer.Domain.Entities.Customer
            {
                Id = customerId,
                Name = "João",
                Email = "joao@email.com",
                Cpf = "12345678901",
                Active = true
            };

            _repositoryMock.Setup(r => r.GetByIdAsync(customerId)).ReturnsAsync(customer);
            _repositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Customer.Domain.Entities.Customer>()))
                .ReturnsAsync((Customer.Domain.Entities.Customer c) => c);

            var dto = new UpdateCustomerDto("João Atualizado", "novo@email.com", true);
            var result = await _service.UpdateAsync(customerId, dto);

            result.Name.Should().Be("João Atualizado");
            result.Email.Should().Be("novo@email.com");
        }

        [Fact]
        public async Task DeleteAsync_Should_Return_False_When_Not_Found()
        {
            _repositoryMock.Setup(r => r.DeleteAsync(It.IsAny<Guid>()))
                .ReturnsAsync(false);

            var result = await _service.DeleteAsync(Guid.NewGuid());

            result.Should().BeFalse();
        }

        [Fact]
        public async Task DeleteAsync_Should_Return_True_When_Deleted()
        {
            var customerId = Guid.NewGuid();

            _repositoryMock.Setup(r => r.DeleteAsync(customerId))
                .ReturnsAsync(true);

            var result = await _service.DeleteAsync(customerId);

            result.Should().BeTrue();
        }

        [Fact]
        public async Task GetAllAsync_Should_Return_All_Customers()
        {
            var customers = new List<Customer.Domain.Entities.Customer>
            {
                new() { Id = Guid.NewGuid(), Name = "João", Email = "joao@email.com", Cpf = "12345678901", Active = true },
                new() { Id = Guid.NewGuid(), Name = "Maria", Email = "maria@email.com", Cpf = "98765432100", Active = true }
            };

            _repositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(customers);

            var result = await _service.GetAllAsync();

            result.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetActiveAsync_Should_Return_Only_Active_Customers()
        {
            var customers = new List<Customer.Domain.Entities.Customer>
            {
                new() { Id = Guid.NewGuid(), Name = "João", Email = "joao@email.com", Cpf = "12345678901", Active = true }
            };

            _repositoryMock.Setup(r => r.GetActiveAsync()).ReturnsAsync(customers);

            var result = await _service.GetActiveAsync();

            result.Should().HaveCount(1);
            result.First().Active.Should().BeTrue();
        }

        [Fact]
        public async Task GetByIdAsync_Should_Return_Customer_When_Found()
        {
            var customerId = Guid.NewGuid();
            var customer = new Customer.Domain.Entities.Customer
            {
                Id = customerId,
                Name = "João",
                Email = "joao@email.com",
                Cpf = "12345678901",
                Active = true
            };

            _repositoryMock.Setup(r => r.GetByIdAsync(customerId)).ReturnsAsync(customer);

            var result = await _service.GetByIdAsync(customerId);

            result.Should().NotBeNull();
            result!.Id.Should().Be(customerId);
        }

        [Fact]
        public async Task GetByIdAsync_Should_Return_Null_When_Not_Found()
        {
            _repositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Customer.Domain.Entities.Customer?)null);

            var result = await _service.GetByIdAsync(Guid.NewGuid());

            result.Should().BeNull();
        }
    }
}