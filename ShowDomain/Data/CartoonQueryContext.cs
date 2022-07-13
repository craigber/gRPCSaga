using CartoonDomain.Service.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CartoonDomain.Service.Data;

public class CartoonQueryContext : DbContext
{
    public DbSet<Cartoon> Cartoons { get; set; }
    public DbSet<Character> Characters { get; set; }

    public CartoonQueryContext(DbContextOptions<CartoonQueryContext> options) : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //modelBuilder.UseSqlite("Filename=Cartoonalogue.db");
        modelBuilder.Entity<Cartoon>().ToTable("Cartoons");
        
    }
}
