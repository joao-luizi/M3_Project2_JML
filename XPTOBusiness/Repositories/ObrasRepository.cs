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

        public void CreateUpdate(ObraDTO obra)
        {
            // Se o ID_Obra for nulo ou 0, é um CREATE (Inserir Nova)
            if (obra.ID_Obra == null || obra.ID_Obra == 0)
            {
                string sql = "EXEC Obras_InserirNova @Autor, @ISBN, @Titulo, @ID_Assunto";
                var p = new Dictionary<string, object> {
                    { "@Autor", obra.Autor ?? "" },
                    { "@ISBN", obra.ISBN ?? "" },
                    { "@Titulo", obra.Titulo ?? "" },
                    { "@ID_Assunto", obra.ID_Assunto },
                    { "@Capa", obra.Capa ?? (object)DBNull.Value }
                };
                DALPro.ExecuteSP(sql, p);
            }
            else
            {
                string sql = "EXEC Obras_Atualizar @ID_Obra, @Autor, @ISBN, @Titulo, @ID_Assunto, @Capa";
                var p = new Dictionary<string, object> {
                    { "@ID_Obra", obra.ID_Obra },
                    { "@Autor", obra.Autor ?? "" },
                    { "@ISBN", obra.ISBN ?? "" },
                    { "@Titulo", obra.Titulo ?? "" },
                    { "@ID_Assunto", obra.ID_Assunto },
                    { "@Capa", obra.Capa ?? (object)DBNull.Value }
                };
                DALPro.ExecuteSP(sql, p);
            }
        }
        public void Delete(long id)
        {
            string sql = "EXEC Obras_Remover @ID_Obra";
            var p = new Dictionary<string, object> { { "@ID_Obra", id } };

            DALPro.ExecuteSP(sql, p);
        }
    }
}