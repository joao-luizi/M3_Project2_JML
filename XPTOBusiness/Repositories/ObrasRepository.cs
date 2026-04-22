using DalPro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XPTOBusiness.Models;
using XPTOBusiness.DTOs;

namespace XPTOBusiness.Repositories
{
    public class ObrasRepository : IObrasRepository
    {
        public List<ObraDTO> Search(string termo) =>
            DALPro.Query<ObraDTO>("EXEC sp_Obras_Search @Termo", new() { { "@Termo", termo ?? "" } });

        public void CreateUpdate(ObraDTO obra) =>
            DALPro.Execute("EXEC sp_Obras_Upsert @ID_Obra, @Titulo, @Autor, @ISBN, @Capa, @ID_Assunto", new() {
            { "@ID_Obra", obra.ID_Obra },
            { "@Titulo", obra.Titulo },
            { "@Autor", obra.Autor },
            { "@ISBN", obra.ISBN },
            { "@Capa", obra.Capa ?? (object)DBNull.Value },
            { "@ID_Assunto", obra.ID_Assunto }
            });

        public void Delete(long id) =>
            DALPro.Execute("EXEC sp_Obras_Delete @ID_Obra", new() { { "@ID_Obra", id } });
    }
}