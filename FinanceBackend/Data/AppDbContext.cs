using Microsoft.EntityFrameworkCore;
using FinanceBackend.Domain;

namespace FinanceBackend.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<FinancialRecord> FinancialRecords { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Configuration can go here if needed
        modelBuilder.Entity<FinancialRecord>()
            .Property(f => f.Amount)
            .HasColumnType("decimal(18,2)");
    }
}
