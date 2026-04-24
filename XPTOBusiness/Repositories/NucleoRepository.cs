using DalPro;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XPTOBusiness.Models;
using Microsoft.Data.SqlClient;
using DalPro;
using System.Data;

namespace XPTOBusiness.Repositories
{
    public interface INucleoRepository
    {
        public IEnumerable<Nucleo> GetAll(string tag);
        public Nucleo GetById(int id, string tag);
        public void Add(Nucleo nucleo, string tag);
        public void Update(Nucleo nucleo, string tag);
        public void Delete(int id, string tag);

        public void TransferirExemplares(string listaIds, long idDestino, string tag);
        public IEnumerable<object> GetRequisicoesPorPeriodo(DateTime inicio, DateTime fim, string tag);
        public DataTable GetDisponibilidadePorNucleo(string tag);
        public DataTable GetDisponibilidadePorNucleoeAssunto(string tag);
    }
    public class NucleoRepository : INucleoRepository
    {
        private readonly IConfiguration _configuration;
        public NucleoRepository(IConfiguration config)
        {
            _configuration = config;
        }

        private string GetConnectionsString(string tag)
        {
            var connectionString = _configuration.GetConnectionString(tag) ?? throw new Exception($"Connection string for tag: {tag} not found!");
            return connectionString;
        }
        public void Add(Nucleo nucleo, string tag)
        {
            DALPro.ConnectionString = GetConnectionsString(tag);
            string sql = "INSERT INTO Nucleos (Nome, Local, ID_TipoNucleo) VALUES (@nome, @local, @tipo)";
            var p = new Dictionary<string, object> {
                { "@nome", nucleo.Nome },
                { "@local", nucleo.Local },
                { "@tipo", nucleo.ID_TipoNucleo }
            };
            DALPro.Execute(sql, parameters: p);
        }

        public void Update(Nucleo nucleo, string tag)
        {
            DALPro.ConnectionString = GetConnectionsString(tag);
            string sql = "UPDATE Nucleos SET Nome=@nome, Local=@local, ID_TipoNucleo=@tipo WHERE ID_Nucleo=@id";
            var p = new Dictionary<string, object> {
                { "@id", nucleo.ID_Nucleo },
                { "@nome", nucleo.Nome },
                { "@local", nucleo.Local },
                { "@tipo", nucleo.ID_TipoNucleo }
            };
            DALPro.Execute(sql, parameters: p);
        }

        public void Delete(int id, string tag)
        {
            DALPro.ConnectionString = GetConnectionsString(tag);
            string sql = "DELETE FROM Nucleos WHERE ID_Nucleo = @id";
            var p = new Dictionary<string, object> { { "@id", id } };
            DALPro.Execute(sql, parameters: p);
        }

        public void TransferirExemplares(string listaIds, long idDestino, string tag)
        {
            DALPro.ConnectionString = GetConnectionsString(tag);
            var p = new Dictionary<string, object> {
                { "@ListaIDsExemplares", listaIds },
                { "@ID_NucleoDestino", idDestino }
            };
            DALPro.ExecuteSP("Nucleos_TransferirExemplares", parameters: p);
        }

        public IEnumerable<object> GetRequisicoesPorPeriodo(DateTime inicio, DateTime fim, string tag)
        {
            DALPro.ConnectionString = GetConnectionsString(tag);

            var p = new Dictionary<string, object> {
                { "@DataInicio", inicio },
                { "@DataFim", fim }
            };

            var dt = DALPro.ExecuteSP("Nucleos_MostrarRequisicoes", parameters: p);

            return dt.AsEnumerable().Select(r => new
            {
                Nome = r["Nome"]?.ToString(),
                Local = r["Local"]?.ToString(),
                Requisicoes = Convert.ToInt32(r["Requisições no Período:"] ?? 0)
            });
        }

        public DataTable GetDisponibilidadePorNucleo(string tag)
        {
            DALPro.ConnectionString = GetConnectionsString(tag);
            return DALPro.ExecuteSP("Disponibilidade_Exemplares_Nucleo");
        }

        public DataTable GetDisponibilidadePorNucleoeAssunto(string tag)
        {
            DALPro.ConnectionString = GetConnectionsString(tag);
            return DALPro.ExecuteSP("Disponibilidade_Exemplares_NucleoAssunto");
        }

        public IEnumerable<Nucleo> GetAll(string tag)
        {
            DALPro.ConnectionString = GetConnectionsString(tag);
            string sql = "SELECT * FROM Nucleos";
            var lista = DALPro.Query<Nucleo>(sql);
            return lista;
        }
        public Nucleo GetById(int id, string tag) { return null; }
    }   
}
