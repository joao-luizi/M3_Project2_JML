using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XPTOBusiness.Models;

namespace XPTOBusiness.Repositories
{
    public interface IExemplaresNucleosRepository
    {
        IEnumerable<ExemplarNucleo> GetAll();
        ExemplarNucleo GetByExemplarId(long idExemplar);
        void Add(ExemplarNucleo exemplarNucleo);
        void Update(ExemplarNucleo exemplarNucleo);
        void Delete(long idExemplar);
    }
}
