using FluentAssertions;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Moq;
using Products.API.Data;
using Products.API.Extensions;
using Products.Infrastructure.Data;

namespace Products.Tests.API.Extensions
{
    public class DatabaseExtensionsTests : IDisposable
    {
        private readonly Mock<ILogger<Program>> _loggerMock;
        private readonly Mock<ILogger<DatabaseSeeder>> _seederLoggerMock;
        private readonly ServiceCollection _services;
        private ServiceProvider _serviceProvider;
        private readonly Mock<IApplicationBuilder> _appBuilderMock;
        private ProductsDbContext _context;
        private DatabaseSeeder _seeder;

        public DatabaseExtensionsTests()
        {
            _loggerMock = new Mock<ILogger<Program>>();
            _seederLoggerMock = new Mock<ILogger<DatabaseSeeder>>();
            _services = new ServiceCollection();
            _appBuilderMock = new Mock<IApplicationBuilder>();

            SetupServices();
        }

        private void SetupServices()
        {
            var options = new DbContextOptionsBuilder<ProductsDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ProductsDbContext(options);
            _seeder = new DatabaseSeeder(_context, _seederLoggerMock.Object);

            _services.Clear();
            _services.AddSingleton(_context);
            _services.AddSingleton(_seeder);
            _services.AddSingleton<ILogger<Program>>(_loggerMock.Object);

            _serviceProvider?.Dispose();
            _serviceProvider = _services.BuildServiceProvider();
            _appBuilderMock.Setup(a => a.ApplicationServices).Returns(_serviceProvider);
        }

        public void Dispose()
        {
            _context?.Database.EnsureDeleted();
            _context?.Dispose();
            _serviceProvider?.Dispose();
        }

        #region ApplyMigrations Tests

        [Fact]
        public void ApplyMigrations_WithNullLogger_ThrowsException()
        {
            var servicesWithoutLogger = new ServiceCollection();
            servicesWithoutLogger.AddSingleton(_context);
            var providerWithoutLogger = servicesWithoutLogger.BuildServiceProvider();

            var appBuilderMock = new Mock<IApplicationBuilder>();
            appBuilderMock.Setup(a => a.ApplicationServices).Returns(providerWithoutLogger);

            Action act = () => appBuilderMock.Object.ApplyMigrations();

            act.Should().Throw<InvalidOperationException>();

            providerWithoutLogger.Dispose();
        }

        [Fact]
        public void ApplyMigrations_WithNullContext_ThrowsException()
        {
            var servicesWithoutContext = new ServiceCollection();
            servicesWithoutContext.AddSingleton<ILogger<Program>>(_loggerMock.Object);
            var providerWithoutContext = servicesWithoutContext.BuildServiceProvider();

            var appBuilderMock = new Mock<IApplicationBuilder>();
            appBuilderMock.Setup(a => a.ApplicationServices).Returns(providerWithoutContext);

            Action act = () => appBuilderMock.Object.ApplyMigrations();

            act.Should().Throw<InvalidOperationException>();

            providerWithoutContext.Dispose();
        }

        #endregion

        #region SeedDatabase Tests

        [Fact]
        public void SeedDatabase_ExecutesSeeder()
        {
            var result = _appBuilderMock.Object.SeedDatabase();

            result.Should().NotBeNull();

            _seederLoggerMock.Verify(
                x => x.Log(
                    It.IsAny<LogLevel>(),
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.AtLeastOnce);
        }

        [Fact]
        public void SeedDatabase_ReturnsApplicationBuilder()
        {
            var result = _appBuilderMock.Object.SeedDatabase();

            result.Should().BeSameAs(_appBuilderMock.Object);
        }

        [Fact]
        public void SeedDatabase_CreatesServiceScope()
        {
            _appBuilderMock.Object.SeedDatabase();

            _appBuilderMock.Verify(a => a.ApplicationServices, Times.AtLeastOnce);
        }

        [Fact]
        public void SeedDatabase_WithNullSeeder_ThrowsException()
        {
            var servicesWithoutSeeder = new ServiceCollection();
            servicesWithoutSeeder.AddSingleton(_context);
            servicesWithoutSeeder.AddSingleton<ILogger<Program>>(_loggerMock.Object);
            var providerWithoutSeeder = servicesWithoutSeeder.BuildServiceProvider();

            var appBuilderMock = new Mock<IApplicationBuilder>();
            appBuilderMock.Setup(a => a.ApplicationServices).Returns(providerWithoutSeeder);

            Action act = () => appBuilderMock.Object.SeedDatabase();

            act.Should().Throw<InvalidOperationException>();

            providerWithoutSeeder.Dispose();
        }

        [Fact]
        public void SeedDatabase_InsertsProductsIntoDatabase()
        {
            _appBuilderMock.Object.SeedDatabase();

            var products = _context.Products.ToList();
            products.Should().NotBeEmpty();
        }

        [Fact]
        public void SeedDatabase_CalledMultipleTimes_DoesNotDuplicateProducts()
        {
            _appBuilderMock.Object.SeedDatabase();
            var firstCount = _context.Products.Count();

            _appBuilderMock.Object.SeedDatabase();
            var secondCount = _context.Products.Count();

            secondCount.Should().Be(firstCount);
        }

        [Fact]
        public void SeedDatabase_CompletesSuccessfully()
        {
            Action act = () => _appBuilderMock.Object.SeedDatabase();

            act.Should().NotThrow();
        }

        [Fact]
        public void SeedDatabase_LogsSeederMessages()
        {
            _appBuilderMock.Object.SeedDatabase();

            _seederLoggerMock.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) => o.ToString()!.Contains("Iniciando seed") ||
                                                   o.ToString()!.Contains("Seed concluído")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.AtLeastOnce);
        }

        [Fact]
        public void SeedDatabase_ReturnsNonNullResult()
        {
            var result = _appBuilderMock.Object.SeedDatabase();

            result.Should().NotBeNull();
        }

        [Fact]
        public void SeedDatabase_AccessesApplicationServices()
        {
            _appBuilderMock.Object.SeedDatabase();

            _appBuilderMock.Verify(a => a.ApplicationServices, Times.AtLeastOnce);
        }

        [Fact]
        public void SeedDatabase_WithEmptyDatabase_SeedsProducts()
        {
            var initialCount = _context.Products.Count();
            initialCount.Should().Be(0);

            _appBuilderMock.Object.SeedDatabase();

            var finalCount = _context.Products.Count();
            finalCount.Should().BeGreaterThan(0);
        }

        #endregion

        #region Error Handling Tests

        [Fact]
        public void ApplyMigrations_WithDisposedContext_ThrowsException()
        {
            var disposableOptions = new DbContextOptionsBuilder<ProductsDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            var disposableContext = new ProductsDbContext(disposableOptions);

            var disposableServices = new ServiceCollection();
            disposableServices.AddSingleton(disposableContext);
            disposableServices.AddSingleton<ILogger<Program>>(_loggerMock.Object);
            var disposableProvider = disposableServices.BuildServiceProvider();

            var disposableAppBuilder = new Mock<IApplicationBuilder>();
            disposableAppBuilder.Setup(a => a.ApplicationServices).Returns(disposableProvider);

            disposableContext.Dispose();

            Action act = () => disposableAppBuilder.Object.ApplyMigrations();

            act.Should().Throw<Exception>();

            disposableProvider.Dispose();
        }

        [Fact]
        public void ApplyMigrations_WhenExceptionOccurs_RethrowsException()
        {
            var disposableOptions = new DbContextOptionsBuilder<ProductsDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            var disposableContext = new ProductsDbContext(disposableOptions);

            var disposableServices = new ServiceCollection();
            disposableServices.AddSingleton(disposableContext);
            disposableServices.AddSingleton<ILogger<Program>>(_loggerMock.Object);
            var disposableProvider = disposableServices.BuildServiceProvider();

            var disposableAppBuilder = new Mock<IApplicationBuilder>();
            disposableAppBuilder.Setup(a => a.ApplicationServices).Returns(disposableProvider);

            disposableContext.Dispose();

            Action act = () => disposableAppBuilder.Object.ApplyMigrations();

            act.Should().Throw<Exception>();

            disposableProvider.Dispose();
        }

        [Fact]
        public void SeedDatabase_UsesCorrectServiceProvider()
        {
            _appBuilderMock.Object.SeedDatabase();

            _appBuilderMock.Verify(
                a => a.ApplicationServices,
                Times.AtLeastOnce,
                "Should access ApplicationServices to create scope");
        }

        [Fact]
        public void SeedDatabase_ExecutesSynchronously()
        {
            var startTime = DateTime.Now;

            _appBuilderMock.Object.SeedDatabase();

            var endTime = DateTime.Now;
            var duration = endTime - startTime;

            duration.Should().BeLessThan(TimeSpan.FromSeconds(5),
                "SeedDatabase should complete quickly for in-memory database");
        }

        [Fact]
        public void SeedDatabase_WithPreExistingData_PreservesData()
        {
            _appBuilderMock.Object.SeedDatabase();
            var firstProducts = _context.Products.ToList();
            var firstProductIds = firstProducts.Select(p => p.Id).ToList();

            _appBuilderMock.Object.SeedDatabase();
            var secondProducts = _context.Products.ToList();
            var secondProductIds = secondProducts.Select(p => p.Id).ToList();

            secondProductIds.Should().BeEquivalentTo(firstProductIds);
        }

        #endregion
    }
}