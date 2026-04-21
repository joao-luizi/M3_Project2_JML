using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPTOBusiness.Models
{
    public class Requisicao
    {
        public long ID_Requisicao { get; set; }

        public long ID_Utilizador { get; set; }

        public long ID_Exemplar { get; set; }

        public DateTime DataRequisicao { get; set; }

        public DateTime? DataEntrega { get; set; }
    }
}
