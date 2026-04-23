using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XPTOBusiness.DTOs;
using XPTOBusiness.Models;

namespace XPTOBusiness.Repositories
{
    public interface IAssuntosRepository
    {
        IEnumerable<AssuntoDTO> GetAll();
        AssuntoDTO GetById(byte id);
        void Add(AssuntoDTO assunto);
        void Update(AssuntoDTO assunto);
        void Delete(byte id);
    }
}
