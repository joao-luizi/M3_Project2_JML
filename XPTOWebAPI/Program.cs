using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using System.Runtime.Intrinsics.X86;
using System.Text;
using XPTOBusiness.DTOs;
using XPTOBusiness.Models;
using XPTOBusiness.Repositories;
using XPTOBusiness.Services.XPTOBusiness.Services;
using XPTOWebAPI.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;



namespace XPTOWebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllers();
            // Add services to the container.
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("cors",
                    policy =>
                    {
                        policy
                            .AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader();
                    });
            });
            builder.Services.AddEndpointsApiExplorer();
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
            builder.Services.AddScoped<IUtilizadorService, UtilizadorService>();
            builder.Services.AddScoped<IRequisicaoService, RequisicaoService>();
            builder.Services.AddScoped<IExemplaresRepository, ExemplaresRepository>();
            builder.Services.AddScoped<IInfracoesRepository, InfracoesRepository>();
            builder.Services.AddScoped<IRequisicoesRepository, RequisicoesRepository>();
            builder.Services.AddScoped<ITipoUtilizadoresRepository, TipoUtilizadoresRepository>();
            builder.Services.AddScoped<IUtilizadoresRepository, UtilizadoresRepository>();
            builder.Services.AddScoped<INucleoRepository, NucleoRepository>();
            builder.Services.AddScoped<ITipoNucleoRepository, TipoNucleoRepository>();
            builder.Services.AddScoped<IExemplaresNucleosRepository, ExemplaresNucleosRepository>();

            builder.Services.AddScoped<NucleoService>();
            builder.Services.AddScoped<UtilizadorService>();

// Add services to the container.
            builder.Services.AddAuthorization();


            var app = builder.Build();

            app.UseCors("cors");
            // Configure the HTTP request pipeline.
            app.UseHttpsRedirection();
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

            app.UseAuthentication();
            app.UseAuthorization();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }



            //5, 6, 10, 11, 12, 14, 15
            //5.Cada leitor pode ter requisitados, no máximo, quatro exemplares
            //14.Os leitores deverão ter a possibilidade de proceder a requisições e
            //devoluções, dentro das normas da biblioteca
            app.MapPost("/requisitar", (RequisicaoDTO dto, IRequisicaoService service) =>
            {
                try
                {
                    service.RequestExemplar(dto.IdUtilizador, dto.IdExemplar, "XPTOConn");

                    return Results.Ok(new { message = "Requisição efetuada com sucesso." });
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
            });

            //6.O tempo máximo para devolução, de cada exemplar, é de quinze dias
            //10.Deve ser possivel suspender o acesso de requisições a leitores que 
            //tenham procedido a devoluções atrasadas em mais que três ocasiões
            //14.Os leitores deverão ter a possibilidade de proceder a requisições e
            //devoluções, dentro das normas da biblioteca
            app.MapPost("/devolver", (RequisicaoDTO dto, IRequisicaoService service) =>
            {
                try
                {
                    service.ReturnExemplar(dto.IdExemplar, "XPTOConn");

                    return Results.Ok(new
                    {
                        message = "Devolução efetuada com sucesso."
                    });
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
            });

            //11.Deve ser possivel reativar o acesso a um leitor suspenso
            app.MapPost("/reativar", (long id, IUtilizadorService service) =>
            {
                try
                {
                    service.ReactivateUser(id, "XPTOConn");

                    return Results.Ok(new
                    {
                        message = "Utilizador Reativado."
                    });
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
            });

            //LOGIN
            app.MapPost("/api/auth/login", (string user, string pass, UtilizadorService service, IConfiguration config) => {
                var u = service.Autenticar(user, pass, "XPTOConn");
                if (u == null) return Results.Unauthorized();

                var token = service.GerarToken(u, config);
                return Results.Ok(new
                {
                    Token = token,
                    Utilizador = u.Nome,
                    Perfil = u.ID_TipoUtilizador == 1 ? "Admin" : "Leitor"
                });
            });
            //15.Deve ser permitido ao leitor cancelar a respetiva inscrição, devendo
            //assumir - se que, nesse caso, é feita a devolução de todos os exemplares
            //que possa ter requisitado e não tenha ainda devolvido
            app.MapPost("/cancel", (long id, IUtilizadorService service) => 
            {
                try
                {
                    service.CancelUser(id, "XPTOConn");

                    return Results.Ok(new
                    {
                        message = "Inscrição Cancelada."
                    });
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
            });

            //12.Deve ser possivel eliminar leitores que estejam há mais de um ano sem
            //fazer qualquer requisição, desde que não tenham nenhuma requisição
            //ativa nesse momento
            app.MapPost("/deleteinactive", (IUtilizadorService service) =>
            {
                try
                {
                    int deleted = service.DeleteInactiveUsers("XPTOConn");
                    string msg;
                    if (deleted > 0)
                        msg = $"{deleted} leitor(es) inativo(s) apagado(s).";
                    else
                        msg = $"Nenhum leitor apagado.";
                    return Results.Ok(new
                    {
                        message = msg
                    }); 
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
            });

            //NUCLEOS
            var nucleosGroup = app.MapGroup("/api/nucleos");

            nucleosGroup.MapGet("/", (NucleoService service, string tag) =>
                Results.Ok(service.ObterTodos()))
                .WithName("GetNucleos")
                .Produces<List<NucleoDTO>>(StatusCodes.Status200OK);

            nucleosGroup.MapGet("/relatorio-requisicoes", (DateTime inicio, DateTime fim, NucleoService service, string tag) =>
                Results.Ok(service.ObterResumoRequisicoes(inicio, fim, tag)))
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

            nucleosGroup.MapPost("/transferencia", (TransferenciaExemplaresDTO dados, NucleoService service) =>
            {
                service.TransferirExemplares(dados);
                return Results.Ok(new { message = "Transferência concluída com sucesso." });
            })
            .RequireAuthorization(policy => policy.RequireRole("Admin"))
            .WithName("TransferirExemplares");

            //ponto 13
            nucleosGroup.MapGet("/dashboard", (NucleoService service) =>
            {
                var dados = service.ObterDadosDecisao();
                return Results.Ok(dados);
            })
            .RequireAuthorization(policy => policy.RequireRole("Admin"))
            .WithName("GetAnaliseDecisao");

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
