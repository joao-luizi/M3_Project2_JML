using Azure;
using DalPro;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XPTOBusiness.Models;

namespace XPTOBusiness.Repositories
{
    public interface IExemplaresNucleosRepository
    {
        IEnumerable<ExemplarNucleo> GetAll(string tag);
        ExemplarNucleo GetByExemplarId(long idExemplar, string tag);
        void Add(ExemplarNucleo exemplarNucleo, string tag);
        void Update(ExemplarNucleo exemplarNucleo, string tag);
        void Delete(long idExemplar, string tag);
    }
    public class ExemplaresNucleosRepository : IExemplaresNucleosRepository
    {
        private readonly IConfiguration _configuration;
        public ExemplaresNucleosRepository(IConfiguration config)
        {
            _configuration = config;
        }
        private string GetConnectionsString(string tag)
        {
            var connectionString = _configuration.GetConnectionString(tag) ?? throw new Exception($"Connection string for tag: {tag} not found!");
            return connectionString;
        }
        public IEnumerable<ExemplarNucleo> GetAll(string tag)
        {
            DalPro.DALPro.ConnectionString = GetConnectionsString(tag);
            string sql = "SELECT * FROM Exemplares_Nucleo";
            var lista = DALPro.Query<ExemplarNucleo>(sql);
            return lista;
        }

        public ExemplarNucleo GetByExemplarId(long idExemplar, string tag)
        {
            DalPro.DALPro.ConnectionString = GetConnectionsString(tag);
            string sql = "SELECT * FROM Exemplares_Nucleo WHERE ID_Exemplar = @id";
            var p = new Dictionary<string, object> { { "@id", idExemplar } };
            var list = DALPro.Query<ExemplarNucleo>(sql, parameters: p);
            if (list == null || list.Count == 0) return null;
            return list[0];
        }

        public void Add(ExemplarNucleo en, string tag)
        {
            DalPro.DALPro.ConnectionString = GetConnectionsString(tag);
            string sql = "INSERT INTO Exemplares_Nucleo (ID_Exemplar, ID_Nucleo) VALUES (@idEx, @idNuc)";
            var p = new Dictionary<string, object> { { "@idEx", en.ID_Exemplar }, { "@idNuc", en.ID_Nucleo } };
            DALPro.Execute(sql, parameters: p);
        }

        public void Update(ExemplarNucleo en, string tag)
        {
            DalPro.DALPro.ConnectionString = GetConnectionsString(tag);
            string sql = "UPDATE Exemplares_Nucleo SET ID_Nucleo = @idNuc WHERE ID_Exemplar = @idEx";
            var p = new Dictionary<string, object> { { "@idNuc", en.ID_Nucleo }, { "@idEx", en.ID_Exemplar } };
            DALPro.Execute(sql, parameters: p);
        }

        public void Delete(long idExemplar, string tag)
        {
            DalPro.DALPro.ConnectionString = GetConnectionsString(tag);
            string sql = "DELETE FROM Exemplares_Nucleo WHERE ID_Exemplar = @id";
            var p = new Dictionary<string, object> { { "@id", idExemplar } };
            DALPro.Execute(sql, parameters: p);
        }
    }
}
