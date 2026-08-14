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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

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
    }
}