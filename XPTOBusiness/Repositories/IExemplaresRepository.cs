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
        IEnumerable<Exemplares> GetAll();
        Exemplares GetById(long id);
        void Add(Exemplares exemplar);
        void Update(Exemplares exemplar);
        void Delete(long id);
    }
}
