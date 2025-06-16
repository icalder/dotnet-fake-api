using Microsoft.EntityFrameworkCore;

namespace FakeApi.Data;

public class WebsiteContext : DbContext
{
  public WebsiteContext(DbContextOptions<WebsiteContext> options)
      : base(options)
  {
  }

  public DbSet<FakeApiSpec.Models.Website> Websites { get; set; } = null!;

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    // Configure the primary key for the Website entity
    modelBuilder.Entity<FakeApiSpec.Models.Website>(entity =>
    {
      entity.HasKey(w => w.Id);
      entity.Property(w => w.Id).ValueGeneratedOnAdd();
    });
  }

  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    var websites = new[]
    {
            new FakeApiSpec.Models.Website { Url = "https://example.com", Name = "Example", Description = "General" },
            new FakeApiSpec.Models.Website { Url = "https://technews.example.com", Name = "Tech News", Description = "Technology" }
        };

    optionsBuilder.UseSeeding((context, _) =>
    {
      Console.WriteLine("Seeding initial data...");
      // Seed initial data if needed
      context.Set<FakeApiSpec.Models.Website>().AddRange(websites);
      context.SaveChanges();
    });

    optionsBuilder.UseAsyncSeeding(async (context, _, cancellationToken) =>
    {
      Console.WriteLine("Seeding initial data asynchronously...");
      // Seed initial data asynchronously if needed
      context.Set<FakeApiSpec.Models.Website>().AddRange(websites);
      await context.SaveChangesAsync(cancellationToken);
    });
  }
}

