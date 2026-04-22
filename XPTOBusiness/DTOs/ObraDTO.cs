using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPTOBusiness.DTOs
{
    public class ObraDTO
    {
        public long? ID_Obra { get; set; }
        public string Titulo { get; set; }
        public string Autor { get; set; }
        public string ISBN { get; set; }
        public byte ID_Assunto { get; set; }
        public byte[] Capa { get; set; }
    }
}