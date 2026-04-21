using Microsoft.AspNetCore.Mvc;
using Microsoft.SqlServer.Server;
using System.Runtime.ConstrainedExecution;
using System.Runtime.Intrinsics.X86;
using XPTOBusiness.DTOs;
using XPTOBusiness.Models;
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
            builder.Services.AddSwaggerGen(options => options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
            {
                Title = "jwtToken",
                Version = "v1"
            }));

            builder.Services.AddAuthorization();


var app = builder.Build();

            app.UseCors("cors");
            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseAuthorization();
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //5.Cada leitor pode ter requisitados, no máximo, quatro exemplares
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
            app.MapPost("/deleteinactive", (long id, IUtilizadorService service) =>
            {
                try
                {
                    service.DeleteInactive("XPTOConn");

                    return Results.Ok(new
                    {
                        message = "Leitores inativos apagados."
                    });
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
            });
            app.Run();
        }
    }
}
