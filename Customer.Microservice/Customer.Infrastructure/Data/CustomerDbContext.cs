using Microsoft.EntityFrameworkCore;

namespace Customer.Infrastructure.Data;

public class CustomerDbContext : DbContext
{
    public CustomerDbContext(DbContextOptions<CustomerDbContext> options) : base(options) { }

    public DbSet<Customer.Domain.Entities.Customer> Customers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Customer.Domain.Entities.Customer>(entity =>
        {
            entity.ToTable("customers");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasColumnName("id")
                .HasColumnType("char(36)");

            entity.Property(e => e.Name)
                .HasColumnName("name")
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.Email)
                .HasColumnName("email")
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(e => e.Cpf)
                .HasColumnName("cpf")
                .IsRequired()
                .HasMaxLength(11);

            entity.Property(e => e.Active)
                .HasColumnName("active")
                .IsRequired()
                .HasDefaultValue(true);

            entity.Property(e => e.CreatedAt)
                .HasColumnName("created_at")
                .HasColumnType("datetime(6)")
                .IsRequired();

            entity.Property(e => e.UpdatedAt)
                .HasColumnName("updated_at")
                .HasColumnType("datetime(6)")
                .IsRequired();

            entity.HasIndex(e => e.Cpf).IsUnique().HasDatabaseName("idx_customers_cpf");
            entity.HasIndex(e => e.Email).HasDatabaseName("idx_customers_email");
            entity.HasIndex(e => e.Active).HasDatabaseName("idx_customers_active");
        });
    }
}