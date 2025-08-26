using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using minimal_api.domain.DTO;
using minimal_api.domain.entities;
using minimal_api.domain.Enums;
using minimal_api.domain.Interfaces;
using minimal_api.domain.ModelViews;
using minimal_api.domain.services;
using minimal_api.DTOs;
using minimal_api.infraestructure.Data;
using minimal_api.infraestructure.Interfaces;
using minimal_api.infraestructure.services;

#region 

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<DBContext>(options =>
{
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"), ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection")));
});

builder.Services.AddScoped<IAdmin, AdminService>();
builder.Services.AddScoped<IVeiculo, VeiculoService>();
builder.Services.AddScoped<DBContext>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#endregion

#region  app

var app = builder.Build();

#region HOME
app.MapGet("/", () => Results.Json(new Home()));
#endregion

#region ADMIN
app.MapPost("/admin/Login", ([FromBody] LoginDto loginDto, IAdmin adminInterface) =>
{
    if (adminInterface.Login(loginDto) != null)
    {
        return Results.Ok("Login successful");
    }
    return Results.Unauthorized();
});

app.MapGet("/admin/Lista", ([FromQuery] int? pagina, IAdmin adminInterface) =>
{
    var admins = new List<AdminModelView>();
    var adms = adminInterface.Todos(pagina);
    foreach (var adm in adms)
    {
        admins.Add(new AdminModelView
        {
            Email = adm.email,
            Id = adm.id,
            Role = adm.Role
        });
    }
    return Results.Ok(admins);

});

app.MapGet("/admin/{id}", ([FromRoute] int id, IAdmin admin) =>
{
    var adminService = admin.BuscarAdminPorId(id);
    if (admin == null)
    {
        return Results.NotFound("Veículo não encontrado");
    }

    return Results.Ok(new AdminModelView
        {
            Email = adminService.email,
            Id = adminService.id,
            Role = adminService.Role
        });
});

app.MapPost("/admin", ([FromBody] AdminDTO adminDto, IAdmin adminInterface) =>
{
    var validacao = new ErrosDeValidacao
    {
        Mensagem = new List<string>()
    };
    if (string.IsNullOrEmpty(adminDto.Email) || !adminDto.Email.Contains("@"))
    {
        validacao.Mensagem.Add("Email inválido.");
    }
    if (string.IsNullOrEmpty(adminDto.Senha) || adminDto.Senha.Length < 6)
    {
        validacao.Mensagem.Add("Senha deve ter no mínimo 6 caracteres.");
    }
    if (string.IsNullOrEmpty(adminDto.Role.ToString()) || (adminDto.Role.ToString() != "adm" && adminDto.Role.ToString() != "editor"))
    {
        validacao.Mensagem.Add("Perfil inválido.");
    }
    if (validacao.Mensagem.Count > 0)
    {
        return Results.BadRequest(validacao.Mensagem);
    }
    var admin = new Admin
    {
        email = adminDto.Email,
        Password = adminDto.Senha,
        Role = adminDto.Role.ToString() ?? Role.Editor.ToString()
    };
    adminInterface.incluir(admin);
    
    return Results.Created($"/admin/{admin.id}", new AdminModelView
    {
        Email = admin.email,
        Id = admin.id,
        Role = admin.Role
    });

});
#endregion

#region VEICULOS


ErrosDeValidacao validaVeiculo(VeiculoDto veiculoDto)
{
    var mensagens = new ErrosDeValidacao
    {
        Mensagem = new List<string>()
    };

    if (string.IsNullOrEmpty(veiculoDto.Nome))
    {
        mensagens.Mensagem.Add("Nome e Marca são obrigatórios.");
    }
    if (string.IsNullOrEmpty(veiculoDto.Marca))
    {
        mensagens.Mensagem.Add("Nome e Marca são obrigatórios.");
    }
    if (veiculoDto.Ano < 1886 || veiculoDto.Ano > DateTime.Now.Year + 1)
    {
        mensagens.Mensagem.Add("Ano inválido.");
    }

    return mensagens;
}

app.MapPost("/CadastrarVeiculo", ([FromBody] VeiculoDto veiculoDto, IVeiculo veiculoInterface) =>
{
    var validacao = validaVeiculo(veiculoDto);

    if (validacao.Mensagem.Count > 0)
    {
        return Results.BadRequest(validacao.Mensagem);
    }

    var veiculo = new Veiculo
    {
        Id = veiculoDto.Id,
        Nome = veiculoDto.Nome,
        Marca = veiculoDto.Marca,
        Ano = veiculoDto.Ano
    };
    veiculoInterface.CriarVeiculo(veiculo);

    return Results.Created($"/veiculos/{veiculo.Id}", veiculo);
}).WithTags("Veiculos");

app.MapGet("/BuscarVeiculos", ([FromQuery] int? Pagina, IVeiculo veiculo) =>
{
    var veiculos = veiculo.FiltrarVeiculos(Pagina);

    return Results.Ok(veiculos);
});

app.MapGet("/BuscarVeiculo/{Id}", ([FromQuery] int id, IVeiculo veiculo) =>
{
    var veiculos = veiculo.ExibirVeiculoPorId(id);
    if (veiculos == null)
    {
        return Results.NotFound("Veículo não encontrado");
    }

    return Results.Ok(veiculos);
});

app.MapPut("/AtualizarVeiculo{id}", ([FromQuery] int id, VeiculoDto veiculoDto, IVeiculo veiculo) =>
{

    var veiculos = veiculo.ExibirVeiculoPorId(id);
    if (veiculos == null)
    {
        return Results.NotFound("Veículo não encontrado");
    }

    var validacao = validaVeiculo(veiculoDto);

    if (validacao.Mensagem.Count > 0)
    {
        return Results.BadRequest(validacao.Mensagem);
    }

    veiculos.Nome = veiculoDto.Nome;
    veiculos.Marca = veiculoDto.Marca;
    veiculos.Ano = veiculoDto.Ano;
    veiculo.AtualizarVeiculo(veiculos);

    return Results.Ok(veiculos);
});

app.MapDelete("/BuscarVeiculo{id}", ([FromQuery]int id, IVeiculo veiculo) =>
{
    var veiculos = veiculo.ExibirVeiculoPorId(id);
    if (veiculos == null)
    {
        return Results.NotFound("Veículo não encontrado");
    }
    veiculo.DeletarVeiculo(veiculos);

    return Results.NoContent();
});
#endregion

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Minimal API V1");
    c.RoutePrefix = String.Empty;
});

app.Run();

#endregion