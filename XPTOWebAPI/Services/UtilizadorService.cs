using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using XPTOBusiness.Models;
using XPTOBusiness.Repositories;

namespace XPTOWebAPI.Services
{
    public interface IUtilizadorService
    {
        public bool ReactivateUser(long id, string tag);
        public bool CancelUser(long id, string tag);

        public int DeleteInactiveUsers(string tag);

        public Utilizador? Autenticar(string username, string password, string tag);

        public string GerarToken(Utilizador user, IConfiguration config, string tag);
    }
    public class UtilizadorService : IUtilizadorService
    {
        private readonly IUtilizadoresRepository _utilizadoresRepo;
        private readonly IRequisicoesRepository _requisicoesRepo;
        private readonly IInfracoesRepository _infracoesRepo;

        private readonly ILogger _logger;
        public UtilizadorService(ILogger<UtilizadorService> logger, IUtilizadoresRepository utilizadoresRepo, IRequisicoesRepository requisicoesRepo, IInfracoesRepository infracoesRepo)
        {
            _utilizadoresRepo = utilizadoresRepo;
            _requisicoesRepo = requisicoesRepo;
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

            _utilizadoresRepo.Update(utilizador, tag);
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

        public int DeleteInactiveUsers(string tag)
        {
            var users = _utilizadoresRepo.GetInactiveUsersEligibleForDeletion(tag);

            int count = 0;

            foreach (var user in users)
            {
                _utilizadoresRepo.DeleteUserById(user.ID_Utilizador, tag);
                count++;
            }

            return count;
        }

        public Utilizador? Autenticar(string username, string password, string tag)
        {
            var user = _utilizadoresRepo.GetAll(tag)
                .FirstOrDefault(u => u.UserName == username && u.PassWord == password && u.Ativo);
            return user;
        }

        public void RegistarUtilizador(Utilizador novoUser, string tag)
        {
            novoUser.Ativo = true;
            _utilizadoresRepo.Insert(novoUser, tag);
        }

        public string GerarToken(Utilizador user, IConfiguration config, string tag)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
        new Claim(ClaimTypes.NameIdentifier, user.ID_Utilizador.ToString()),
        new Claim(ClaimTypes.Name, user.UserName ?? ""),
        new Claim(ClaimTypes.Email, user.Email ?? ""),
        new Claim(ClaimTypes.Role, user.ID_TipoUtilizador == 1 ? "Admin" : "User")
    };

            var token = new JwtSecurityToken(
                issuer: config["Jwt:Issuer"],
                audience: config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(3),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
