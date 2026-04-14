using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XPTOBusiness.Models;

namespace XPTOBusiness.Repositories
{
    public interface INucleoRepository
    {
        void GetAll();
        void GetById(int id);
        void Add(Nucleo nucleo);
        void Update(Nucleo nucleo);
        void Delete(int id);
    }
}
