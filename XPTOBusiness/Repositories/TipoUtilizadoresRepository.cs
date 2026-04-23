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
    public class TipoUtilizadoresRepositories : ITipoUtilizadoresRepositories
    {
        private readonly IConfiguration _configuration;
        public TipoUtilizadoresRepositories(IConfiguration config)
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
            DALPro.ConnectionString = GetConnectionsString(tag);
            string sql = "SELECT * FROM Products";

            return DALPro.Query<TipoUtilizador>(sql);
        }

        public TipoUtilizador GetById(int id, string tag)
        {
            DALPro.ConnectionString = GetConnectionsString(tag);
            string sql = "SELECT * FROM  WHERE =@id";

            var param = new Dictionary<string, object>
        {
            {"@id", id}
        };

            return DALPro.Query<TipoUtilizador>(sql, param).FirstOrDefault();
        }

        public int Insert(TipoUtilizador tu, string tag)
        {
            DALPro.ConnectionString = GetConnectionsString(tag);
            SqlTransaction? trans = null;
            try
            {
                trans = DALPro.BeginTransaction();

                string sql = @"INSERT INTO

                           SELECT SCOPE_IDENTITY();";

                var param = new Dictionary<string, object>
                {
                    //{"@UserName", tu.UserName},
                    //{"@PassWord", tu.PassWord},
                    //{"@Nome", tu.Nome},
                    //{"@Email", tu.Email},
                    //{"@ID_TipoUtilizador", tu.ID_TipoUtilizador},
                    //{"@Ativo", tu.Ativo}
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
            DALPro.ConnectionString = GetConnectionsString(tag);
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

        public void Delete(int id, string tag)
        {
            DALPro.ConnectionString = GetConnectionsString(tag);
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
