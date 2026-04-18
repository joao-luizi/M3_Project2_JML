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
        Assuntos GetById(byte id);
        void Add(Assuntos assunto);
        void Update(Assuntos assunto);
        void Delete(byte id);
    }
}
