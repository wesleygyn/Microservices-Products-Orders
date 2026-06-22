using Customer.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Customer.API.Data;

public class DatabaseSeeder
{
    private readonly CustomerDbContext _context;
    private readonly ILogger<DatabaseSeeder> _logger;

    public DatabaseSeeder(CustomerDbContext context, ILogger<DatabaseSeeder> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task SeedAsync()
    {
        try
        {
            if (await _context.Customers.AnyAsync())
            {
                _logger.LogInformation("ℹ️ Banco de dados já contém clientes. Seed ignorado.");
                return;
            }

            _logger.LogInformation("🌱 Iniciando seed de dados...");

            var customers = new List<Customer.Domain.Entities.Customer>
            {
                new()
                {
                    Name = "Cliente Anônimo",
                    Email = "anonimo@lanchonete.com",
                    Cpf = "00000000000",
                    Active = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new()
                {
                    Name = "João Silva",
                    Email = "joao.silva@email.com",
                    Cpf = "12345678901",
                    Active = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new()
                {
                    Name = "Maria Souza",
                    Email = "maria.souza@email.com",
                    Cpf = "98765432100",
                    Active = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            };

            await _context.Customers.AddRangeAsync(customers);
            await _context.SaveChangesAsync();

            _logger.LogInformation("✅ {Count} clientes inseridos com sucesso!", customers.Count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Erro ao executar seed de dados");
        }
    }
}