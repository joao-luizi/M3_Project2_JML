using XPTOBusiness.Repositories;
using XPTOBusiness.Services;
using XPTOBusiness.DTOs;
using DalPro;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using XPTOBusiness.Services.XPTOBusiness.Services;

var builder = WebApplication.CreateBuilder(args);

DALPro.ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string não encontrada.");

builder.Services.AddScoped<INucleoRepository, NucleoRepository>();
builder.Services.AddScoped<ITipoNucleoRepository, TipoNucleoRepository>();
builder.Services.AddScoped<IExemplaresNucleosRepository, ExemplaresNucleosRepository>();
builder.Services.AddScoped<NucleoService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


var nucleosGroup = app.MapGroup("/api/nucleos");

nucleosGroup.MapGet("/", (NucleoService service) =>
    Results.Ok(service.ObterTodos()))
    .WithName("GetNucleos");

nucleosGroup.MapPost("/", (SaveNucleoDTO dto, NucleoService service) =>
{
    service.CriarNucleo(dto);
    return Results.Created($"/api/nucleos", dto);
})
.WithName("CreateNucleo");

nucleosGroup.MapPost("/transferencia", (TransferenciaExemplaresDTO dados, NucleoService service) =>
{
    service.TransferirExemplares(dados);
    return Results.Ok(new { message = "Transferência concluída com sucesso." });
})
.WithName("TransferirExemplares");

nucleosGroup.MapGet("/relatorio-requisicoes", (DateTime inicio, DateTime fim, NucleoService service) =>
    Results.Ok(service.ObterResumoRequisicoes(inicio, fim)))
    .WithName("GetRelatorioRequisicoes");

nucleosGroup.MapGet("/disponibilidade", (bool porAssunto, NucleoService service) =>
    Results.Ok(service.ObterDisponibilidade(porAssunto)))
    .WithName("GetDisponibilidade");

app.Run();