using Microsoft.EntityFrameworkCore;
using minimal_api.DTOs;
using minimal_api.infraestructure.DBContext;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

builder.Services.AddDbContext<DBContext>(options =>
{
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"), ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection")));
});

app.MapGet("/", () => "Hello World!");

app.MapPost("Login", (LoginDto loginDto) =>
{
    if (loginDto.Username == "admin" && loginDto.Password == "12345")
    {
        return Results.Ok("Login successful");
    }
    return Results.Unauthorized();
});

app.Run();