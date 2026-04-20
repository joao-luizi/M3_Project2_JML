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
   
    public interface IRequisicoesRepository
    {
        public List<Requisicao> GetAll(string tag);
        public Requisicao GetById(long id, string tag);
        public long Insert(Requisicao r, string tag);
        public void Update(Requisicao r, string tag);
        public void Delete(long id, string tag);

        public int CountActiveByUser(long userId, string tag);
    }

    public class RequisicoesRepository : IRequisicoesRepository
    {
        private readonly IConfiguration _configuration;
        public RequisicoesRepository(IConfiguration config)
        {
            _configuration = config;
        }

        private string GetConnectionsString(string tag)
        {
            var connectionString = _configuration.GetConnectionString(tag) ?? throw new Exception($"Connection string for tag: {tag} not found!");
            return connectionString;
        }
        public List<Requisicao> GetAll(string tag)
        {
            DalPro.DALPro.ConnectionString = GetConnectionsString(tag);
            string sql = "SELECT * FROM [dbo].[Requisicoes]";

            return DALPro.Query<Requisicao>(sql);
        }

        public Requisicao GetById(long id, string tag)
        {
            DalPro.DALPro.ConnectionString = GetConnectionsString(tag);
            string sql = "SELECT * FROM [dbo].[Requisicoes] WHERE ID_Requisicao = @id";

            var param = new Dictionary<string, object>
    {
        { "@id", id }
    };

            return DALPro.Query<Requisicao>(sql, param).FirstOrDefault();
        }



        public long Insert(Requisicao r, string tag)
        {
            DalPro.DALPro.ConnectionString = GetConnectionsString(tag);
            SqlTransaction? trans = null;

            try
            {
                trans = DALPro.BeginTransaction();

                string sql = @"
            INSERT INTO [dbo].[Requisicoes]
                (ID_Utilizador, ID_Exemplar, DataRequisicao, DataEntrega)
            VALUES
                (@ID_Utilizador, @ID_Exemplar, @DataRequisicao, @DataEntrega);

            SELECT SCOPE_IDENTITY();";

                var param = new Dictionary<string, object>
        {
            { "@ID_Utilizador", r.IdUtilizador },
            { "@ID_Exemplar", r.IdExemplar },
            { "@DataRequisicao", r.DataRequisicao }, // ou DateTime.Now se preferires forçar aqui
            { "@DataEntrega", (object?)r.DataEntrega ?? DBNull.Value }
        };

                long ret = Convert.ToInt64(DALPro.ExecuteScalar(sql, param, trans));

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

        public void Update(Requisicao r, string tag)
        {
            DalPro.DALPro.ConnectionString = GetConnectionsString(tag);
            SqlTransaction? trans = null;

            try
            {
                trans = DALPro.BeginTransaction();

                string sql = @"
            UPDATE [dbo].[Requisicoes]
            SET
                ID_Utilizador = @ID_Utilizador,
                ID_Exemplar = @ID_Exemplar,
                DataRequisicao = @DataRequisicao,
                DataEntrega = @DataEntrega
            WHERE ID_Requisicao = @ID_Requisicao";

                var param = new Dictionary<string, object>
        {
            { "@ID_Requisicao", r.IdRequisicao },
            { "@ID_Utilizador", r.IdUtilizador },
            { "@ID_Exemplar", r.IdExemplar },
            { "@DataRequisicao", r.DataRequisicao },
            { "@DataEntrega", (object?)r.DataEntrega ?? DBNull.Value }
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

        public void Delete(long id, string tag)
        {
            DalPro.DALPro.ConnectionString = GetConnectionsString(tag);
            SqlTransaction? trans = null;

            try
            {
                trans = DALPro.BeginTransaction();

                string sql = "DELETE FROM [dbo].[Requisicoes] WHERE ID_Requisicao = @id";

                var param = new Dictionary<string, object>
        {
            { "@id", id }
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

        public int CountActiveByUser(long userId, string tag)
        {
            DALPro.ConnectionString = GetConnectionsString(tag);

            string sql = @"
            SELECT COUNT(*) 
            FROM Requisicoes
            WHERE ID_Utilizador = @userId
              AND DataEntrega IS NULL";

            var param = new Dictionary<string, object>
    {
        { "@userId", userId }
    };

            object result = DALPro.ExecuteScalar(sql, param);

            return result != null ? Convert.ToInt32(result) : 0;
        }
    }
}
