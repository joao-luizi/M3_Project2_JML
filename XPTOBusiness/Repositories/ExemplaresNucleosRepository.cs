using DalPro;
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
    public class ExemplaresNucleosRepository : IExemplaresNucleosRepository
    {
        public IEnumerable<ExemplarNucleo> GetAll()
        {
            string sql = "SELECT * FROM Exemplares_Nucleo";
            var lista = DALPro.Query<ExemplarNucleo>(sql);
            return lista;
        }

        public ExemplarNucleo GetByExemplarId(long idExemplar)
        {
            string sql = "SELECT * FROM Exemplares_Nucleo WHERE ID_Exemplar = @id";
            var p = new Dictionary<string, object> { { "@id", idExemplar } };
            var list = DALPro.Query<ExemplarNucleo>(sql, parameters: p);
            if (list == null || list.Count == 0) return null;
            return list[0];
        }

        public void Add(ExemplarNucleo en)
        {
            string sql = "INSERT INTO Exemplares_Nucleo (ID_Exemplar, ID_Nucleo) VALUES (@idEx, @idNuc)";
            var p = new Dictionary<string, object> { { "@idEx", en.ID_Exemplar }, { "@idNuc", en.ID_Nucleo } };
            DALPro.Execute(sql, parameters: p);
        }

        public void Update(ExemplarNucleo en)
        {
            string sql = "UPDATE Exemplares_Nucleo SET ID_Nucleo = @idNuc WHERE ID_Exemplar = @idEx";
            var p = new Dictionary<string, object> { { "@idNuc", en.ID_Nucleo }, { "@idEx", en.ID_Exemplar } };
            DALPro.Execute(sql, parameters: p);
        }

        public void Delete(long idExemplar)
        {
            string sql = "DELETE FROM Exemplares_Nucleo WHERE ID_Exemplar = @id";
            var p = new Dictionary<string, object> { { "@id", idExemplar } };
            DALPro.Execute(sql, parameters: p);
        }
    }
}
