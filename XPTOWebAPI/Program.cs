using DalPro;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using XPTOBusiness.DTOs;
using XPTOBusiness.Repositories;
using XPTOBusiness.Services;

namespace XPTOWebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
var builder = WebApplication.CreateBuilder(args);

        DALPro.ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            // Registar Repositórios e Serviços (Dependency Injection)
            builder.Services.AddScoped<IObrasRepository, ObrasRepository>();
            builder.Services.AddScoped<IExemplaresRepository, ExemplarRepository>();
            builder.Services.AddScoped<ILeitorRepository, LeitorRepository>();
            builder.Services.AddScoped<IBibliotecaService, BibliotecaService>();
            builder.Services.AddAuthorization();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new() { Title = "Biblioteca API", Version = "v1" });
            });

            // 👉 ADICIONADO: Configuração do CORS
            // Criámos uma política chamada "AllowAll" que permite pedidos de qualquer frontend.
            // (Em produção, deves restringir o WithOrigins ao domínio do teu frontend)
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });


            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // 👉 ADICIONADO: Ativar o CORS usando a política que criámos acima
            app.UseCors("AllowAll");


            // ==============================================================================
            // 1. OBRAS (Requisitos 1 e 4)
            // ==============================================================================

            // Pesquisar Obras
            app.MapGet("/api/obras/search", (string termo, IBibliotecaService servico) =>
            {
                try
                {
                    var resultados = servico.PesquisarObras(termo);
                    return Results.Ok(resultados);
                }
                catch (Exception ex) { return Results.Problem(ex.Message); }
            });

            // Acrescentar Obra
            app.MapPost("/api/obras", (ObraDTO obra, IBibliotecaService servico) =>
            {
                try
                {
                    obra.ID_Obra = 0; // Forçar INSERT
                    servico.GuardarObra(obra);
                    return Results.Ok("Obra registada com sucesso.");
                }
                catch (Exception ex) { return Results.Problem(ex.Message); }
            });

            // Alterar Obra
            app.MapPut("/api/obras/{id}", (long id, ObraDTO obra, IBibliotecaService servico) =>
            {
                try
                {
                    obra.ID_Obra = id; // Forçar UPDATE com o ID passado no URL
                    servico.GuardarObra(obra);
                    return Results.Ok("Obra atualizada com sucesso.");
                }
                catch (Exception ex) { return Results.Problem(ex.Message); }
            });

            // Eliminar Obra
            app.MapDelete("/api/obras/{id}", (long id, IBibliotecaService servico) =>
            {
                try
                {
                    servico.EliminarObra(id);
                    return Results.Ok("Obra eliminada com sucesso.");
                }
                catch (Exception ex) { return Results.Problem(ex.Message); }
            });


            // ==============================================================================
            // 2. EXEMPLARES (Requisitos 1.d e 3)
            // ==============================================================================

            // Transferir Exemplares
            app.MapPost("/api/exemplares/transferir", (TransferirExemplarDTO dto, IBibliotecaService servico) =>
            {
                try
                {
                    servico.TransferirExemplar(dto.ID_Exemplar, dto.ID_TipoNucleo);
                    return Results.Ok("Transferência concluída com sucesso.");
                }
                catch (Exception ex) { return Results.Problem(ex.Message); }
            });

            // Adicionar número de exemplares
            app.MapPost("/api/exemplares/adicionar", (long idObra, int idNucleo, IBibliotecaService servico) =>
            {
                try
                {
                    servico.AdicionarExemplar(idObra, idNucleo);
                    return Results.Ok("Exemplar adicionado com sucesso à obra.");
                }
                catch (Exception ex) { return Results.Problem(ex.Message); }
            });


            // ==============================================================================
            // 3. LEITORES (Requisito 5)
            // ==============================================================================

            // Situação atual do Leitor (Atrasos, Urgências, etc.)
            app.MapGet("/api/leitores/{id}/situacao", (int id, IBibliotecaService servico) =>
            {
                try
                {
                    var situacao = servico.ObterSituacaoLeitor(id);
                    if (situacao == null || situacao.Count == 0)
                        return Results.NotFound("Nenhuma requisição ativa encontrada para este leitor.");

                    return Results.Ok(situacao);
                }
                catch (Exception ex) { return Results.Problem(ex.Message); }
            });

            // Histórico do Leitor (com filtros opcionais passados por Query String)
            app.MapGet("/api/leitores/{id}/historico", (
                int id,
                [FromQuery] int? nucleoId,
                [FromQuery] DateTime? inicio,
                [FromQuery] DateTime? fim,
                IBibliotecaService servico) =>
            {
                try
                {
                    var historico = servico.ObterHistoricoLeitor(id, nucleoId, inicio, fim);
                    return Results.Ok(historico);
                }
                catch (Exception ex) { return Results.Problem(ex.Message); }
            });

            app.Run();
        }
    }
}
