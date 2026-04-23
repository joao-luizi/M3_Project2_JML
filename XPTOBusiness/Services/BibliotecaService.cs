using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XPTOBusiness.DTOs;
using XPTOBusiness.Repositories;

namespace XPTOBusiness.Services
{
    public interface IBibliotecaService
    {
        public List<SituacaoLeitorDTO> ObterSituacaoLeitor(int userId, string tag);


    }
    public class BibliotecaService : IBibliotecaService
    {
        private readonly ILeitorRepository _leitorRepo;
        private readonly IObrasRepository _obrasRepo;
        private readonly IExemplaresRepository _exemplaresRepo;
        public BibliotecaService(
            ILeitorRepository leitorRepo,
            IObrasRepository obrasRepo,
            IExemplaresRepository exemplaresRepo)
        {
            _leitorRepo = leitorRepo;
            _obrasRepo = obrasRepo;
            _exemplaresRepo = exemplaresRepo;
        }

        // ==========================================
        // LÓGICA DO LEITOR
        // ==========================================
        public List<SituacaoLeitorDTO> ObterSituacaoLeitor(int userId)
        {
            return _leitorRepo.GetSituacaoAtiva(userId);
        }
        public List<HistoricoLeitorDTO> ObterHistoricoLeitor(int userId, int? nucleoId, DateTime? inicio, DateTime? fim)
        {
            return _leitorRepo.GetHistorico(userId, nucleoId, inicio, fim);
        }

        // ==========================================
        // LÓGICA DAS OBRAS
        // ==========================================
        public List<ObraDTO> PesquisarObras(string termo)
        {
            return _obrasRepo.Search(termo);
        }

        public void GuardarObra(ObraDTO obra)
        {
            if (string.IsNullOrWhiteSpace(obra.Titulo))
                throw new ArgumentException("O título da obra é obrigatório.");

            _obrasRepo.CreateUpdate(obra);
        }

        public void EliminarObra(long id)
        {
            _obrasRepo.Delete(id);
        }

        // ==========================================
        // LÓGICA DOS EXEMPLARES
        // ==========================================
        public void TransferirExemplar(long idExemplar, int idNovoNucleo)
        {
            _exemplaresRepo.TransferirExemplar(idExemplar, idNovoNucleo);
        }

        public void AdicionarExemplar(long idObra, int idNucleo)
        {
            _exemplaresRepo.AdicionarExemplar(idObra, idNucleo);
        }
    }
}