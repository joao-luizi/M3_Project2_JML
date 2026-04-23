using DalPro;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using XPTOBusiness.DTOs;

namespace XPTOBusiness.Repositories
{
    public interface ILeitorRepository
    {
        public List<SituacaoLeitorDTO> GetSituacaoAtiva(int userId, string tag);

        public List<HistoricoLeitorDTO> GetHistorico(int userId, int? nucleoId, DateTime? inicio, DateTime? fim, string tag);
    }
    public class LeitorRepository : ILeitorRepository
    {
        private readonly IConfiguration _configuration;
        public LeitorRepository(IConfiguration config)
        {
            _configuration = config;
        }

        private string GetConnectionsString(string tag)
        {
            var connectionString = _configuration.GetConnectionString(tag) ?? throw new Exception($"Connection string for tag: {tag} not found!");
            return connectionString;
        }
        public List<SituacaoLeitorDTO> GetSituacaoAtiva(int userId, string tag)
        {
            DALPro.ConnectionString = GetConnectionsString(tag);
            return DALPro.Query<SituacaoLeitorDTO>("EXEC sp_Leitor_Situacao @ID_Utilizador", new() { { "@ID_Utilizador", userId } });
        }

        public List<HistoricoLeitorDTO> GetHistorico(int userId, int? nucleoId, DateTime? inicio, DateTime? fim, string tag)
        {
            DALPro.ConnectionString = GetConnectionsString(tag);
            // Exemplo de Query direta se preferir não criar SP para filtros dinâmicos complexos
            string sql = "SELECT O.Titulo, TN.Descricao as Nucleo, R.DataRequisicao, R.DataEntrega FROM Requisicoes R " +
                         "JOIN Exemplares E ON R.ID_Exemplar = E.ID_Exemplar " +
                         "JOIN Obras O ON E.ID_Obra = O.ID_Obra " +
                         "JOIN TipoNucleos TN ON E.ID_TipoNucleo = TN.ID_TipoNucleo " +
                         "WHERE R.ID_Utilizador = @uid";

            return DALPro.Query<HistoricoLeitorDTO>(sql, new() { { "@uid", userId } });
        }
    }
}