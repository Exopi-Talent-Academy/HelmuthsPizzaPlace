using Microsoft.EntityFrameworkCore;

namespace PizzaPlace.Data;

public sealed class PizzaDbContext(DbContextOptions<PizzaDbContext> options)
    : DbContext(options)
{
    public DbSet<StockEntity> Stock => Set<StockEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<StockEntity>(entity =>
        {
            entity.HasKey(stock => stock.Id);

            entity.HasIndex(stock => stock.StockType)
                .IsUnique();

            entity.Property(stock => stock.StockType)
                .HasConversion<string>();

            entity.Property(stock => stock.Amount)
                .IsRequired();
        });
    }
}