using DalPro;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XPTOBusiness.DTOs;
using XPTOBusiness.Models;

namespace XPTOBusiness.Repositories
{
    public interface IInfracoesRepository
    {
        public List<Infracao> GetAll(string tag);
        public Infracao? GetByUserId(long id, string tag);
        public long Insert(Infracao e, string tag);
        public void Update(Infracao e, string tag);
        public void Delete(long id, string tag);

       
    }

    public class InfracoesRepository : IInfracoesRepository
    {
        private readonly IConfiguration _configuration;
        public InfracoesRepository(IConfiguration config)
        {
            _configuration = config;
        }

        private string GetConnectionsString(string tag)
        {
            var connectionString = _configuration.GetConnectionString(tag) ?? throw new Exception($"Connection string for tag: {tag} not found!");
            return connectionString;
        }
        public List<Infracao> GetAll(string tag)
        {
            DalPro.DALPro.ConnectionString = GetConnectionsString(tag);

            string sql = @"
        SELECT 
            ID_Utilizador AS IdUtilizador,
            InfracoesTotal,
            InfracoesAtuais
        FROM [dbo].[Infracoes]";

            return DALPro.Query<Infracao>(sql);
        }

        public Infracao? GetByUserId(long id, string tag)
        {
            DalPro.DALPro.ConnectionString = GetConnectionsString(tag);

            string sql = @"
        SELECT 
            ID_Utilizador AS IdUtilizador,
            InfracoesTotal,
            InfracoesAtuais
        FROM [dbo].[Infracoes]
        WHERE ID_Utilizador = @id";

            var param = new Dictionary<string, object>
    {
        { "@id", id }
    };

            return DALPro.Query<Infracao>(sql, param).FirstOrDefault();
        }

        public long Insert(Infracao e, string tag)
        {
            DalPro.DALPro.ConnectionString = GetConnectionsString(tag);
            SqlTransaction? trans = null;

            try
            {
                trans = DALPro.BeginTransaction();

                string sql = @"
            INSERT INTO [dbo].[Infracoes]
                (ID_Utilizador, InfracoesTotal, InfracoesAtuais)
            VALUES
                (@IdUtilizador, @InfracoesTotal, @InfracoesAtuais);

            SELECT @IdUtilizador;";

                var param = new Dictionary<string, object>
        {
            { "@IdUtilizador", e.IdUtilizador },
            { "@InfracoesTotal", e.InfracoesTotal },
            { "@InfracoesAtuais", e.InfracoesAtuais }
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

        public void Update(Infracao e, string tag)
        {
            DalPro.DALPro.ConnectionString = GetConnectionsString(tag);
            SqlTransaction? trans = null;

            try
            {
                trans = DALPro.BeginTransaction();

                string sql = @"
            UPDATE [dbo].[Infracoes]
            SET 
                InfracoesTotal = @InfracoesTotal,
                InfracoesAtuais = @InfracoesAtuais
            WHERE ID_Utilizador = @IdUtilizador";

                var param = new Dictionary<string, object>
        {
            { "@IdUtilizador", e.IdUtilizador },
            { "@InfracoesTotal", e.InfracoesTotal },
            { "@InfracoesAtuais", e.InfracoesAtuais }
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

                string sql = @"
            DELETE FROM [dbo].[Infracoes]
            WHERE ID_Utilizador = @id";

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
    }
}
