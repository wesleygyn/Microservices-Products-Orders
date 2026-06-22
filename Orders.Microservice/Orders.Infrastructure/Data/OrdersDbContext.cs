using Microsoft.EntityFrameworkCore;
using Orders.Domain.Entities;
using Orders.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.Infrastructure.Data
{
    public class OrdersDbContext : DbContext
    {
        public OrdersDbContext(DbContextOptions<OrdersDbContext> options) : base(options)
        {
        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("orders");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.CustomerId)
                    .HasColumnName("customer_id");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasConversion<string>()
                    .IsRequired();

                entity.Property(e => e.Observation)
                    .HasColumnName("observation")
                    .HasMaxLength(500);

                entity.Property(e => e.Number)
                    .HasColumnName("number")
                    .IsRequired();

                entity.Property(e => e.PaymentId)
                    .HasColumnName("payment_id")
                    .HasMaxLength(100);

                entity.Property(e => e.QrCode)
                    .HasColumnName("qr_code")
                    .HasMaxLength(500);

                entity.Property(e => e.PaymentStatus)
                    .HasColumnName("payment_status")
                    .HasConversion<string>()
                    .IsRequired()
                    .HasDefaultValue(PaymentStatusEnum.PENDING);

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("datetime(6)")
                    .IsRequired();

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasColumnType("datetime(6)")
                    .IsRequired();

                entity.HasMany(e => e.Items)
                    .WithOne(i => i.Order)
                    .HasForeignKey(i => i.OrderId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(e => e.CustomerId).HasDatabaseName("idx_orders_customer_id");
                entity.HasIndex(e => e.Status).HasDatabaseName("idx_orders_status");
                entity.HasIndex(e => e.Number).HasDatabaseName("idx_orders_number").IsUnique();

                entity.HasCheckConstraint("chk_orders_number_positive", "number > 0");
            });

            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.ToTable("order_items");
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.OrderId)
                    .HasColumnName("order_id")
                    .IsRequired();

                entity.Property(e => e.ProductId)
                    .HasColumnName("product_id")
                    .IsRequired();

                entity.Property(e => e.ProductName)
                    .HasColumnName("product_name")
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Quantity)
                    .HasColumnName("quantity")
                    .IsRequired();

                entity.Property(e => e.UnitPrice)
                    .HasColumnName("unit_price")
                    .HasColumnType("decimal(10,2)")
                    .IsRequired();

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasColumnType("datetime(6)")
                    .IsRequired();

                entity.HasIndex(e => e.ProductId).HasDatabaseName("idx_order_items_product_id");

                entity.HasCheckConstraint("chk_order_items_quantity_positive", "quantity > 0");
                entity.HasCheckConstraint("chk_order_items_price_positive", "unit_price >= 0.00");
            });
        }
    }
}