using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Products.Domain.Entities;
using Products.Domain.Enums;
using Products.Infrastructure.Data;

namespace Products.Tests.Infrastructure.Data
{
    public class ProductsDbContextTests
    {
        private ProductsDbContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<ProductsDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new ProductsDbContext(options);
        }

        #region DbContext Initialization Tests

        [Fact]
        public void ProductsDbContext_CanBeInstantiated()
        {
            using var context = CreateContext();

            context.Should().NotBeNull();
        }

        [Fact]
        public void ProductsDbContext_HasProductsDbSet()
        {
            using var context = CreateContext();

            context.Products.Should().NotBeNull();
        }

        [Fact]
        public void ProductsDbContext_ProductsDbSet_IsCorrectType()
        {
            using var context = CreateContext();

            context.Products.Should().BeAssignableTo<DbSet<Product>>();
        }

        #endregion

        #region Product Entity Configuration Tests

        [Fact]
        public void ProductsDbContext_ProductEntity_HasCorrectTableName()
        {
            using var context = CreateContext();
            var entityType = context.Model.FindEntityType(typeof(Product));
            var tableName = entityType!.GetTableName();

            tableName.Should().Be("products");
        }

        [Fact]
        public void ProductsDbContext_ProductEntity_HasPrimaryKey()
        {
            using var context = CreateContext();
            var entityType = context.Model.FindEntityType(typeof(Product));
            var primaryKey = entityType!.FindPrimaryKey();

            primaryKey.Should().NotBeNull();
            primaryKey!.Properties.Should().HaveCount(1);
            primaryKey.Properties.First().Name.Should().Be("Id");
        }

        [Fact]
        public void ProductsDbContext_ProductEntity_IdProperty_IsConfigured()
        {
            using var context = CreateContext();
            var entityType = context.Model.FindEntityType(typeof(Product));
            var idProperty = entityType!.FindProperty("Id");

            idProperty.Should().NotBeNull();
            idProperty!.GetColumnName().Should().Be("id");
            idProperty.ValueGenerated.Should().Be(Microsoft.EntityFrameworkCore.Metadata.ValueGenerated.OnAdd);
        }

        [Fact]
        public void ProductsDbContext_ProductEntity_NameProperty_IsConfigured()
        {
            using var context = CreateContext();
            var entityType = context.Model.FindEntityType(typeof(Product));
            var nameProperty = entityType!.FindProperty("Name");

            nameProperty.Should().NotBeNull();
            nameProperty!.GetColumnName().Should().Be("name");
            nameProperty.IsNullable.Should().BeFalse();
            nameProperty.GetMaxLength().Should().Be(200);
        }

        [Fact]
        public void ProductsDbContext_ProductEntity_PriceProperty_IsConfigured()
        {
            using var context = CreateContext();
            var entityType = context.Model.FindEntityType(typeof(Product));
            var priceProperty = entityType!.FindProperty("Price");

            priceProperty.Should().NotBeNull();
            priceProperty!.GetColumnName().Should().Be("price");
            priceProperty.IsNullable.Should().BeFalse();
        }

        [Fact]
        public void ProductsDbContext_ProductEntity_CategoryProperty_IsConfigured()
        {
            using var context = CreateContext();
            var entityType = context.Model.FindEntityType(typeof(Product));
            var categoryProperty = entityType!.FindProperty("Category");

            categoryProperty.Should().NotBeNull();
            categoryProperty!.GetColumnName().Should().Be("category");
            categoryProperty.IsNullable.Should().BeFalse();
        }

        [Fact]
        public void ProductsDbContext_ProductEntity_DescriptionProperty_IsConfigured()
        {
            using var context = CreateContext();
            var entityType = context.Model.FindEntityType(typeof(Product));
            var descriptionProperty = entityType!.FindProperty("Description");

            descriptionProperty.Should().NotBeNull();
            descriptionProperty!.GetColumnName().Should().Be("description");
            descriptionProperty.GetMaxLength().Should().Be(500);
        }

        [Fact]
        public void ProductsDbContext_ProductEntity_ActiveProperty_HasDefaultValue()
        {
            using var context = CreateContext();
            var entityType = context.Model.FindEntityType(typeof(Product));
            var activeProperty = entityType!.FindProperty("Active");

            activeProperty.Should().NotBeNull();
            activeProperty!.GetColumnName().Should().Be("active");
            activeProperty.IsNullable.Should().BeFalse();
            activeProperty.GetDefaultValue().Should().Be(true);
        }

        [Fact]
        public void ProductsDbContext_ProductEntity_ImageUrlProperty_IsConfigured()
        {
            using var context = CreateContext();
            var entityType = context.Model.FindEntityType(typeof(Product));
            var imageUrlProperty = entityType!.FindProperty("ImageUrl");

            imageUrlProperty.Should().NotBeNull();
            imageUrlProperty!.GetColumnName().Should().Be("image_url");
            imageUrlProperty.GetMaxLength().Should().Be(500);
        }

        [Fact]
        public void ProductsDbContext_ProductEntity_CreatedAtProperty_IsConfigured()
        {
            using var context = CreateContext();
            var entityType = context.Model.FindEntityType(typeof(Product));
            var createdAtProperty = entityType!.FindProperty("CreatedAt");

            createdAtProperty.Should().NotBeNull();
            createdAtProperty!.GetColumnName().Should().Be("created_at");
            createdAtProperty.IsNullable.Should().BeFalse();
        }

        [Fact]
        public void ProductsDbContext_ProductEntity_UpdatedAtProperty_IsConfigured()
        {
            using var context = CreateContext();
            var entityType = context.Model.FindEntityType(typeof(Product));
            var updatedAtProperty = entityType!.FindProperty("UpdatedAt");

            updatedAtProperty.Should().NotBeNull();
            updatedAtProperty!.GetColumnName().Should().Be("updated_at");
            updatedAtProperty.IsNullable.Should().BeFalse();
        }

        #endregion

        #region Index Tests

        [Fact]
        public void ProductsDbContext_ProductEntity_HasCategoryIndex()
        {
            using var context = CreateContext();
            var entityType = context.Model.FindEntityType(typeof(Product));
            var indexes = entityType!.GetIndexes();
            var categoryIndex = indexes.FirstOrDefault(i =>
                i.Properties.Any(p => p.Name == "Category"));

            categoryIndex.Should().NotBeNull();
        }

        [Fact]
        public void ProductsDbContext_ProductEntity_HasActiveIndex()
        {
            using var context = CreateContext();
            var entityType = context.Model.FindEntityType(typeof(Product));
            var indexes = entityType!.GetIndexes();
            var activeIndex = indexes.FirstOrDefault(i =>
                i.Properties.Any(p => p.Name == "Active"));

            activeIndex.Should().NotBeNull();
        }

        [Fact]
        public void ProductsDbContext_ProductEntity_HasTwoIndexes()
        {
            using var context = CreateContext();
            var entityType = context.Model.FindEntityType(typeof(Product));
            var indexes = entityType!.GetIndexes();

            indexes.Count().Should().BeGreaterThanOrEqualTo(2);
        }

        #endregion

        #region CRUD Operations Tests

        [Fact]
        public async Task ProductsDbContext_CanAddProduct()
        {
            using var context = CreateContext();
            var product = new Product
            {
                Name = "Produto Teste",
                Price = 50.00m,
                Category = CategoryEnum.SANDWICH,
                Active = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            context.Products.Add(product);
            await context.SaveChangesAsync();

            var savedProduct = await context.Products.FirstOrDefaultAsync(p => p.Name == "Produto Teste");
            savedProduct.Should().NotBeNull();
            savedProduct!.Name.Should().Be("Produto Teste");
        }

        [Fact]
        public async Task ProductsDbContext_CanAddProductWithAllProperties()
        {
            using var context = CreateContext();
            var product = new Product
            {
                Name = "Produto Completo",
                Price = 99.99m,
                Category = CategoryEnum.DRINK,
                Description = "Descrição completa do produto",
                Active = true,
                ImageUrl = "https://example.com/image.jpg",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            context.Products.Add(product);
            await context.SaveChangesAsync();

            var savedProduct = await context.Products.FirstOrDefaultAsync(p => p.Name == "Produto Completo");
            savedProduct.Should().NotBeNull();
            savedProduct!.Description.Should().Be("Descrição completa do produto");
            savedProduct.ImageUrl.Should().Be("https://example.com/image.jpg");
        }

        [Fact]
        public async Task ProductsDbContext_CanUpdateProduct()
        {
            using var context = CreateContext();
            var product = new Product
            {
                Name = "Produto Original",
                Price = 50.00m,
                Category = CategoryEnum.SANDWICH,
                Active = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            context.Products.Add(product);
            await context.SaveChangesAsync();

            product.Name = "Produto Atualizado";
            product.Price = 75.00m;
            product.UpdatedAt = DateTime.UtcNow;
            await context.SaveChangesAsync();

            var updatedProduct = await context.Products.FindAsync(product.Id);
            updatedProduct!.Name.Should().Be("Produto Atualizado");
            updatedProduct.Price.Should().Be(75.00m);
        }

        [Fact]
        public async Task ProductsDbContext_CanDeleteProduct()
        {
            using var context = CreateContext();
            var product = new Product
            {
                Name = "Produto Para Deletar",
                Price = 30.00m,
                Category = CategoryEnum.SIDE,
                Active = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            context.Products.Add(product);
            await context.SaveChangesAsync();

            context.Products.Remove(product);
            await context.SaveChangesAsync();

            var deletedProduct = await context.Products.FindAsync(product.Id);
            deletedProduct.Should().BeNull();
        }

        [Fact]
        public async Task ProductsDbContext_CanAddMultipleProducts()
        {
            using var context = CreateContext();
            var products = new List<Product>
            {
                new Product
                {
                    Name = "Produto 1",
                    Price = 10.00m,
                    Category = CategoryEnum.SANDWICH,
                    Active = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Product
                {
                    Name = "Produto 2",
                    Price = 20.00m,
                    Category = CategoryEnum.DRINK,
                    Active = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            };

            context.Products.AddRange(products);
            await context.SaveChangesAsync();

            var savedProducts = await context.Products.ToListAsync();
            savedProducts.Should().HaveCount(2);
        }

        #endregion

        #region Query Tests

        [Fact]
        public async Task ProductsDbContext_CanQueryAllProducts()
        {
            using var context = CreateContext();
            var products = new[]
            {
                new Product
                {
                    Name = "Produto A",
                    Price = 10.00m,
                    Category = CategoryEnum.SANDWICH,
                    Active = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Product
                {
                    Name = "Produto B",
                    Price = 20.00m,
                    Category = CategoryEnum.DRINK,
                    Active = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            };
            context.Products.AddRange(products);
            await context.SaveChangesAsync();

            var result = await context.Products.ToListAsync();

            result.Should().HaveCount(2);
        }

        [Fact]
        public async Task ProductsDbContext_CanFilterByCategory()
        {
            using var context = CreateContext();
            var products = new[]
            {
                new Product
                {
                    Name = "Lanche",
                    Price = 15.00m,
                    Category = CategoryEnum.SANDWICH,
                    Active = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Product
                {
                    Name = "Bebida",
                    Price = 5.00m,
                    Category = CategoryEnum.DRINK,
                    Active = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            };
            context.Products.AddRange(products);
            await context.SaveChangesAsync();

            var sandwiches = await context.Products
                .Where(p => p.Category == CategoryEnum.SANDWICH)
                .ToListAsync();

            sandwiches.Should().HaveCount(1);
            sandwiches.First().Category.Should().Be(CategoryEnum.SANDWICH);
        }

        [Fact]
        public async Task ProductsDbContext_CanFilterByActive()
        {
            using var context = CreateContext();
            var products = new[]
            {
                new Product
                {
                    Name = "Produto Ativo",
                    Price = 10.00m,
                    Category = CategoryEnum.SANDWICH,
                    Active = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Product
                {
                    Name = "Produto Inativo",
                    Price = 20.00m,
                    Category = CategoryEnum.DRINK,
                    Active = false,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            };
            context.Products.AddRange(products);
            await context.SaveChangesAsync();

            var activeProducts = await context.Products
                .Where(p => p.Active)
                .ToListAsync();

            activeProducts.Should().HaveCount(1);
            activeProducts.First().Active.Should().BeTrue();
        }

        [Fact]
        public async Task ProductsDbContext_CanFilterByPriceRange()
        {
            using var context = CreateContext();
            var products = new[]
            {
                new Product
                {
                    Name = "Barato",
                    Price = 5.00m,
                    Category = CategoryEnum.SIDE,
                    Active = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Product
                {
                    Name = "Médio",
                    Price = 15.00m,
                    Category = CategoryEnum.SANDWICH,
                    Active = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Product
                {
                    Name = "Caro",
                    Price = 50.00m,
                    Category = CategoryEnum.DESSERT,
                    Active = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            };
            context.Products.AddRange(products);
            await context.SaveChangesAsync();

            var mediumPriceProducts = await context.Products
                .Where(p => p.Price >= 10.00m && p.Price <= 20.00m)
                .ToListAsync();

            mediumPriceProducts.Should().HaveCount(1);
            mediumPriceProducts.First().Name.Should().Be("Médio");
        }

        [Fact]
        public async Task ProductsDbContext_CanOrderByPrice()
        {
            using var context = CreateContext();
            var products = new[]
            {
                new Product
                {
                    Name = "Produto C",
                    Price = 30.00m,
                    Category = CategoryEnum.SANDWICH,
                    Active = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Product
                {
                    Name = "Produto A",
                    Price = 10.00m,
                    Category = CategoryEnum.DRINK,
                    Active = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Product
                {
                    Name = "Produto B",
                    Price = 20.00m,
                    Category = CategoryEnum.SIDE,
                    Active = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            };
            context.Products.AddRange(products);
            await context.SaveChangesAsync();

            var orderedProducts = await context.Products
                .OrderBy(p => p.Price)
                .ToListAsync();

            orderedProducts.Should().HaveCount(3);
            orderedProducts.First().Price.Should().Be(10.00m);
            orderedProducts.Last().Price.Should().Be(30.00m);
        }

        [Fact]
        public async Task ProductsDbContext_CanSearchByName()
        {
            using var context = CreateContext();
            var products = new[]
            {
                new Product
                {
                    Name = "Hambúrguer Especial",
                    Price = 25.00m,
                    Category = CategoryEnum.SANDWICH,
                    Active = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Product
                {
                    Name = "Refrigerante",
                    Price = 5.00m,
                    Category = CategoryEnum.DRINK,
                    Active = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            };
            context.Products.AddRange(products);
            await context.SaveChangesAsync();

            var searchResults = await context.Products
                .Where(p => p.Name.Contains("Hambúrguer"))
                .ToListAsync();

            searchResults.Should().HaveCount(1);
            searchResults.First().Name.Should().Contain("Hambúrguer");
        }

        #endregion

        #region Category Enum Tests

        [Fact]
        public async Task ProductsDbContext_CanSaveAllCategoryTypes()
        {
            using var context = CreateContext();
            var products = new[]
            {
                new Product
                {
                    Name = "Lanche",
                    Price = 20.00m,
                    Category = CategoryEnum.SANDWICH,
                    Active = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Product
                {
                    Name = "Acompanhamento",
                    Price = 10.00m,
                    Category = CategoryEnum.SIDE,
                    Active = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Product
                {
                    Name = "Bebida",
                    Price = 5.00m,
                    Category = CategoryEnum.DRINK,
                    Active = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Product
                {
                    Name = "Sobremesa",
                    Price = 15.00m,
                    Category = CategoryEnum.DESSERT,
                    Active = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            };

            context.Products.AddRange(products);
            await context.SaveChangesAsync();

            // Assert
            var savedProducts = await context.Products.ToListAsync();
            savedProducts.Should().HaveCount(4);
            savedProducts.Select(p => p.Category).Should().Contain(new[]
            {
                CategoryEnum.SANDWICH,
                CategoryEnum.SIDE,
                CategoryEnum.DRINK,
                CategoryEnum.DESSERT
            });
        }

        #endregion

        #region SaveChanges Tests

        [Fact]
        public async Task ProductsDbContext_SaveChangesAsync_ReturnsSavedEntitiesCount()
        {
            using var context = CreateContext();
            var product = new Product
            {
                Name = "Produto Teste",
                Price = 10.00m,
                Category = CategoryEnum.SANDWICH,
                Active = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            context.Products.Add(product);

            var result = await context.SaveChangesAsync();

            result.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task ProductsDbContext_SaveChanges_PersistsData()
        {
            var product = new Product
            {
                Name = "Produto Persistente",
                Price = 40.00m,
                Category = CategoryEnum.DESSERT,
                Active = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            int productId;
            var options = new DbContextOptionsBuilder<ProductsDbContext>()
                .UseInMemoryDatabase(databaseName: "PersistenceTest")
                .Options;

            using (var context = new ProductsDbContext(options))
            {
                context.Products.Add(product);
                await context.SaveChangesAsync();
                productId = product.Id;
            }

            using (var context = new ProductsDbContext(options))
            {
                var savedProduct = await context.Products.FindAsync(productId);
                savedProduct.Should().NotBeNull();
                savedProduct!.Name.Should().Be("Produto Persistente");
            }
        }

        #endregion

        #region Edge Cases Tests

        [Fact]
        public async Task ProductsDbContext_CanSaveProductWithZeroPrice()
        {
            using var context = CreateContext();
            var product = new Product
            {
                Name = "Produto Grátis",
                Price = 0.00m,
                Category = CategoryEnum.SIDE,
                Active = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            context.Products.Add(product);
            await context.SaveChangesAsync();

            var savedProduct = await context.Products.FirstOrDefaultAsync(p => p.Name == "Produto Grátis");
            savedProduct.Should().NotBeNull();
            savedProduct!.Price.Should().Be(0.00m);
        }

        [Fact]
        public async Task ProductsDbContext_CanSaveProductWithNullDescription()
        {
            using var context = CreateContext();
            var product = new Product
            {
                Name = "Produto Sem Descrição",
                Price = 25.00m,
                Category = CategoryEnum.SANDWICH,
                Description = null,
                Active = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            context.Products.Add(product);
            await context.SaveChangesAsync();

            var savedProduct = await context.Products
                .FirstOrDefaultAsync(p => p.Name == "Produto Sem Descrição");
            savedProduct.Should().NotBeNull();
            savedProduct!.Description.Should().BeNull();
        }

        [Fact]
        public async Task ProductsDbContext_CanSaveProductWithNullImageUrl()
        {
            using var context = CreateContext();
            var product = new Product
            {
                Name = "Produto Sem Imagem",
                Price = 30.00m,
                Category = CategoryEnum.DRINK,
                ImageUrl = null,
                Active = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            context.Products.Add(product);
            await context.SaveChangesAsync();

            var savedProduct = await context.Products
                .FirstOrDefaultAsync(p => p.Name == "Produto Sem Imagem");
            savedProduct.Should().NotBeNull();
            savedProduct!.ImageUrl.Should().BeNull();
        }

        [Fact]
        public async Task ProductsDbContext_CanSaveInactiveProduct()
        {
            using var context = CreateContext();
            var product = new Product
            {
                Name = "Produto Inativo",
                Price = 20.00m,
                Category = CategoryEnum.SANDWICH,
                Active = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            context.Products.Add(product);
            await context.SaveChangesAsync();

            var savedProduct = await context.Products
                .FirstOrDefaultAsync(p => p.Name == "Produto Inativo");
            savedProduct.Should().NotBeNull();
            savedProduct!.Active.Should().BeFalse();
        }

        #endregion

        #region Count and Aggregate Tests

        [Fact]
        public async Task ProductsDbContext_CanCountProducts()
        {
            using var context = CreateContext();
            var products = Enumerable.Range(1, 5).Select(i => new Product
            {
                Name = $"Produto {i}",
                Price = i * 10.00m,
                Category = CategoryEnum.SANDWICH,
                Active = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            });
            context.Products.AddRange(products);
            await context.SaveChangesAsync();

            var count = await context.Products.CountAsync();

            count.Should().Be(5);
        }

        [Fact]
        public async Task ProductsDbContext_CanCalculateAveragePrice()
        {
            using var context = CreateContext();
            var products = new[]
            {
                new Product
                {
                    Name = "Produto 1",
                    Price = 10.00m,
                    Category = CategoryEnum.SANDWICH,
                    Active = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Product
                {
                    Name = "Produto 2",
                    Price = 20.00m,
                    Category = CategoryEnum.DRINK,
                    Active = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Product
                {
                    Name = "Produto 3",
                    Price = 30.00m,
                    Category = CategoryEnum.SIDE,
                    Active = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            };
            context.Products.AddRange(products);
            await context.SaveChangesAsync();

            var averagePrice = await context.Products.AverageAsync(p => p.Price);

            averagePrice.Should().Be(20.00m);
        }

        #endregion
    }
}