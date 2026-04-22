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
            { "@ID_Exemplar", idExemplar },
            { "@ID_Nucleo", idNovoNucleo }
        };
            DALPro.ExecuteSP("sp_TransferirExemplar", p);
        }

        // Atualizar número de exemplares
        public void AdicionarExemplar(long idObra, int idNucleoInicial)
        {
            var p = new Dictionary<string, object>
        {
            { "@ID_Obra", idObra },
            { "@ID_Nucleo", idNucleoInicial }
        };
            DALPro.ExecuteSP("sp_AdicionarExemplar", p);
        }
    }
}
