using CartoonDomain.Common.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CartoonDomain.Service.Data;

public class CartoonCommandContext : DbContext
{
    public DbSet<Cartoon> Cartoons { get; set; }
    public DbSet<Character> Characters { get; set; }

    public CartoonCommandContext(DbContextOptions<CartoonCommandContext> options) : base(options)
    { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Filename=Cartoons.db");
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cartoon>().ToTable("Cartoons");
    }
}
