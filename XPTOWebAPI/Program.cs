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
using XPTOBusiness.Services;
using XPTOBusiness.Services.XPTOBusiness.Services;
using XPTOWebAPI.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Markdig;



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
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "jwtToken",
                    Version = "v1"
                });

                options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Description = "Enter: Bearer {your JWT token}",
                    Name = "Authorization",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                    Scheme = "bearer"
                });

                options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
                {
                    {
                        new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                        {
                            Reference = new Microsoft.OpenApi.Models.OpenApiReference
                            {
                                Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });
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
            builder.Services.AddScoped<IBibliotecaService, BibliotecaService>();
            builder.Services.AddScoped<IRequisicaoService, RequisicaoService>();
            builder.Services.AddScoped<INucleoService, NucleoService>();
            builder.Services.AddScoped<IUtilizadorService, UtilizadorService>();

            builder.Services.AddScoped<IAssuntosRepository, AssuntosRepository>();
            builder.Services.AddScoped<IExemplaresNucleosRepository, ExemplaresNucleosRepository>();
            builder.Services.AddScoped<IExemplaresRepository, ExemplaresRepository>();
            builder.Services.AddScoped<IInfracoesRepository, InfracoesRepository>();
            builder.Services.AddScoped<ILeitorRepository, LeitorRepository>();
            builder.Services.AddScoped<INucleoRepository, NucleoRepository>();
            builder.Services.AddScoped<IObrasRepository, ObrasRepository>();
            builder.Services.AddScoped<IRequisicoesRepository, RequisicoesRepository>();
            builder.Services.AddScoped<ITipoNucleoRepository, TipoNucleoRepository>();
            builder.Services.AddScoped<ITipoUtilizadoresRepository, TipoUtilizadoresRepository>();
            builder.Services.AddScoped<IUtilizadoresRepository, UtilizadoresRepository>();
            


// Add services to the container.
            builder.Services.AddAuthorization();


            var app = builder.Build();

            app.UseCors("cors");
            // Configure the HTTP request pipeline.
            app.UseHttpsRedirection();
            

            app.UseAuthentication();
            app.UseAuthorization();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }


            app.MapGet("/", async () =>
            {
                var path = Path.Combine(AppContext.BaseDirectory, "README.md");

                if (!File.Exists(path))
                    return Results.Redirect("/swagger");

                var markdown = await File.ReadAllTextAsync(path);

                if (string.IsNullOrWhiteSpace(markdown))
                    return Results.Redirect("/swagger");

                var html = Markdown.ToHtml(markdown);
                return Results.Content(html, "text/html");
            });

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
            app.MapPost("/api/auth/login", (string user, string pass, IUtilizadorService service, IConfiguration config) => {
                var u = service.Autenticar(user, pass, "XPTOConn");
                if (u == null) return Results.Unauthorized();

                var token = service.GerarToken(u, config, "XPTOConn");
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

            nucleosGroup.MapGet("/", (INucleoService service) =>
                Results.Ok(service.ObterTodos("XPTOConn")))
                .WithName("GetNucleos")
                .Produces<List<NucleoDTO>>(StatusCodes.Status200OK);

            nucleosGroup.MapPost("/", (SaveNucleoDTO dto, INucleoService service) =>
            {
                service.CriarNucleo(dto, "XPTOConn");
                return Results.Created($"/api/nucleos", dto);
            })
            .RequireAuthorization(policy => policy.RequireRole("Admin"))
            .WithName("CreateNucleo");

            nucleosGroup.MapGet("/relatorio-requisicoes", (DateTime inicio, DateTime fim, INucleoService service) =>
                Results.Ok(service.ObterResumoRequisicoes(inicio, fim, "XPTOConn")))
                .WithName("GetRelatorioRequisicoes");

            nucleosGroup.MapGet("/disponibilidade", (bool porAssunto, INucleoService service) =>
                Results.Ok(service.ObterDisponibilidade(porAssunto, "XPTOConn")))
                .WithName("GetDisponibilidade");


            nucleosGroup.MapPost("/transferencia", (TransferenciaExemplaresDTO dados, INucleoService service) =>
            {
                service.TransferirExemplares(dados, "XPTOConn");
                return Results.Ok(new { message = "Transferência concluída com sucesso." });
            })
            .RequireAuthorization(policy => policy.RequireRole("Admin"))
            .WithName("TransferirExemplares");

            //ponto 13
            nucleosGroup.MapGet("/dashboard", (INucleoService service) =>
            {
                var dados = service.ObterDadosDecisao("XPTOConn");
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
                    var resultados = servico.PesquisarObras(termo, "XPTOConn");
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
                    servico.GuardarObra(obra, "XPTOConn");
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
                    servico.GuardarObra(obra, "XPTOConn");
                    return Results.Ok("Obra atualizada com sucesso.");
                }
                catch (Exception ex) { return Results.Problem(ex.Message); }
            });

            // Eliminar Obra
            app.MapDelete("/api/obras/{id}", (long id, IBibliotecaService servico) =>
            {
                try
                {
                    servico.EliminarObra(id, "XPTOConn");
                    return Results.Ok("Obra eliminada com sucesso.");
                }
                catch (Exception ex) { return Results.Problem(ex.Message); }
            });



            // ==============================================================================
            // 2. EXEMPLARES (Requisitos 1.d e 3)
            // ==============================================================================

            // Transferir Exemplares
            app.MapPost("/api/exemplares/transferir", (ExemplarDTO dto, INucleoService service) =>
            {
                try
                {
                    var t = new TransferenciaExemplaresDTO(
                    
                        new List<long> { dto.ID_Exemplar },
                        dto.ID_Nucleo
                    );
                    service.TransferirExemplares(t, "XPTOConn");
                    return Results.Ok(new { message = "Transferência concluída com sucesso." });
                }
                catch (Exception ex) { return Results.Problem(ex.Message); }
            }).RequireAuthorization(policy => policy.RequireRole("Admin"))
            .WithName("TransferirExemplares"); ;

            // Adicionar número de exemplares
            app.MapPost("/api/exemplares/adicionar", (long idObra, int idNucleo, IBibliotecaService servico) =>
            {
                try
                {
                    servico.AdicionarExemplar(idObra, idNucleo, "XPTOConn");
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
                    var situacao = servico.ObterSituacaoLeitor(id, "XPTOConn");
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
                    var historico = servico.ObterHistoricoLeitor(id, nucleoId, inicio, fim, "XPTOConn");
                    return Results.Ok(historico);
                }
                catch (Exception ex) { return Results.Problem(ex.Message); }
            });

            app.Run();
        }
    }
}
