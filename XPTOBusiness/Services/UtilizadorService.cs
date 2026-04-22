using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XPTOBusiness.Models;
using XPTOBusiness.Repositories;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;

namespace XPTOBusiness.Services
{
    //ponto 1*
    public class UtilizadorService
    {
        private readonly IUtilizadoresRepository _userRepo;
        private const string ConnectionTag = "DefaultConnection";

        public UtilizadorService(IUtilizadoresRepository userRepo)
        {
            _userRepo = userRepo;
        }

        public Utilizador? Autenticar(string username, string password)
        {
            var user = _userRepo.GetAll(ConnectionTag)
                .FirstOrDefault(u => u.UserName == username && u.PassWord == password && u.Ativo);
            return user;
        }

        public void RegistarUtilizador(Utilizador novoUser)
        {
            novoUser.Ativo = true;
            _userRepo.Insert(novoUser, ConnectionTag);
        }

        public string GerarToken(Utilizador user, IConfiguration config)
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
