using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPTOBusiness.DTOs
{
    public class HistoricoLeitorDTO
    {
        public string Titulo { get; set; } = null!;
        public string Nucleo { get; set; } = null!;
        public DateTime DataRequisicao { get; set; }
        public DateTime? DataEntrega { get; set; }
    }
}