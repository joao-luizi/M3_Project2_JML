using XPTOBusiness.Models;
using XPTOBusiness.Repositories;

namespace XPTOWebAPI.Services
{
    public interface IUtilizadorService
    {
        public bool ReactivateUser(long id, string tag);
        public bool CancelUser(long id, string tag);
    }
    public class UtilizadorService : IUtilizadorService
    {
        private readonly IUtilizadoresRepository _utilizadoresRepo;
        private readonly IRequisicoesRepository _requisicoesRepo;
        private readonly IExemplaresRepository _exemplaresRepo;
        private readonly IInfracoesRepository _infracoesRepo;

        private readonly ILogger _logger;
        public UtilizadorService(ILogger<UtilizadorService> logger, IUtilizadoresRepository utilizadoresRepo, IRequisicoesRepository requisicoesRepo,
            IExemplaresRepository exemplaresRepo, IInfracoesRepository infracoesRepo)
        {
            _utilizadoresRepo = utilizadoresRepo;
            _requisicoesRepo = requisicoesRepo;
            _exemplaresRepo = exemplaresRepo;
            _infracoesRepo = infracoesRepo;
            _logger = logger;
        }

        public bool CancelUser(long id, string tag)
        {
            var utilizador = _utilizadoresRepo.GetById(id, tag);
            if (utilizador == null)
                throw new Exception("Utilizador não existe.");

            var requisicoes = _requisicoesRepo.GetActiveByUserId(id, tag);

            if (!_requisicoesRepo.CloseAllByUserId(id, tag))
                  throw new Exception("Erro ao fechar requisições de utilizador.");
            utilizador.Ativo = false;

            _utilizadoresRepo.Update(utilizador, tag);

            var infracoes = _infracoesRepo.GetByUserId(id, tag);

            if (infracoes == null)
                return true;

            infracoes.InfracoesTotal += infracoes.InfracoesAtuais;
            infracoes.InfracoesAtuais = 0;
            _infracoesRepo.Update(infracoes, tag);
            return true;
            
        }
        public bool ReactivateUser(long id, string tag)
        {
            var utilizador = _utilizadoresRepo.GetById(id, tag);

            if (utilizador == null)
                throw new Exception("Utilizador não existe.");
            if (utilizador.Ativo)
                throw new Exception("Utilizador já está ativo.");
            utilizador.Ativo = true;

            var infracoes = _infracoesRepo.GetByUserId(id, tag);

            if (infracoes == null)
            {
                //infracoes = new Infracao()
                //{
                //    ID_Utilizador = id,
                //    InfracoesAtuais = 0,
                //    InfracoesTotal = 0
                //};
                //_infracoesRepo.Insert(infracoes, tag);
                // a logica parece ser que só criamos registo de infração quando existe uma infração. Assim nunca havera
                //infraçoes (registos) com infracoes totais ou atuais a 0. Mas se criarmos também não faz diferença
                return true;
            }

            infracoes.InfracoesTotal += infracoes.InfracoesAtuais;
            infracoes.InfracoesAtuais = 0;
            _infracoesRepo.Update(infracoes, tag);

            return true;
        }
        
    }
}
