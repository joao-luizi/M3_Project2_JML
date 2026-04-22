using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPTOBusiness.Models
{
    public class Obras
    {
        public long ID_Obra { get; set; }
        public string Autor { get; set; }
        public string ISBN { get; set; }
        public string Titulo { get; set; }
        public byte[] Capa { get; set; } // !!!! MUDAR NO SSMS PARA VARBINARY(MAX) !!!!
        public byte ID_Assunto { get; set; }
    }
}