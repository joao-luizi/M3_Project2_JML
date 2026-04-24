using DalPro;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
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
            var parametros = new Dictionary<string, object> { { "@ID_Utilizador", userId } };
            DataTable dt = DALPro.ExecuteSP("Utilizador_ConsultarEstadoRequisicoes", parametros);

            var lista = new List<SituacaoLeitorDTO>();
            foreach (DataRow row in dt.Rows)
            {
                lista.Add(new SituacaoLeitorDTO
                {
                    Titulo = row["Titulo"].ToString()!,
                    Nucleo = row["Nome"].ToString()!,
                    DataLimite = Convert.ToDateTime(row["Data Limite"]),
                    Status = row["Situacao"].ToString()!
                });
            }
            return lista;
        }

        public List<HistoricoLeitorDTO> GetHistorico(int userId, int? nucleoId, DateTime? inicio, DateTime? fim, string tag)
        {
            DALPro.ConnectionString = GetConnectionsString(tag);
            var parametros = new Dictionary<string, object> { { "@ID_Utilizador", userId } };

            if (nucleoId.HasValue) parametros.Add("@ID_Nucleo", nucleoId.Value);
            if (inicio.HasValue) parametros.Add("@IntervaloInicio", inicio.Value);
            if (fim.HasValue) parametros.Add("@IntervaloFim", fim.Value);

            DataTable dt = DALPro.ExecuteSP("Utilizador_ConsultarRequisicoes", parametros);

            var lista = new List<HistoricoLeitorDTO>();
            foreach (DataRow row in dt.Rows)
            {
                lista.Add(new HistoricoLeitorDTO
                {
                    Titulo = row["Titulo"].ToString()!,
                    Nucleo = row["Nome"].ToString()!,
                    DataRequisicao = Convert.ToDateTime(row["DataRequisicao"]),
                    DataEntrega = row["DataEntrega"] != DBNull.Value ? Convert.ToDateTime(row["DataEntrega"]) : null
                });
            }
            return lista;
        }
    }
}