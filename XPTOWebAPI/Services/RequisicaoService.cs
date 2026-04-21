using Microsoft.AspNetCore.Mvc;
using XPTOBusiness.DTOs;
using XPTOBusiness.Models;
using XPTOBusiness.Repositories;

namespace XPTOWebAPI.Services
{
    public interface IRequisicaoService
    {
        public bool RequestExemplar(long userId, long exemplarId, string tag);
        public void ReturnExemplar(long idExemplar, string tag);
    }
    public class RequisicaoService : IRequisicaoService
    {
        private readonly IUtilizadoresRepository _utilizadoresRepo;
        private readonly IRequisicoesRepository _requisicoesRepo;
        private readonly IExemplaresRepository _exemplaresRepo;
        private readonly IInfracoesRepository _infracoesRepo;

        private readonly ILogger _logger;
        public RequisicaoService(ILogger<RequisicaoService> logger, IUtilizadoresRepository utilizadoresRepo, IRequisicoesRepository requisicoesRepo, 
            IExemplaresRepository exemplaresRepo, IInfracoesRepository infracoesRepo)
        {
            _utilizadoresRepo = utilizadoresRepo;
            _requisicoesRepo = requisicoesRepo;
            _exemplaresRepo = exemplaresRepo;
            _infracoesRepo = infracoesRepo;
            _logger = logger;
        }
        public bool RequestExemplar(long userId, long exemplarId, string tag)
        {
       
            var user = _utilizadoresRepo.GetById(userId, tag);

            if (user == null)
                throw new Exception("Utilizador não encontrado.");

            if (user.ID_TipoUtilizador != 1)
                throw new Exception("Apenas leitores podem requisitar.");

            var infracao = _infracoesRepo.GetByUserId(userId, tag);

            if (infracao != null && infracao.InfracoesAtuais > 3)
                throw new Exception("Utilizador suspenso por excesso de devoluções atrasadas.");

            var activeCount = _requisicoesRepo.CountActiveByUser(userId, tag);

            if (activeCount >= 4)
                throw new Exception("Limite de 4 exemplares atingido.");

            var exemplar = _exemplaresRepo.GetWithObraAndNucleo(exemplarId, tag);

            if (exemplar == null)
                throw new Exception("Exemplar inválido.");

            if (_requisicoesRepo.IsAlreadyRequested(exemplarId, tag))
                throw new Exception("Exemplar já requisitado.");

            var availableInNucleo = _exemplaresRepo.CountByObraAndNucleo(
                exemplar.IdObra,
                exemplar.IdNucleo, 
                tag
            );

            if (availableInNucleo <= 1)
                throw new Exception("Exemplar reservado para consulta presencial.");

            _requisicoesRepo.Insert(new Requisicao() { ID_Utilizador = userId, ID_Exemplar = exemplarId }, tag);

            return true;
        }

        public void ReturnExemplar(long idExemplar, string tag)
        {
            var requisicao = _requisicoesRepo.GetActiveByExemplarId(idExemplar, tag);

            if (requisicao == null)
                throw new Exception("Requisição ativa não encontrada.");


            var dataLimite = requisicao.DataRequisicao.AddDays(15);
            requisicao.DataEntrega = DateTime.Now;

            bool isLate = requisicao.DataEntrega > dataLimite;

            
            _requisicoesRepo.Update(requisicao, tag);

            
            if (isLate)
            {
                var infracao = _infracoesRepo.GetByUserId(requisicao.ID_Utilizador, tag);

                if (infracao == null)
                {
                    infracao = new Infracao()
                    {
                        ID_Utilizador = requisicao.ID_Utilizador,
                        InfracoesAtuais = 0,
                        InfracoesTotal = 0
                    };
                    _infracoesRepo.Insert(infracao, tag);
                }
                
                var totalInfractions = infracao.InfracoesAtuais++;

                
                if (totalInfractions > 3)
                {
                   var utilizador = _utilizadoresRepo.GetById(requisicao.ID_Utilizador, tag);
                    if (utilizador == null)
                        throw new Exception("Active user not found.");
                    utilizador.Ativo = false;
                    _utilizadoresRepo.Update(utilizador, tag);
                    infracao.InfracoesTotal += infracao.InfracoesAtuais;
                    infracao.InfracoesAtuais = 0;
                }
                
                
                _infracoesRepo.Update(infracao, tag);
                

            }
        }
    }
}
