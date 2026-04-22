using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPTOBusiness.DTOs
{
    public record DisponibilidadeDTO(
        string Titulo,
        string Autor,
        string NomeNucleo,
        string? Assunto,
        int Total,
        int Requisitadas,
        int PresencaObrigatoria,
        int DisponiveisParaRequisicao
    );
}
