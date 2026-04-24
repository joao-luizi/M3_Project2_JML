using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPTOBusiness.Models
{
    public class Nucleo
    {
        public long ID_Nucleo { get; set; }
        public string? Nome { get; set; }
        public string? Local { get; set; }
        public byte ID_TipoNucleo { get; set; }

    }
}
