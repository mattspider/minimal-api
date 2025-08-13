using minimal_api.DTOs;


var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

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