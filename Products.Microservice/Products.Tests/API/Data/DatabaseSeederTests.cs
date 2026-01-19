using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Products.API.Data;
using Products.Domain.Entities;
using Products.Domain.Enums;
using Products.Infrastructure.Data;
using Xunit;

namespace Products.Tests.API.Data
{
    public class DatabaseSeederTests : IDisposable
    {
        private readonly ProductsDbContext _context;
        private readonly Mock<ILogger<DatabaseSeeder>> _loggerMock;
        private readonly DatabaseSeeder _seeder;

        public DatabaseSeederTests()
        {
            var options = new DbContextOptionsBuilder<ProductsDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ProductsDbContext(options);
            _loggerMock = new Mock<ILogger<DatabaseSeeder>>();
            _seeder = new DatabaseSeeder(_context, _loggerMock.Object);
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        #region SeedAsync Tests

        [Fact]
        public async Task SeedAsync_WithEmptyDatabase_SeedsProductsSuccessfully()
        {
            await _seeder.SeedAsync();

            var products = await _context.Products.ToListAsync();
            products.Should().NotBeEmpty();
            products.Should().HaveCount(12);
        }

        [Fact]
        public async Task SeedAsync_WithEmptyDatabase_LogsInitiationMessage()
        {
            await _seeder.SeedAsync();

            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => o.ToString()!.Contains("Iniciando seed")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task SeedAsync_WithEmptyDatabase_LogsSuccessMessage()
        {
            await _seeder.SeedAsync();

            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => o.ToString()!.Contains("Seed concluído com sucesso")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task SeedAsync_WithExistingProducts_DoesNotSeedAgain()
        {
            var existingProduct = new Product
            {
                Name = "Produto Existente",
                Price = 10.00m,
                Category = CategoryEnum.SANDWICH,
                Active = true
            };
            await _context.Products.AddAsync(existingProduct);
            await _context.SaveChangesAsync();

            await _seeder.SeedAsync();

            var products = await _context.Products.ToListAsync();
            products.Should().HaveCount(1);
            products.First().Name.Should().Be("Produto Existente");
        }

        [Fact]
        public async Task SeedAsync_WithExistingProducts_LogsSkipMessage()
        {
            var existingProduct = new Product
            {
                Name = "Produto Existente",
                Price = 10.00m,
                Category = CategoryEnum.SANDWICH,
                Active = true
            };
            await _context.Products.AddAsync(existingProduct);
            await _context.SaveChangesAsync();
            await _seeder.SeedAsync();

            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => o.ToString()!.Contains("já contém produtos") &&
                                                   o.ToString()!.Contains("Seed ignorado")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task SeedAsync_WhenExceptionOccurs_LogsError()
        {
            var options = new DbContextOptionsBuilder<ProductsDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var disposableContext = new ProductsDbContext(options);
            var seederWithDisposableContext = new DatabaseSeeder(disposableContext, _loggerMock.Object);

            await disposableContext.DisposeAsync();

            await seederWithDisposableContext.SeedAsync();

            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => o.ToString()!.Contains("Erro ao executar seed")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task SeedAsync_WhenExceptionOccurs_DoesNotThrow()
        {
            var options = new DbContextOptionsBuilder<ProductsDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var disposableContext = new ProductsDbContext(options);
            var seederWithDisposableContext = new DatabaseSeeder(disposableContext, _loggerMock.Object);

            await disposableContext.DisposeAsync();

            Func<Task> act = async () => await seederWithDisposableContext.SeedAsync();

            await act.Should().NotThrowAsync();
        }

        #endregion

        #region Product Categories Tests

        [Fact]
        public async Task SeedAsync_ShouldCreateSandwichProducts()
        {
            await _seeder.SeedAsync();

            var sandwiches = await _context.Products
                .Where(p => p.Category == CategoryEnum.SANDWICH)
                .ToListAsync();

            sandwiches.Should().HaveCount(3);
            sandwiches.Select(s => s.Name).Should().Contain(new[]
            {
                "X-Burger Clássico",
                "X-Bacon",
                "X-Egg"
            });
        }

        [Fact]
        public async Task SeedAsync_ShouldCreateSideProducts()
        {
            await _seeder.SeedAsync();

            var sides = await _context.Products
                .Where(p => p.Category == CategoryEnum.SIDE)
                .ToListAsync();

            sides.Should().HaveCount(3);
            sides.Select(s => s.Name).Should().Contain(new[]
            {
                "Batata Frita Grande",
                "Onion Rings",
                "Nuggets (10 unidades)"
            });
        }

        [Fact]
        public async Task SeedAsync_ShouldCreateDrinkProducts()
        {
            await _seeder.SeedAsync();

            var drinks = await _context.Products
                .Where(p => p.Category == CategoryEnum.DRINK)
                .ToListAsync();

            drinks.Should().HaveCount(3);
            drinks.Select(d => d.Name).Should().Contain(new[]
            {
                "Coca-Cola 350ml",
                "Suco de Laranja Natural",
                "Refrigerante 2L"
            });
        }

        [Fact]
        public async Task SeedAsync_ShouldCreateDessertProducts()
        {
            await _seeder.SeedAsync();

            var desserts = await _context.Products
                .Where(p => p.Category == CategoryEnum.DESSERT)
                .ToListAsync();

            desserts.Should().HaveCount(3);
            desserts.Select(d => d.Name).Should().Contain(new[]
            {
                "Sundae de Chocolate",
                "Torta de Maçã",
                "Milkshake de Morango"
            });
        }

        #endregion

        #region Product Properties Tests

        [Fact]
        public async Task SeedAsync_AllProducts_ShouldBeActive()
        {
            await _seeder.SeedAsync();

            var products = await _context.Products.ToListAsync();
            products.Should().OnlyContain(p => p.Active == true);
        }

        [Fact]
        public async Task SeedAsync_AllProducts_ShouldHaveName()
        {
            await _seeder.SeedAsync();

            var products = await _context.Products.ToListAsync();
            products.Should().OnlyContain(p => !string.IsNullOrWhiteSpace(p.Name));
        }

        [Fact]
        public async Task SeedAsync_AllProducts_ShouldHavePositivePrice()
        {
            await _seeder.SeedAsync();

            var products = await _context.Products.ToListAsync();
            products.Should().OnlyContain(p => p.Price > 0);
        }

        [Fact]
        public async Task SeedAsync_AllProducts_ShouldHaveDescription()
        {
            await _seeder.SeedAsync();

            var products = await _context.Products.ToListAsync();
            products.Should().OnlyContain(p => !string.IsNullOrWhiteSpace(p.Description));
        }

        [Fact]
        public async Task SeedAsync_AllProducts_ShouldHaveImageUrl()
        {
            await _seeder.SeedAsync();

            var products = await _context.Products.ToListAsync();
            products.Should().OnlyContain(p => !string.IsNullOrWhiteSpace(p.ImageUrl));
        }

        [Fact]
        public async Task SeedAsync_AllProducts_ShouldHaveValidCategory()
        {
            await _seeder.SeedAsync();

            var products = await _context.Products.ToListAsync();
            products.Should().OnlyContain(p => Enum.IsDefined(typeof(CategoryEnum), p.Category));
        }

        #endregion

        #region Specific Product Tests

        [Fact]
        public async Task SeedAsync_XBurgerClassico_ShouldHaveCorrectProperties()
        {
            await _seeder.SeedAsync();

            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.Name == "X-Burger Clássico");

            product.Should().NotBeNull();
            product!.Price.Should().Be(25.90m);
            product.Category.Should().Be(CategoryEnum.SANDWICH);
            product.Active.Should().BeTrue();
            product.Description.Should().Contain("Hambúrguer de carne bovina");
        }

        [Fact]
        public async Task SeedAsync_BatataFritaGrande_ShouldHaveCorrectProperties()
        {
            await _seeder.SeedAsync();

            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.Name == "Batata Frita Grande");

            product.Should().NotBeNull();
            product!.Price.Should().Be(15.90m);
            product.Category.Should().Be(CategoryEnum.SIDE);
            product.Active.Should().BeTrue();
        }

        [Fact]
        public async Task SeedAsync_CocaCola_ShouldHaveCorrectProperties()
        {
            await _seeder.SeedAsync();

            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.Name == "Coca-Cola 350ml");

            product.Should().NotBeNull();
            product!.Price.Should().Be(6.50m);
            product.Category.Should().Be(CategoryEnum.DRINK);
            product.Active.Should().BeTrue();
        }

        [Fact]
        public async Task SeedAsync_SundaeChocolate_ShouldHaveCorrectProperties()
        {
            await _seeder.SeedAsync();

            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.Name == "Sundae de Chocolate");

            product.Should().NotBeNull();
            product!.Price.Should().Be(10.90m);
            product.Category.Should().Be(CategoryEnum.DESSERT);
            product.Active.Should().BeTrue();
        }

        #endregion

        #region Price Range Tests

        [Fact]
        public async Task SeedAsync_ShouldHaveProductsInDifferentPriceRanges()
        {
            await _seeder.SeedAsync();

            var products = await _context.Products.ToListAsync();

            products.Should().Contain(p => p.Price < 10m);
            products.Should().Contain(p => p.Price >= 10m && p.Price < 20m);
            products.Should().Contain(p => p.Price >= 20m);
        }

        [Fact]
        public async Task SeedAsync_CheapestProduct_ShouldBeCocaCola()
        {
            await _seeder.SeedAsync();

            var cheapest = await _context.Products
                .OrderBy(p => p.Price)
                .FirstAsync();

            cheapest.Name.Should().Be("Coca-Cola 350ml");
            cheapest.Price.Should().Be(6.50m);
        }

        [Fact]
        public async Task SeedAsync_MostExpensiveProduct_ShouldBeXBacon()
        {
            await _seeder.SeedAsync();

            var mostExpensive = await _context.Products
                .OrderByDescending(p => p.Price)
                .FirstAsync();

            mostExpensive.Name.Should().Be("X-Bacon");
            mostExpensive.Price.Should().Be(29.90m);
        }

        #endregion

        #region Data Persistence Tests

        [Fact]
        public async Task SeedAsync_ShouldPersistDataToDatabase()
        {
            await _seeder.SeedAsync();

            var count = await _context.Products.CountAsync();
            count.Should().Be(12);
        }

        [Fact]
        public async Task SeedAsync_Products_ShouldHaveGeneratedIds()
        {
            await _seeder.SeedAsync();

            var products = await _context.Products.ToListAsync();
            products.Should().OnlyContain(p => p.Id > 0);
        }

        [Fact]
        public async Task SeedAsync_Products_ShouldHaveUniqueIds()
        {
            await _seeder.SeedAsync();

            var products = await _context.Products.ToListAsync();
            var ids = products.Select(p => p.Id).ToList();
            ids.Should().OnlyHaveUniqueItems();
        }

        [Fact]
        public async Task SeedAsync_Products_ShouldHaveUniqueNames()
        {
            await _seeder.SeedAsync();

            var products = await _context.Products.ToListAsync();
            var names = products.Select(p => p.Name).ToList();
            names.Should().OnlyHaveUniqueItems();
        }

        #endregion

        #region Logging Tests

        [Fact]
        public async Task SeedAsync_ShouldLogProductInsertionMessage()
        {
            await _seeder.SeedAsync();

            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => o.ToString()!.Contains("Inserindo produtos")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task SeedAsync_ShouldLogProductCountAfterInsertion()
        {
            await _seeder.SeedAsync();

            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => o.ToString()!.Contains("produtos inseridos com sucesso")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task SeedAsync_WithEmptyDatabase_ShouldLogMultipleTimes()
        {
            await _seeder.SeedAsync();

            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.AtLeast(3));
        }

        #endregion

        #region Edge Cases Tests

        [Fact]
        public async Task SeedAsync_CalledMultipleTimes_ShouldOnlySeedOnce()
        {
            await _seeder.SeedAsync();
            await _seeder.SeedAsync();
            await _seeder.SeedAsync();

            var products = await _context.Products.ToListAsync();
            products.Should().HaveCount(12);
        }

        [Fact]
        public async Task SeedAsync_AfterManualDeletion_ShouldNotSeedAgain()
        {
            await _seeder.SeedAsync();
            var productToKeep = await _context.Products.FirstAsync();
            var productsToDelete = await _context.Products.Where(p => p.Id != productToKeep.Id).ToListAsync();
            _context.Products.RemoveRange(productsToDelete);
            await _context.SaveChangesAsync();

            await _seeder.SeedAsync();

            var products = await _context.Products.ToListAsync();
            products.Should().HaveCount(1);
        }

        #endregion

        #region Category Distribution Tests

        [Fact]
        public async Task SeedAsync_ShouldHaveEqualDistributionAcrossCategories()
        {
            await _seeder.SeedAsync();

            var products = await _context.Products.ToListAsync();
            var groupedByCategory = products.GroupBy(p => p.Category).ToList();

            groupedByCategory.Should().HaveCount(4);
            groupedByCategory.Should().OnlyContain(g => g.Count() == 3);
        }

        [Fact]
        public async Task SeedAsync_EachCategory_ShouldHaveExactlyThreeProducts()
        {
            await _seeder.SeedAsync();

            var sandwichCount = await _context.Products.CountAsync(p => p.Category == CategoryEnum.SANDWICH);
            var sideCount = await _context.Products.CountAsync(p => p.Category == CategoryEnum.SIDE);
            var drinkCount = await _context.Products.CountAsync(p => p.Category == CategoryEnum.DRINK);
            var dessertCount = await _context.Products.CountAsync(p => p.Category == CategoryEnum.DESSERT);

            sandwichCount.Should().Be(3);
            sideCount.Should().Be(3);
            drinkCount.Should().Be(3);
            dessertCount.Should().Be(3);
        }

        #endregion

        #region Image URL Tests

        [Fact]
        public async Task SeedAsync_AllProducts_ShouldHaveImageUrlWithHttps()
        {
            await _seeder.SeedAsync();

            var products = await _context.Products.ToListAsync();
            products.Should().OnlyContain(p => p.ImageUrl!.StartsWith("https://"));
        }

        [Fact]
        public async Task SeedAsync_AllProducts_ShouldHaveImageUrlWithExampleDomain()
        {
            await _seeder.SeedAsync();

            var products = await _context.Products.ToListAsync();
            products.Should().OnlyContain(p => p.ImageUrl!.Contains("example.com"));
        }

        #endregion
    }
}