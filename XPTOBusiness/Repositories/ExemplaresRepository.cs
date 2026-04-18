using DalPro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XPTOBusiness.Models;

namespace XPTOBusiness.Repositories
{
    public class ExemplarRepository : IExemplaresRepository
    {
        public IEnumerable<Exemplares> GetAll()
        {
            return DALPro.Query<Exemplares>("SELECT ID_Exemplar, ID_Obra FROM Exemplares");
        }

        public Exemplares? GetById(long id)
        {
            string sql = "SELECT ID_Exemplar, ID_Obra FROM Exemplares WHERE ID_Exemplar = @id";
            var par = new Dictionary<string, object> { { "@id", id } };
            return DALPro.Query<Exemplares>(sql, par).FirstOrDefault();
        }

        public void Add(Exemplares exemplar)
        {
            string sql = "INSERT INTO Exemplares (ID_Obra) VALUES (@ID_Obra)";
            var par = new Dictionary<string, object> { { "@ID_Obra", exemplar.ID_Obra } };
            DALPro.Execute(sql, par);
        }

        public void Update(Exemplares exemplar)
        {
            string sql = "UPDATE Exemplares SET ID_Obra = @ID_Obra WHERE ID_Exemplar = @id";
            var par = new Dictionary<string, object> {
            { "@id", exemplar.ID_Exemplar },
            { "@ID_Obra", exemplar.ID_Obra }
        };
            DALPro.Execute(sql, par);
        }

        public void Delete(long id)
        {
            DALPro.Execute("DELETE FROM Exemplares WHERE ID_Exemplar = @id",
                new Dictionary<string, object> { { "@id", id } });
        }
    }
}
