using XPTOBusiness.Repositories;
using XPTOBusiness.Services;
using XPTOBusiness.DTOs;
using DalPro;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using XPTOBusiness.Services.XPTOBusiness.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

DALPro.ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string não encontrada.");

builder.Services.AddScoped<INucleoRepository, NucleoRepository>();
builder.Services.AddScoped<ITipoNucleoRepository, TipoNucleoRepository>();
builder.Services.AddScoped<IExemplaresNucleosRepository, ExemplaresNucleosRepository>();
builder.Services.AddScoped<IUtilizadoresRepository, UtilizadoresRepository>();
builder.Services.AddScoped<ITipoUtilizadoresRepositories, TipoUtilizadoresRepositories>();

builder.Services.AddScoped<NucleoService>();
builder.Services.AddScoped<UtilizadorService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//LOGIN
app.MapPost("/api/auth/login", (string user, string pass, UtilizadorService service, IConfiguration config) => {
    var u = service.Autenticar(user, pass);
    if (u == null) return Results.Unauthorized();

    var token = service.GerarToken(u, config);
    return Results.Ok(new
    {
        Token = token,
        Utilizador = u.Nome,
        Perfil = u.ID_TipoUtilizador == 1 ? "Admin" : "Leitor"
    });
});

//NUCLEOS
var nucleosGroup = app.MapGroup("/api/nucleos");

nucleosGroup.MapGet("/", (NucleoService service) =>
    Results.Ok(service.ObterTodos()))
    .WithName("GetNucleos")
    .Produces<List<NucleoDTO>>(StatusCodes.Status200OK);

nucleosGroup.MapGet("/relatorio-requisicoes", (DateTime inicio, DateTime fim, NucleoService service) =>
    Results.Ok(service.ObterResumoRequisicoes(inicio, fim)))
    .WithName("GetRelatorioRequisicoes");

nucleosGroup.MapGet("/disponibilidade", (bool porAssunto, NucleoService service) =>
    Results.Ok(service.ObterDisponibilidade(porAssunto)))
    .WithName("GetDisponibilidade");

nucleosGroup.MapPost("/", (SaveNucleoDTO dto, NucleoService service) =>
{
    service.CriarNucleo(dto);
    return Results.Created($"/api/nucleos", dto);
})
.RequireAuthorization(policy => policy.RequireRole("Admin"))
.WithName("CreateNucleo");

//nucleosGroup.MapPost("/transferencia", (TransferenciaExemplaresDTO dados, NucleoService service) =>
//{
//    service.TransferirExemplares(dados);
//    return Results.Ok(new { message = "Transferência concluída com sucesso." });
//})
//.RequireAuthorization(policy => policy.RequireRole("Admin"))
//.WithName("TransferirExemplares");

//ponto 13
nucleosGroup.MapGet("/dashboard", (NucleoService service) =>
{
    var dados = service.ObterDadosDecisao();
    return Results.Ok(dados);
})
.RequireAuthorization(policy => policy.RequireRole("Admin"))
.WithName("GetAnaliseDecisao");

app.Run();