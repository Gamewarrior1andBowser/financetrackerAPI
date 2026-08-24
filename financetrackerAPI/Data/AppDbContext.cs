using Microsoft.EntityFrameworkCore;
using financetrackerAPI.Models;

namespace financetrackerAPI.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(
        DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Budget> Budgets { get; set; }

    // Student B
    public DbSet<Wallet> Wallets { get; set; }
    public DbSet<Asset> Assets { get; set; }
    public DbSet<PortfolioHolding> PortfolioHoldings { get; set; }
    public DbSet<TradeLedger> TradeLedgers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Student A
        modelBuilder.Entity<Category>()
            .HasKey(c => c.categoryID);

        modelBuilder.Entity<Transaction>()
            .HasKey(t => t.transactionsID);

        modelBuilder.Entity<Transaction>()
            .HasOne(t => t.Category)
            .WithMany()
            .HasForeignKey(t => t.categoryID)
            .HasPrincipalKey(c => c.categoryID)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Budget>()
            .HasKey(t => t.budgetID);

        modelBuilder.Entity<Budget>()
            .HasOne(t => t.Category)
            .WithMany()
            .HasForeignKey(t => t.categoryID)
            .HasPrincipalKey(c => c.categoryID)
            .OnDelete(DeleteBehavior.Cascade);

        // Student B - Wallet
        modelBuilder.Entity<Wallet>()
            .HasOne<User>()
            .WithOne()
            .HasForeignKey<Wallet>(w => w.userID);

        modelBuilder.Entity<Wallet>()
            .HasIndex(w => w.userID)
            .IsUnique();

        modelBuilder.Entity<Wallet>()
            .Property(w => w.AvailableCash)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Wallet>()
            .Property(w => w.InitialCash)
            .HasPrecision(18, 2);

        // Student B - Asset
        modelBuilder.Entity<Asset>()
            .HasIndex(a => a.Symbol)
            .IsUnique();

        modelBuilder.Entity<Asset>()
            .Property(a => a.CurrentPrice)
            .HasPrecision(18, 4);

        // Student B - PortfolioHolding
        modelBuilder.Entity<PortfolioHolding>()
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(p => p.userID)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<PortfolioHolding>()
            .HasOne<Asset>()
            .WithMany()
            .HasForeignKey(p => p.assetID)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<PortfolioHolding>()
            .HasIndex(p => new { p.userID, p.assetID })
            .IsUnique();

        modelBuilder.Entity<PortfolioHolding>()
            .Property(p => p.Quantity)
            .HasPrecision(18, 6);

        modelBuilder.Entity<PortfolioHolding>()
            .Property(p => p.AveragePurchasePrice)
            .HasPrecision(18, 4);

        // Student B - TradeLedger
        modelBuilder.Entity<TradeLedger>()
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(t => t.userID)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<TradeLedger>()
            .HasOne<Asset>()
            .WithMany()
            .HasForeignKey(t => t.assetID)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<TradeLedger>()
            .Property(t => t.Quantity)
            .HasPrecision(18, 6);

        modelBuilder.Entity<TradeLedger>()
            .Property(t => t.ExecutionPrice)
            .HasPrecision(18, 4);

        modelBuilder.Entity<TradeLedger>()
            .Property(t => t.TotalValue)
            .HasPrecision(18, 2);
    }
}