using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XPTOBusiness.Models;

namespace XPTOBusiness.Repositories
{
    public interface IAssuntosRepository
    {
        IEnumerable<Assuntos> GetAll();
        Assuntos GetById(int id);
        void Add(Assuntos assunto);
        void Update(Assuntos assunto);
        void Delete(int id);
    }
}
