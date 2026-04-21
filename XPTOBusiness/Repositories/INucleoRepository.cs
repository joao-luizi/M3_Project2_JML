using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XPTOBusiness.Models;

namespace XPTOBusiness.Repositories
{
    public interface INucleoRepository
    {
        IEnumerable<Nucleo> GetAll();
        Nucleo GetById(int id);
        void Add(Nucleo nucleo);
        void Update(Nucleo nucleo);
        void Delete(int id);

        void TransferirExemplares(string listaIds, long idDestino);
        DataTable GetRequisicoesPorPeriodo(DateTime inicio, DateTime fim);
        DataTable GetDisponibilidadePorNucleo();
        DataTable GetDisponibilidadePorNucleoeAssunto();
    }
}
