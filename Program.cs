using Microsoft.EntityFrameworkCore;
using FakeApi.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<WidgetContext>(opt =>
    opt.UseInMemoryDatabase("TodoList"));
builder.Services.AddScoped<DemoService.IWidgets, Services.Widgets>();

var app = builder.Build();

// Ensure the database is created and seeded
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<WidgetContext>();
    context.Database.EnsureCreated();   
}

// Configure the HTTP request pipeline.


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
