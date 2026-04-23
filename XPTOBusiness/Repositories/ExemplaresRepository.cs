using DalPro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XPTOBusiness.Models;

namespace XPTOBusiness.Repositories
{
    public class ExemplarRepository : IExemplaresRepository
    {
        public void TransferirExemplar(long idExemplar, int idNovoNucleo)
        {
            var p = new Dictionary<string, object>
            {
                { "@ListaIDsExemplares", idExemplar.ToString() },
                { "@ID_NucleoDestino", idNovoNucleo }
            };
            DALPro.ExecuteSP("Nucleos_TransferirExemplares", p);
        }

        public void AdicionarExemplar(long idObra, int idNucleoInicial)
        {
            var p = new Dictionary<string, object>
            {
                { "@ID_Obra", idObra },
                { "@ID_Nucleo", idNucleoInicial },
                { "@Quantidade", 1 }
            };
            DALPro.ExecuteSP("Obras_AdicionarExemplares", p);
        }
    }
}
