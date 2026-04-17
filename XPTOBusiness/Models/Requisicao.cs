using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPTOBusiness.Models
{
    public class Requisicao
    {
        public long IdRequisicao { get; set; }

        public long IdUtilizador { get; set; }

        public long IdExemplar { get; set; }

        public DateTime DataRequisicao { get; set; }

        public DateTime? DataEntrega { get; set; }
    }
}
