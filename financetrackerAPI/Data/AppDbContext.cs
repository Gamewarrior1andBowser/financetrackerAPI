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



        modelBuilder.Entity<Transaction>()
            .HasOne(t => t.Category)
            .WithMany(c => c.Transactions)
            .HasForeignKey(t => t.categoryID)
            .OnDelete(DeleteBehavior.Cascade);



        modelBuilder.Entity<Transaction>()
            .Property(t => t.Amount)
            .HasPrecision(10, 2);

    }

}