using Microsoft.EntityFrameworkCore;
using UserManagementAPI.Data;
using UserManagementAPI.Models;

var builder = WebApplication.CreateBuilder(args);

// Controllers + JSON
builder.Services.AddControllers();

// EF Core InMemory
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseInMemoryDatabase("UsersDb"));

// Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Swagger solo en Development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

// Seed inicial
await SeedAsync(app);

app.Run();

static async Task SeedAsync(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    if (!await db.Users.AnyAsync())
    {
        db.Users.AddRange(
            new User { FirstName = "Ana", LastName = "Silva", Email = "ana.silva@techhive.local", Department = "RRHH" },
            new User { FirstName = "Luis", LastName = "Ramos", Email = "luis.ramos@techhive.local", Department = "TI" }
        );
        await db.SaveChangesAsync();
    }
}