using Azure;
using DalPro;
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
    public interface IObrasRepository
    {
        // CRUD e Manutenção (Adicionar/remover/atualizar)
        public void CreateUpdate(ObraDTO obra, string tag);

        // Pesquisa
        public List<ObraDTO> Search(string termo, string tag);

        // Remoção
        public void Delete(long id, string tag);
    }
    public class ObrasRepository : IObrasRepository
    {
        private readonly IConfiguration _configuration;
        public ObrasRepository(IConfiguration config)
        {
            _configuration = config;
        }

        private string GetConnectionsString(string tag)
        {
            var connectionString = _configuration.GetConnectionString(tag) ?? throw new Exception($"Connection string for tag: {tag} not found!");
            return connectionString;
        }

        public List<ObraDTO> Search(string termo, string tag)
        {
            DALPro.ConnectionString = GetConnectionsString(tag);
            return DALPro.Query<ObraDTO>("EXEC sp_Obras_Search @Termo", new() { { "@Termo", termo ?? "" } });
        }

        public void CreateUpdate(ObraDTO obra, string tag)
        {
            DALPro.ConnectionString = GetConnectionsString(tag);
            DALPro.Execute("EXEC sp_Obras_Upsert @ID_Obra, @Titulo, @Autor, @ISBN, @Capa, @ID_Assunto", new() {
            { "@ID_Obra", obra.ID_Obra },
            { "@Titulo", obra.Titulo },
            { "@Autor", obra.Autor },
            { "@ISBN", obra.ISBN },
            { "@Capa", obra.Capa ?? (object)DBNull.Value },
            { "@ID_Assunto", obra.ID_Assunto }
            });
        }

        public void Delete(long id, string tag)
        {
            DALPro.ConnectionString = GetConnectionsString(tag);
            DALPro.Execute("EXEC sp_Obras_Delete @ID_Obra", new() { { "@ID_Obra", id } });
        }
       
    }
}