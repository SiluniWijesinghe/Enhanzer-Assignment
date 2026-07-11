using EnhanzerApi.Models;
using Microsoft.EntityFrameworkCore;

namespace EnhanzerApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<LocationDetail> LocationDetails => Set<LocationDetail>();
    public DbSet<PurchaseBillHeader> PurchaseBillHeaders => Set<PurchaseBillHeader>();
    public DbSet<PurchaseBillLine> PurchaseBillLines => Set<PurchaseBillLine>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PurchaseBillHeader>()
            .HasMany(h => h.Lines)
            .WithOne(l => l.PurchaseBillHeader)
            .HasForeignKey(l => l.PurchaseBillHeaderId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<LocationDetail>()
            .HasIndex(l => new { l.CompanyCode, l.Location_Code });
    }
}
