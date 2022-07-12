using CartoonDomain.Service.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CartoonDomain.Service.Data;

public class CartoonContext : DbContext
{
    public DbSet<Cartoon> Cartoons { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cartoon>().ToTable("Cartoons");
    }
}
