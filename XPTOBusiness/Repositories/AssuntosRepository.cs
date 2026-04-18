using DalPro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XPTOBusiness.Models;

namespace XPTOBusiness.Repositories
{
    public class AssuntosRepository : IAssuntosRepository
    {
        public IEnumerable<Assuntos> GetAll()
        {
            return DALPro.Query<Assuntos>("SELECT ID_Assunto, Assunto FROM Assuntos");
        }

        public Assuntos? GetById(byte id)
        {
            string sql = "SELECT ID_Assunto, Assunto FROM Assuntos WHERE ID_Assunto = @id";
            var par = new Dictionary<string, object> { { "@id", id } };
            return DALPro.Query<Assuntos>(sql, par).FirstOrDefault();
        }

        public void Add(Assuntos assunto)
        {
            string sql = "INSERT INTO Assuntos (Assunto) VALUES (@Assunto)";
            var par = new Dictionary<string, object> { { "@Assunto", assunto.Assunto ?? "" } };
            DALPro.Execute(sql, par);
        }

        public void Update(Assuntos assunto)
        {
            string sql = "UPDATE Assuntos SET Assunto = @Assunto WHERE ID_Assunto = @id";
            var par = new Dictionary<string, object> {
            { "@id", assunto.ID_Assunto },
            { "@Assunto", assunto.Assunto ?? "" }
        };
            DALPro.Execute(sql, par);
        }

        public void Delete(byte id)
        {
            DALPro.Execute("DELETE FROM Assuntos WHERE ID_Assunto = @id",
                new Dictionary<string, object> { { "@id", id } });
        }
    }
}