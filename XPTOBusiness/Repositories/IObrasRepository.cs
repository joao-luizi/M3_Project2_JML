using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XPTOBusiness.DTOs;
using XPTOBusiness.Models;

namespace XPTOBusiness.Repositories
{

    public interface IObrasRepository
    {
        // CRUD e Manutenção (Adicionar/remover/atualizar)
        public void CreateUpdate(ObraDTO obra);

        // Pesquisa
        public List<ObraDTO> Search(string termo);

        // Remoção
        public void Delete(long id);
    }
}
