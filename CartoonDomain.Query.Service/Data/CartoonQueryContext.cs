using CartoonDomain.Common.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CartoonDomain.Query.Service.Data;

public class CartoonQueryContext : DbContext
{
    public DbSet<Cartoon> Cartoons { get; set; }
    public DbSet<Character> Characters { get; set; }

    public CartoonQueryContext(DbContextOptions<CartoonQueryContext> options) : base(options)
    { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite(@"Filename=..\Cartoons.db");
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cartoon>().ToTable("Cartoons");
    }
}
