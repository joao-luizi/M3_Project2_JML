using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPTOBusiness.Models
{
    public class Obra
    {
        public long ID_Obra { get; set; }
        public string? Autor { get; set; }
        public string? Titulo { get; set; } = string.Empty;
        public byte[]? Capa { get; set; } = null; // mudar para VARBINARY(MAX) no SSMS
        public string? ISBN { get; set; }
        public byte ID_Assunto { get; set; }

    }
}