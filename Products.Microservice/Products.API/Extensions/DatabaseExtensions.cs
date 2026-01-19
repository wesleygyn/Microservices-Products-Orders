using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Products.API.Data;
using Products.Infrastructure.Data;
using System;
using System.Linq;

namespace Products.API.Extensions
{
    public static class DatabaseExtensions
    {
        public static IApplicationBuilder ApplyMigrations(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var services = scope.ServiceProvider;
            var logger = services.GetRequiredService<ILogger<Program>>();

            try
            {
                var context = services.GetRequiredService<ProductsDbContext>();

                logger.LogInformation("🔄 Verificando migrations pendentes...");

                var pendingMigrations = context.Database.GetPendingMigrations().ToList();

                if (pendingMigrations.Any())
                {
                    logger.LogInformation("📦 Aplicando {Count} migration(s) pendente(s)...", pendingMigrations.Count);
                    context.Database.Migrate();
                    logger.LogInformation("✅ Migrations aplicadas com sucesso!");
                }
                else
                {
                    logger.LogInformation("✅ Banco de dados está atualizado!");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "❌ Erro ao aplicar migrations");
                throw;
            }

            return app;
        }

        public static IApplicationBuilder SeedDatabase(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var services = scope.ServiceProvider;
            var seeder = services.GetRequiredService<DatabaseSeeder>();

            seeder.SeedAsync().GetAwaiter().GetResult();

            return app;
        }
    }
}