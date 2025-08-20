using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using minimal_api.domain.ModelViews;
using minimal_api.domain.services;
using minimal_api.DTOs;
using minimal_api.infraestructure.Data;
using minimal_api.infraestructure.Interfaces;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<DBContext>(options =>
{
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"), ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection")));
});

builder.Services.AddScoped<IAdmin, AdminService>();
builder.Services.AddScoped<DBContext>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

#region HOME
app.MapGet("/", () => Results.Json(new Home()));
#endregion

#region ADMIN
app.MapPost("Login", ([FromBody] LoginDto loginDto, IAdmin adminInterface) =>
{
    if (adminInterface.Login(loginDto) != null)
    {
        return Results.Ok("Login successful");
    }
    return Results.Unauthorized();
});
#endregion

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Minimal API V1");
    c.RoutePrefix = String.Empty;
});

app.Run();