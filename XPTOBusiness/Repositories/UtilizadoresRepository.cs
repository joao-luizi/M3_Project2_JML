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
            string sql = "SELECT * FROM Products";

            return DALPro.Query<Utilizador>(sql);
        }

        public Utilizador GetById(long id, string tag)
        {
            DalPro.DALPro.ConnectionString = GetConnectionsString(tag);
            string sql = "SELECT * FROM  WHERE =@id";

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

                string sql = @"INSERT INTO

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

        public void Update(Utilizador u, string tag)
        {
            DalPro.DALPro.ConnectionString = GetConnectionsString(tag);
            SqlTransaction? trans = null;
            try
            {
                trans = DALPro.BeginTransaction();
                string sql = @"UPDATE Products
                       SET ProductName=@ProductName,
                           UnitPrice=@UnitPrice
                       WHERE ProductID=@ProductID";

                var param = new Dictionary<string, object>
                {
                    //{"@ProductID", p.ProductID},
                    //{"@ProductName", p.ProductName},
                    //{"@UnitPrice", p.UnitPrice}
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

                string sql = "DELETE FROM Products WHERE ProductID=@id";

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
