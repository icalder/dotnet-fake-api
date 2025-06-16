using Microsoft.EntityFrameworkCore;
using FakeApi.Data;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); // Handle enum serialization
    });

builder.Services.AddDbContext<WidgetContext>(opt =>
    opt.UseInMemoryDatabase("TodoList"));
builder.Services.AddDbContext<WebsiteContext>(opt =>
    opt.UseInMemoryDatabase("Websites"));

var app = builder.Build();

// Ensure the databases are created and seeded
using (var scope = app.Services.CreateScope())
{
    var dbContextTypes = builder.Services.Where(s => s.ServiceType.IsAssignableTo(typeof(DbContext)))
        .Select(s => s.ServiceType);

    foreach (var dbContextType in dbContextTypes)
    {
        var dbContext = (DbContext)scope.ServiceProvider.GetRequiredService(dbContextType);
        dbContext.Database.EnsureCreated();
    }
}

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
