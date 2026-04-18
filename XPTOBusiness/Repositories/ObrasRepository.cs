using DalPro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XPTOBusiness.Models;

namespace XPTOBusiness.Repositories
{
    public class ObraRepository : IObrasRepository
    {
        public IEnumerable<Obra> GetAll()
        {
            string sql = "SELECT ID_Obra, Autor, ISBN, Titulo, Capa, ID_Assunto FROM Obras";
            return DALPro.Query<Obra>(sql);
        }

        public Obra? GetById(long id)
        {
            string sql = "SELECT ID_Obra, Autor, ISBN, Titulo, Capa, ID_Assunto FROM Obras WHERE ID_Obra = @id";
            var par = new Dictionary<string, object> { { "@id", id } };
            return DALPro.Query<Obra>(sql, par).FirstOrDefault();
        }

        public void Add(Obra obra)
        {
            string sql = @"INSERT INTO Obras (Autor, ISBN, Titulo, Capa, ID_Assunto) 
                       VALUES (@Autor, @ISBN, @Titulo, @Capa, @ID_Assunto)";

            var par = new Dictionary<string, object> {
            { "@Autor", obra.Autor },
            { "@ISBN", obra.ISBN },
            { "@Titulo", obra.Titulo},
            { "@Capa", obra.Capa },
            { "@ID_Assunto", obra.ID_Assunto }
        };
            DALPro.Execute(sql, par);
        }

        public void Update(Obra obra)
        {
            string sql = @"UPDATE Obras SET Autor=@Autor, ISBN=@ISBN, Titulo=@Titulo, 
                       Capa=@Capa, ID_Assunto=@ID_Assunto WHERE ID_Obra=@id";

            var par = new Dictionary<string, object> {
            { "@id", obra.ID_Obra },
            { "@Autor", obra.Autor },
            { "@ISBN", obra.ISBN },
            { "@Titulo", obra.Titulo },
            { "@Capa", obra.Capa },
            { "@ID_Assunto", obra.ID_Assunto }
        };
            DALPro.Execute(sql, par);
        }

        public void Delete(long id)
        {
            string sql = "DELETE FROM Obras WHERE ID_Obra = @id";
            var par = new Dictionary<string, object> { { "@id", id } };
            DALPro.Execute(sql, par);
        }
    }
}