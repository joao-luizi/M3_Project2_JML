using Microsoft.AspNetCore.Mvc;
using XPTOBusiness.Repositories;

namespace XPTOWebAPI.Services
{
    public interface IRequisicaoService
    {
        public bool RequisitarExemplar(long userId, long exemplarId, string tag);
    }
    public class RequisicaoService : IRequisicaoService
    {
        private readonly IUtilizadoresRepository _utilizadoresRepo;
        private readonly IRequisicoesRepository _requisicoesRepo;
        private readonly IExemplaresRepository _exemplaresRepo;

        private readonly ILogger _logger;
        public RequisicaoService(ILogger<RequisicaoService> logger, IUtilizadoresRepository utilizadoresRepo, IRequisicoesRepository requisicoesRepo, 
            IExemplaresRepository exemplaresRepo)
        {
            _utilizadoresRepo = utilizadoresRepo;
            _requisicoesRepo = requisicoesRepo;
            _exemplaresRepo = exemplaresRepo;
            _logger = logger;
        }
        public bool RequisitarExemplar(long userId, long exemplarId, string tag)
        {
       
            var user = _utilizadoresRepo.GetById(userId, tag);

            if (user == null)
                throw new Exception("Utilizador não encontrado.");

            if (user.ID_TipoUtilizador != 1)
                throw new Exception("Apenas leitores podem requisitar.");

            var activeCount = _requisicoesRepo.CountActiveByUser(userId, tag);

            if (activeCount >= 4)
                throw new Exception("Limite de 4 exemplares atingido.");

            var exemplar = _exemplaresRepo.GetWithObraAndNucleo(exemplarId, tag);

            if (exemplar == null)
                throw new Exception("Exemplar inválido.");

            if (_requisicaoRepository.IsAlreadyRequested(exemplarId))
                throw new Exception("Exemplar já requisitado.");

            var availableInNucleo = _exemplaresRepo.CountByObraAndNucleo(
                exemplar.IdObra,
                exemplar.IdNucleo, 
                tag
            );

            if (availableInNucleo <= 1)
                throw new Exception("Exemplar reservado para consulta presencial.");

            _requisicaoRepository.Insert(userId, exemplarId);

            return true;
        }
    }
}
