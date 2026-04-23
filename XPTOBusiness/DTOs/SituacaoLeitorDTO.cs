using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPTOBusiness.DTOs
{
    public class SituacaoLeitorDTO
    {
        public string Titulo { get; set; }
        public string Nucleo { get; set; }
        public DateTime DataLimite { get; set; }
        public string Status { get; set; }
    }
}