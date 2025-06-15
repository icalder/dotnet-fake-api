using Microsoft.EntityFrameworkCore;

namespace FakeApi.Data;


public class WidgetContext : DbContext
{
    public WidgetContext(DbContextOptions<WidgetContext> options)
        : base(options)
    {
    }

    public DbSet<DemoService.Widget> Widgets { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure the primary key for the Widget entity
        modelBuilder.Entity<DemoService.Widget>(entity =>
        {
            entity.HasKey(w => w.Id);
            entity.Property(w => w.Id).ValueGeneratedOnAdd();
        });
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var widgets = new[]
        {
            new DemoService.Widget { Color = "Red", Weight = 33 },
            new DemoService.Widget { Color = "Blue", Weight = 44 }
        };

        optionsBuilder.UseSeeding((context, _) =>
        {
            Console.WriteLine("Seeding initial data...");
            // Seed initial data if needed
            context.Set<DemoService.Widget>().AddRange(widgets);
            context.SaveChanges();
        });

        optionsBuilder.UseAsyncSeeding(async (context, _, cancellationToken) =>
        {
            Console.WriteLine("Seeding initial data asynchronously...");
            // Seed initial data asynchronously if needed
            context.Set<DemoService.Widget>().AddRange(widgets);
            await context.SaveChangesAsync(cancellationToken);
        });
    }
}