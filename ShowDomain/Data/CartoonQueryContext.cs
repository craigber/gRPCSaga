using CartoonDomain.Service.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CartoonDomain.Service.Data;

public class CartoonQueryContext : DbContext
{
    public DbSet<Cartoon> Cartoons { get; set; }

    public CartoonQueryContext(DbContextOptions<CartoonQueryContext> options) : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //modelBuilder.UseSqlite("Filename=Cartoonalogue.db");
        modelBuilder.Entity<Cartoon>().ToTable("Cartoons");

        SeedData(modelBuilder);
    }

    private void SeedData(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cartoon>().HasData(
            new Cartoon
            {
                Id = 1,
                Title = "Rocky and Friends",
                YearBegin = 1959,
                YearEnd = 1963,
                Description = "The adventures of Moose and Squirrel as they fight the bad guys from Potsylvania.",
                Rating = 5.0m
            },
            new Cartoon
            {
                Id = 2,
                Title = "The Simpsons",
                YearBegin = 1989,
                Description = "Just your typical American family.",
                Rating = 5.0m
            }
        );
    }
}
