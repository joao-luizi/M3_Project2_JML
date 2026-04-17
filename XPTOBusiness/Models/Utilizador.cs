using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPTOBusiness.Models
{
    public class Utilizador
    {
        public long ID_Utilizador { get; set; }

        public string? UserName { get; set; }

        public string? PassWord { get; set; }

        public string? Nome { get; set; }

        public string? Email { get; set; }

        public int ID_TipoUtilizador { get; set; }

        public bool Ativo { get; set; }
    }
}
