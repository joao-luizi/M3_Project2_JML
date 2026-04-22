using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XPTOBusiness.Models;

namespace XPTOBusiness.Repositories
{
    public interface ITipoNucleoRepository
    {
        IEnumerable<TipoNucleo> GetAll();
        TipoNucleo? GetById(byte id);
        void Add(TipoNucleo tipo);
        void Update(TipoNucleo tipo);
        void Delete(byte id);
    }
}
