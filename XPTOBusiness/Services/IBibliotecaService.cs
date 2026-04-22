using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XPTOBusiness.DTOs;

namespace XPTOBusiness.Services
{
    public interface IBibliotecaService
    {
        public List<SituacaoLeitorDTO> ObterSituacaoLeitor(int userId);


    }
}
