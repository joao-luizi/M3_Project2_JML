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
        public List<HistoricoLeitorDTO> ObterHistoricoLeitor(int userId, int? nucleoId, DateTime? inicio, DateTime? fim);
        public List<ObraDTO> PesquisarObras(string termo);
        public void GuardarObra(ObraDTO obra);
        public void EliminarObra(long id);
        public void TransferirExemplar(long idExemplar, int idNovoNucleo);
        public void AdicionarExemplar(long idObra, int idNucleo);
    }
}