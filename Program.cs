using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using minimal_api.domain.services;
using minimal_api.DTOs;
using minimal_api.infraestructure.DBContext;
using minimal_api.infraestructure.Interfaces;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<DBContext>(options =>
{
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"), ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection")));
});

builder.Services.AddScoped<IAdmin, AdminService>();
builder.Services.AddScoped<DBContext>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapPost("Login", ([FromBody] LoginDto loginDto, IAdmin adminInterface) =>
{
    if (adminInterface.Login(loginDto) != null)
    {
        return Results.Ok("Login successful");
    }
    return Results.Unauthorized();
});

app.Run();