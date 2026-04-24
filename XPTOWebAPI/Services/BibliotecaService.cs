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
        public List<HistoricoLeitorDTO> ObterHistoricoLeitor(int userId, int? nucleoId, DateTime? inicio, DateTime? fim, string tag);
        public List<ObraDTO> PesquisarObras(string termo, string tag);
        public void GuardarObra(ObraDTO obra, string tag);
        public void EliminarObra(long id, string tag);

        public void TransferirExemplar(long idExemplar, long idNovoNucleo, string tag);

        public void AdicionarExemplar(long idObra, int idNucleo, string tag);

    }
    public class BibliotecaService : IBibliotecaService
    {
        private readonly ILeitorRepository _leitorRepo;
        private readonly IObrasRepository _obrasRepo;
        private readonly IExemplaresRepository _exemplaresRepo;
        private readonly ILogger _logger;
        public BibliotecaService(
            ILogger<BibliotecaService> logger,
            ILeitorRepository leitorRepo,
            IObrasRepository obrasRepo,
            IExemplaresRepository exemplaresRepo)
        {
            _logger = logger;
            _leitorRepo = leitorRepo;
            _obrasRepo = obrasRepo;
            _exemplaresRepo = exemplaresRepo;
        }

        // ==========================================
        // LÓGICA DO LEITOR
        // ==========================================
        public List<SituacaoLeitorDTO> ObterSituacaoLeitor(int userId, string tag)
        {
            return _leitorRepo.GetSituacaoAtiva(userId, tag);
        }
        public List<HistoricoLeitorDTO> ObterHistoricoLeitor(int userId, int? nucleoId, DateTime? inicio, DateTime? fim, string tag)
        {
            return _leitorRepo.GetHistorico(userId, nucleoId, inicio, fim, tag);
        }

        // ==========================================
        // LÓGICA DAS OBRAS
        // ==========================================
        public List<ObraDTO> PesquisarObras(string termo, string tag)
        {
            return _obrasRepo.Search(termo, tag);
        }

        public void GuardarObra(ObraDTO obra, string tag)
        {
            if (string.IsNullOrWhiteSpace(obra.Titulo))
                throw new ArgumentException("O título da obra é obrigatório.");

            _obrasRepo.CreateUpdate(obra, tag);
        }

        public void EliminarObra(long id, string tag)
        {
            _obrasRepo.Delete(id, tag);
        }

        // ==========================================
        // LÓGICA DOS EXEMPLARES
        // ==========================================
        public void TransferirExemplar(long idExemplar, long idNovoNucleo, string tag)
        {
            _exemplaresRepo.TransferirExemplar(idExemplar, idNovoNucleo, tag);
        }

        public void AdicionarExemplar(long idObra, int idNucleo, string tag)
        {
            _exemplaresRepo.AdicionarExemplar(idObra, idNucleo, tag);
        }
    }
}
