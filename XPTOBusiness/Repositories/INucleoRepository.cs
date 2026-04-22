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
        public IEnumerable<Nucleo> GetAll();
        public Nucleo GetById(int id);
        public void Add(Nucleo nucleo);
        public void Update(Nucleo nucleo);
        public void Delete(int id);

        public void TransferirExemplares(string listaIds, long idDestino);
        public DataTable GetRequisicoesPorPeriodo(DateTime inicio, DateTime fim);
        public DataTable GetDisponibilidadePorNucleo();
        public DataTable GetDisponibilidadePorNucleoeAssunto();
    }
}
