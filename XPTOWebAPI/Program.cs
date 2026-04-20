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

// Add services to the container.
            builder.Services.AddAuthorization();


var app = builder.Build();

// Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseAuthorization();


            //5.Cada leitor pode ter requisitados, no máximo, quatro exemplares
            //6.O tempo máximo para devolução, de cada exemplar, é de quinze dias
            //10.Deve ser possivel suspender o acesso de requisições a leitores que
            //tenham procedido a devoluções atrasadas em mais que três ocasiões
            //11.Deve ser possivel reativar o acesso a um leitor suspenso
            //12.Deve ser possivel eliminar leitores que estejam há mais de um ano sem
            //fazer qualquer requisição, desde que não tenham nenhuma requisição
            //ativa nesse momento
            //14.Os leitores deverão ter a possibilidade de proceder a requisições e
            //devoluções, dentro das normas da biblioteca
            //15.Deve ser permitido ao leitor cancelar a respetiva inscrição, devendo
            //assumir - se que, nesse caso, é feita a devolução de todos os exemplares
            //que possa ter requisitado e não tenha ainda devolvido

            app.MapPost("/requisitar", (RequisicaoDto dto, IRequisicaoService _requisicaoService) =>
            {
                _requisicaoService.RequisitarExemplar(dto.IdUtilizador, dto.IdExemplar, "XPTOConn");
                
            });

            

            app.Run();
        }
    }
}
