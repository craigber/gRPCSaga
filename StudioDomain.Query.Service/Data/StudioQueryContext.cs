using StudioDomain.Common.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace StudioDomain.Service.Data;

public class StudioQueryContext : DbContext
{
    public DbSet<Studio> Studios { get; set; }
    public StudioQueryContext(DbContextOptions<StudioQueryContext> options) : base(options)
    { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite(@"Filename=..\Data\Studios.db");
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Studio>().ToTable("Studios");
    }
}