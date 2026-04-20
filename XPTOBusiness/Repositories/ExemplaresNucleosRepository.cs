using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XPTOBusiness.Models;

namespace XPTOBusiness.Repositories
{
    public class ExemplaresNucleosRepository : IExemplaresNucleosRepository
    {
        public IEnumerable<ExemplarNucleo> GetAll()
        {
            string sql = "SELECT * FROM Exemplares_Nucleo";
            DataTable dt = DalPro.DALPro.ExecuteQuery(sql);
            var lista = new List<ExemplarNucleo>();
            foreach (DataRow row in dt.Rows)
            {
                lista.Add(new ExemplarNucleo
                {
                    ID_Exemplar = Convert.ToInt64(row["ID_Exemplar"]),
                    ID_Nucleo = Convert.ToInt32(row["ID_Nucleo"])
                });
            }
            return lista;
        }

        public ExemplarNucleo GetByExemplarId(long idExemplar)
        {
            string sql = "SELECT * FROM Exemplares_Nucleo WHERE ID_Exemplar = @id";
            var p = new Dictionary<string, object> { { "@id", idExemplar } };
            DataTable dt = DalPro.DALPro.ExecuteQuery(sql, parameters: p);
            if (dt.Rows.Count == 0) return null;
            return new ExemplarNucleo
            {
                ID_Exemplar = Convert.ToInt64(dt.Rows[0]["ID_Exemplar"]),
                ID_Nucleo = Convert.ToInt32(dt.Rows[0]["ID_Nucleo"])
            };
        }

        public void Add(ExemplarNucleo en)
        {
            string sql = "INSERT INTO Exemplares_Nucleo (ID_Exemplar, ID_Nucleo) VALUES (@idEx, @idNuc)";
            var p = new Dictionary<string, object> { { "@idEx", en.ID_Exemplar }, { "@idNuc", en.ID_Nucleo } };
            DalPro.DALPro.ExecuteNonQuery(sql, parameters: p);
        }

        public void Update(ExemplarNucleo en)
        {
            string sql = "UPDATE Exemplares_Nucleo SET ID_Nucleo = @idNuc WHERE ID_Exemplar = @idEx";
            var p = new Dictionary<string, object> { { "@idNuc", en.ID_Nucleo }, { "@idEx", en.ID_Exemplar } };
            DalPro.DALPro.ExecuteNonQuery(sql, parameters: p);
        }

        public void Delete(long idExemplar)
        {
            string sql = "DELETE FROM Exemplares_Nucleo WHERE ID_Exemplar = @id";
            var p = new Dictionary<string, object> { { "@id", idExemplar } };
            DalPro.DALPro.ExecuteNonQuery(sql, parameters: p);
        }
    }
}
