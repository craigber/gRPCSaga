using StudioDomain.Common.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace StudioDomain.Command.Service.Data;

public class StudioCommandContext : DbContext
{
    public DbSet<Studio> Studios { get; set; }
    public StudioCommandContext(DbContextOptions<StudioCommandContext> options) : base(options)
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