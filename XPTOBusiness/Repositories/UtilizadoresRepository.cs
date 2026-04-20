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
    public interface IUtilizadoresRepository
    {
        public List<Utilizador> GetAll(string tag);
        public Utilizador GetById(long id, string tag);
        public long Insert(Utilizador u, string tag);
        public void Update(Utilizador u, string tag);
        public void Delete(long id, string tag);
    }

    public  class UtilizadoresRepository : IUtilizadoresRepository
    {
        private readonly IConfiguration _configuration;
        public UtilizadoresRepository(IConfiguration config)
        {
            _configuration = config;
        }

        private string GetConnectionsString(string tag)
        {
            var connectionString = _configuration.GetConnectionString(tag) ?? throw new Exception($"Connection string for tag: {tag} not found!");
            return connectionString;
        }
        public List<Utilizador> GetAll(string tag)
        {
            DalPro.DALPro.ConnectionString = GetConnectionsString(tag);
            string sql = "SELECT * FROM [dbo].[Utilizadores]";

            return DALPro.Query<Utilizador>(sql);
        }

        public Utilizador GetById(long id, string tag)
        {
            DalPro.DALPro.ConnectionString = GetConnectionsString(tag);
            string sql = "SELECT * FROM [dbo].[Utilizadores] WHERE =@id";

            var param = new Dictionary<string, object>
        {
            {"@id", id}
        };

            return DALPro.Query<Utilizador>(sql, param).FirstOrDefault();
        }

        public long Insert(Utilizador u, string tag)
        {
            DalPro.DALPro.ConnectionString = GetConnectionsString(tag);
            SqlTransaction? trans = null;
            try
            {
                trans = DALPro.BeginTransaction();

                string sql = @"INSERT INTO [dbo].[Utilizadores]
                       (UserName, PassWord, Nome, Email, ID_TipoUtilizador, Ativo)
                       VALUES
                       (@UserName, @PassWord, @Nome, @Email, @ID_TipoUtilizador, @Ativo);
                       
                       SELECT SCOPE_IDENTITY();";

                var param = new Dictionary<string, object>
                {
                    {"@UserName", u.UserName},
                    {"@PassWord", u.PassWord},
                    {"@Nome", u.Nome},
                    {"@Email", u.Email},
                    {"@ID_TipoUtilizador", u.ID_TipoUtilizador},
                    {"@Ativo", u.Ativo}
                };
                long ret = Convert.ToInt32(DALPro.ExecuteScalar(sql, param, trans));
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

        public void Update(Utilizador u, string tag)
        {
            DalPro.DALPro.ConnectionString = GetConnectionsString(tag);
            SqlTransaction? trans = null;
            try
            {
                trans = DALPro.BeginTransaction();
                string sql = @"UPDATE [dbo].[Utilizadores]
                       SET
                           UserName = @UserName,
                           PassWord = @PassWord,
                           Nome = @Nome,
                           Email = @Email,
                           ID_TipoUtilizador = @ID_TipoUtilizador,
                           Ativo = @Ativo
                       WHERE ID_Utilizador = @ID_Utilizador";

                var param = new Dictionary<string, object>
                {
                    {"@ID_Utilizador", u.ID_Utilizador},
                    {"@UserName", u.UserName},
                    {"@PassWord", u.PassWord},
                    {"@Nome", u.Nome},
                    {"@Email", u.Email},
                    {"@ID_TipoUtilizador", u.ID_TipoUtilizador},
                    {"@Ativo", u.Ativo}
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

        public void Delete(long id, string tag)
        {
            DalPro.DALPro.ConnectionString = GetConnectionsString(tag);
            SqlTransaction? trans = null;
            try
            {
                trans = DALPro.BeginTransaction();
                string sql = "DELETE FROM [dbo].[Utilizadores] WHERE ID_Utilizador = @id";

                var param = new Dictionary<string, object>
                {
                    {"@id", id}
                };

                DALPro.Execute(sql, param, trans);
                DALPro.Commit(trans);
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
