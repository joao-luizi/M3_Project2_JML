using DalPro;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XPTOBusiness.Models;

namespace XPTOBusiness.Repositories
{
    public interface ITipoUtilizadoresRepository
    {
        public List<TipoUtilizador> GetAll(string tag);
        public TipoUtilizador GetById(int id, string tag);
        public int Insert(TipoUtilizador tu, string tag);
        public void Update(TipoUtilizador tu, string tag);
        public void Delete(int id, string tag);

    }
    public  class TipoUtilizadoresRepository : ITipoUtilizadoresRepository
    {
        private readonly IConfiguration _configuration;
        public TipoUtilizadoresRepository(IConfiguration config)
        {
            _configuration = config;
        }

        private string GetConnectionsString(string tag)
        {
            var connectionString = _configuration.GetConnectionString(tag) ?? throw new Exception($"Connection string for tag: {tag} not found!");
            return connectionString;
        }
        public List<TipoUtilizador> GetAll(string tag)
        {
            DalPro.DALPro.ConnectionString = GetConnectionsString(tag);
            string sql = "SELECT * FROM [dbo].[TipoUtilizadores]";

            return DALPro.Query<TipoUtilizador>(sql);
        }

        public TipoUtilizador GetById(int id, string tag)
        {
            DalPro.DALPro.ConnectionString = GetConnectionsString(tag);
            string sql = "SELECT * FROM [dbo].[TipoUtilizadores] WHERE =@id";

            var param = new Dictionary<string, object>
        {
            {"@id", id}
        };

            return DALPro.Query<TipoUtilizador>(sql, param).FirstOrDefault();
        }

        public int Insert(TipoUtilizador tu, string tag)
        {
            DalPro.DALPro.ConnectionString = GetConnectionsString(tag);
            SqlTransaction? trans = null;
            try
            {
                trans = DALPro.BeginTransaction();

                string sql = @"INSERT INTO TipoUtilizadores (Perfil)
                       VALUES (@Perfil);
                       SELECT SCOPE_IDENTITY();";

                var param = new Dictionary<string, object>
                {
                     {"@Perfil", tu.Perfil}
                };
                int ret = Convert.ToInt32(DALPro.ExecuteScalar(sql, param, trans));
                DALPro.Commit(trans);
                return ret;
            }
            catch
            {
                if (trans != null)
                    DALPro.Rollback(trans);
                throw;
            }

        }

        public void Update(TipoUtilizador tu, string tag)
        {
            DalPro.DALPro.ConnectionString = GetConnectionsString(tag);
            SqlTransaction? trans = null;
            try
            {
                trans = DALPro.BeginTransaction();
                string sql = @"UPDATE TipoUtilizadores
                       SET Perfil = @Perfil
                       WHERE ID_TipoUtilizador = @ID_TipoUtilizador;"; ;

                var param = new Dictionary<string, object>
                {
                    {"@Perfil", tu.Perfil},
                    {"@ID_TipoUtilizador", tu.ID_TipoUtilizador}
                };

                DALPro.Execute(sql, param, trans);
            }
            catch
            {
                if (trans != null)
                    DALPro.Rollback(trans);
                throw;
            }
        }

        public void Delete(int id, string tag)
        {
            DalPro.DALPro.ConnectionString = GetConnectionsString(tag);
            SqlTransaction? trans = null;
            try
            {

                string sql = @"DELETE FROM TipoUtilizadores
                       WHERE ID_TipoUtilizador = @id;";

                var param = new Dictionary<string, object>
                {
                    {"@id", id}
                };

                DALPro.Execute(sql, param, trans);
            }
            catch
            {
                if (trans != null)
                    DALPro.Rollback(trans);
                throw;
            }
        }
    }
}
