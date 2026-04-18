using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XPTOBusiness.Models;

namespace XPTOBusiness.Repositories
{
    public interface IObrasRepository
    {
        IEnumerable<Obra> GetAll();
        Obra GetById(long id);
        void Add(Obra obra);
        void Update(Obra obra);
        void Delete(long id);
    }
}
