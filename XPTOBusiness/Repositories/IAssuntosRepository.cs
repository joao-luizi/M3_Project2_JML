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
        void Add(AssuntoCreateDTO assunto);
        void Update(AssuntoUpdateDTO assunto);
        void Delete(byte id);
    }
}
