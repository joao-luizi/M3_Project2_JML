using DalPro;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XPTOBusiness.Models;

namespace XPTOBusiness.Repositories
{
    public interface ITipoNucleoRepository
    {
        IEnumerable<TipoNucleo> GetAll(string tag);
        TipoNucleo? GetById(byte id, string tag);
        void Add(TipoNucleo tipo, string tag);
        void Update(TipoNucleo tipo, string tag);
        void Delete(byte id, string tag);
    }
    public class TipoNucleoRepository : ITipoNucleoRepository
    {
        private readonly IConfiguration _configuration;
        public TipoNucleoRepository(IConfiguration config)
        {
            _configuration = config;
        }

        private string GetConnectionsString(string tag)
        {
            var connectionString = _configuration.GetConnectionString(tag) ?? throw new Exception($"Connection string for tag: {tag} not found!");
            return connectionString;
        }
        public IEnumerable<TipoNucleo> GetAll(string tag)
        {
            DALPro.ConnectionString = GetConnectionsString(tag);
            string sql = "SELECT * FROM TipoNucleos";
            var lista = DALPro.Query<TipoNucleo>(sql);
            return lista;
        }

        public TipoNucleo? GetById(byte id, string tag)
        {
            DALPro.ConnectionString = GetConnectionsString(tag);
            string sql = "SELECT * FROM Tipo_Nucleos WHERE ID_TipoNucleo = @id";
            var p = new Dictionary<string, object> { { "@id", id } };
            var lista = DALPro.Query<TipoNucleo>(sql, parameters: p);
            return lista.FirstOrDefault();
        }

        public void Add(TipoNucleo tipo, string tag)
        {
            DALPro.ConnectionString = GetConnectionsString(tag);
            string sql = "INSERT INTO TipoNucleos (Descricao) VALUES (@desc)";
            var p = new Dictionary<string, object> { { "@desc", tipo.Descricao } };
            DALPro.Execute(sql, parameters: p);
        }

        public void Update(TipoNucleo tipo, string tag)
        {
            DALPro.ConnectionString = GetConnectionsString(tag);
            string sql = "UPDATE TipoNucleos SET Descricao = @desc WHERE ID_TipoNucleo = @id";
            var p = new Dictionary<string, object> { { "@desc", tipo.Descricao }, { "@id", tipo.ID_TipoNucleo } };
            DALPro.Execute(sql, parameters: p);
        }

        public void Delete(byte id, string tag)
        {
            DALPro.ConnectionString = GetConnectionsString(tag);
            string sql = "DELETE FROM TipoNucleos WHERE ID_TipoNucleo = @id";
            var p = new Dictionary<string, object> { { "@id", id } };
            DALPro.Execute(sql, parameters: p);
        }
    }
}
