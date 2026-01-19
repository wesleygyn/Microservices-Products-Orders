using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Products.Domain.Entities;
using Products.Domain.Enums;
using Products.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Products.API.Data
{
    public class DatabaseSeeder
    {
        private readonly ProductsDbContext _context;
        private readonly ILogger<DatabaseSeeder> _logger;

        public DatabaseSeeder(ProductsDbContext context, ILogger<DatabaseSeeder> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task SeedAsync()
        {
            try
            {
                if (await _context.Products.AnyAsync())
                {
                    _logger.LogInformation("ℹ️ Banco de dados já contém produtos. Seed ignorado.");
                    return;
                }

                _logger.LogInformation("🌱 Iniciando seed de dados...");

                await SeedProductsAsync();

                _logger.LogInformation("✅ Seed concluído com sucesso!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Erro ao executar seed de dados");
            }
        }

        private async Task SeedProductsAsync()
        {
            _logger.LogInformation("📦 Inserindo produtos...");

            var products = new List<Product>
        {
            // LANCHES (SANDWICH)
            new Product
            {
                Name = "X-Burger Clássico",
                Price = 25.90m,
                Category = CategoryEnum.SANDWICH,
                Description = "Hambúrguer de carne bovina (150g), queijo, alface, tomate e molho especial da casa",
                Active = true,
                ImageUrl = "https://example.com/images/x-burger.jpg"
            },
            new Product
            {
                Name = "X-Bacon",
                Price = 29.90m,
                Category = CategoryEnum.SANDWICH,
                Description = "Hambúrguer de carne bovina (150g) com bacon crocante, queijo cheddar e molho barbecue",
                Active = true,
                ImageUrl = "https://example.com/images/x-bacon.jpg"
            },
            new Product
            {
                Name = "X-Egg",
                Price = 27.90m,
                Category = CategoryEnum.SANDWICH,
                Description = "Hambúrguer de carne bovina (150g), ovo, queijo, presunto e maionese",
                Active = true,
                ImageUrl = "https://example.com/images/x-egg.jpg"
            },

            // ACOMPANHAMENTOS (SIDE)
            new Product
            {
                Name = "Batata Frita Grande",
                Price = 15.90m,
                Category = CategoryEnum.SIDE,
                Description = "Porção generosa de batatas fritas crocantes com sal",
                Active = true,
                ImageUrl = "https://example.com/images/batata-grande.jpg"
            },
            new Product
            {
                Name = "Onion Rings",
                Price = 17.90m,
                Category = CategoryEnum.SIDE,
                Description = "Anéis de cebola empanados e fritos até ficarem dourados",
                Active = true,
                ImageUrl = "https://example.com/images/onion-rings.jpg"
            },
            new Product
            {
                Name = "Nuggets (10 unidades)",
                Price = 18.90m,
                Category = CategoryEnum.SIDE,
                Description = "Nuggets de frango empanados e crocantes",
                Active = true,
                ImageUrl = "https://example.com/images/nuggets.jpg"
            },

            // BEBIDAS (DRINK)
            new Product
            {
                Name = "Coca-Cola 350ml",
                Price = 6.50m,
                Category = CategoryEnum.DRINK,
                Description = "Refrigerante Coca-Cola lata 350ml gelada",
                Active = true,
                ImageUrl = "https://example.com/images/coca-lata.jpg"
            },
            new Product
            {
                Name = "Suco de Laranja Natural",
                Price = 9.90m,
                Category = CategoryEnum.DRINK,
                Description = "Suco de laranja natural 400ml",
                Active = true,
                ImageUrl = "https://example.com/images/suco-laranja.jpg"
            },
            new Product
            {
                Name = "Refrigerante 2L",
                Price = 12.90m,
                Category = CategoryEnum.DRINK,
                Description = "Refrigerante 2 litros (diversos sabores)",
                Active = true,
                ImageUrl = "https://example.com/images/refri-2l.jpg"
            },

            // SOBREMESAS (DESSERT)
            new Product
            {
                Name = "Sundae de Chocolate",
                Price = 10.90m,
                Category = CategoryEnum.DESSERT,
                Description = "Sorvete cremoso de baunilha com cobertura de chocolate",
                Active = true,
                ImageUrl = "https://example.com/images/sundae-chocolate.jpg"
            },
            new Product
            {
                Name = "Torta de Maçã",
                Price = 12.90m,
                Category = CategoryEnum.DESSERT,
                Description = "Torta de maçã quentinha com canela",
                Active = true,
                ImageUrl = "https://example.com/images/torta-maca.jpg"
            },
            new Product
            {
                Name = "Milkshake de Morango",
                Price = 14.90m,
                Category = CategoryEnum.DESSERT,
                Description = "Milkshake cremoso de morango 400ml",
                Active = true,
                ImageUrl = "https://example.com/images/milkshake-morango.jpg"
            }
        };

            await _context.Products.AddRangeAsync(products);
            await _context.SaveChangesAsync();

            _logger.LogInformation("✅ {Count} produtos inseridos com sucesso!", products.Count);
        }
    }
}