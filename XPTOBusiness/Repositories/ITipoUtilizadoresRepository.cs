using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XPTOBusiness.Models;

namespace XPTOBusiness.Repositories
{
    public interface ITipoUtilizadoresRepositories
    {
        List<TipoUtilizador> GetAll(string tag);
        TipoUtilizador GetById(int id, string tag);
        int Insert(TipoUtilizador tu, string tag);
        void Update(TipoUtilizador tu, string tag);
        void Delete(int id, string tag);
    }
}
