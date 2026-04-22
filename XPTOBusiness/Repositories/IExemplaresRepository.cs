using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XPTOBusiness.Models;

namespace XPTOBusiness.Repositories
{
    public interface IExemplaresRepository
    {
        public void TransferirExemplar(long idExemplar, int idNovoNucleo);
        public void AdicionarExemplar(long idObra, int idNucleoInicial);
    }
}
