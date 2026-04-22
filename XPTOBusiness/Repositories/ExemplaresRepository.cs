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


    public interface IExemplaresRepository
    {
        public List<Exemplar> GetAll(string tag);
        public Exemplar GetById(long id, string tag);
        public long Insert(Exemplar e, string tag);
        public void Update(Exemplar e, string tag);
        public void Delete(long id, string tag);

        public ExemplarDTO? GetWithObraAndNucleo(long exemplarId, string tag);

        public int CountByObraAndNucleo(long obraId, long nucleoId, string tag);
    }

    public class ExemplaresRepository : IExemplaresRepository
    {
        private readonly IConfiguration _configuration;
        public ExemplaresRepository(IConfiguration config)
        {
            _configuration = config;
        }

        private string GetConnectionsString(string tag)
        {
            var connectionString = _configuration.GetConnectionString(tag) ?? throw new Exception($"Connection string for tag: {tag} not found!");
            return connectionString;
        }
        public List<Exemplar> GetAll(string tag)
        {
            DalPro.DALPro.ConnectionString = GetConnectionsString(tag);
            string sql = "SELECT * FROM [dbo].[Exemplares]";

            return DALPro.Query<Exemplar>(sql);
        }

        public Exemplar GetById(long id, string tag)
        {
            DalPro.DALPro.ConnectionString = GetConnectionsString(tag);
            string sql = "SELECT * FROM [dbo].[Exemplares] WHERE ID_Exemplar = @id";

            var param = new Dictionary<string, object>
            {
                {"@id", id}
            };

            return DALPro.Query<Exemplar>(sql, param).FirstOrDefault();
        }

        public long Insert(Exemplar e, string tag)
        {
            DalPro.DALPro.ConnectionString = GetConnectionsString(tag);
            SqlTransaction? trans = null;

            try
            {
                trans = DALPro.BeginTransaction();

                string sql = @"
            INSERT INTO [dbo].[Exemplares] (ID_Obra)
            VALUES (@ID_Obra);

            SELECT SCOPE_IDENTITY();";

                var param = new Dictionary<string, object>
        {
            {"@ID_Obra", e.ID_Obra}
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

        public void Update(Exemplar e, string tag)
        {
            DalPro.DALPro.ConnectionString = GetConnectionsString(tag);
            SqlTransaction? trans = null;

            try
            {
                trans = DALPro.BeginTransaction();

                string sql = @"
            UPDATE [dbo].[Exemplares]
            SET ID_Obra = @ID_Obra
            WHERE ID_Exemplar = @ID_Exemplar";

                var param = new Dictionary<string, object>
        {
            {"@ID_Exemplar", e.ID_Exemplar},
            {"@ID_Obra", e.ID_Obra}
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
            DALPro.ConnectionString = GetConnectionsString(tag);
            SqlTransaction? trans = null;

            try
            {
                trans = DALPro.BeginTransaction();

                string sql = "DELETE FROM [dbo].[Exemplares] WHERE ID_Exemplar = @id";

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

        public ExemplarDTO? GetWithObraAndNucleo(long exemplarId, string tag)
        {
            DALPro.ConnectionString = GetConnectionsString(tag);

            string sql = @"
            SELECT 
                E.ID_Exemplar,
                E.ID_Obra,
                EN.ID_Nucleo
            FROM Exemplares E
            JOIN Exemplares_Nucleo EN 
                ON E.ID_Exemplar = EN.ID_Exemplar
            WHERE E.ID_Exemplar = @idExemplar";

            var param = new Dictionary<string, object>
            {
                { "@idExemplar", exemplarId }
            };

            return DALPro.Query<ExemplarDTO>(sql, param).FirstOrDefault();

        }

        public int CountByObraAndNucleo(long obraId, long nucleoId, string tag)
        {
            DALPro.ConnectionString = GetConnectionsString(tag);

            string sql = @"
            SELECT COUNT(*)
            FROM Exemplares_Nucleo EN
            JOIN Exemplares E 
                ON EN.ID_Exemplar = E.ID_Exemplar
            WHERE E.ID_Obra = @obraId
              AND EN.ID_Nucleo = @nucleoId";

            var param = new Dictionary<string, object>
            {
                { "@obraId", obraId },
                { "@nucleoId", nucleoId }
            };

            object result = DALPro.ExecuteScalar(sql, param);

            return result != null ? Convert.ToInt32(result) : 0;
        }
    }
}
