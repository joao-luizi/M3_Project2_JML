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
    public interface IAssuntosRepository
    {
        IEnumerable<Assuntos> GetAll(string tag);
        Assuntos GetById(byte id, string tag);
        void Add(Assuntos assunto, string tag);
        void Update(Assuntos assunto, string tag);
        void Delete(byte id, string tag);
    }

    public class AssuntosRepository : IAssuntosRepository
    {
        private readonly IConfiguration _configuration;
        public AssuntosRepository(IConfiguration config)
        {
            _configuration = config;
        }
        private string GetConnectionsString(string tag)
        {
            var connectionString = _configuration.GetConnectionString(tag) ?? throw new Exception($"Connection string for tag: {tag} not found!");
            return connectionString;
        }
        public IEnumerable<Assuntos> GetAll(string tag)
        {
            DALPro.ConnectionString = GetConnectionsString(tag);
            return DALPro.Query<Assuntos>("SELECT ID_Assunto, Assunto FROM Assuntos");
        }

        public Assuntos? GetById(byte id, string tag)
        {
            DALPro.ConnectionString = GetConnectionsString(tag);
            string sql = "SELECT ID_Assunto, Assunto FROM Assuntos WHERE ID_Assunto = @id";
            var par = new Dictionary<string, object> { { "@id", id } };
            return DALPro.Query<Assuntos>(sql, par).FirstOrDefault();
        }

        public void Add(Assuntos assunto, string tag)
        {
            DALPro.ConnectionString = GetConnectionsString(tag);
            string sql = "INSERT INTO Assuntos (Assunto) VALUES (@Assunto)";
            var par = new Dictionary<string, object> { { "@Assunto", assunto.Assunto ?? "" } };
            DALPro.Execute(sql, par);
        }

        public void Update(Assuntos assunto, string tag)
        {
            DALPro.ConnectionString = GetConnectionsString(tag);
            string sql = "UPDATE Assuntos SET Assunto = @Assunto WHERE ID_Assunto = @id";
            var par = new Dictionary<string, object> {
            { "@id", assunto.ID_Assunto },
            { "@Assunto", assunto.Assunto }
        };
            DALPro.Execute(sql, par);
        }

        public void Delete(byte id, string tag)
        {
            DALPro.ConnectionString = GetConnectionsString(tag);
            DALPro.Execute("DELETE FROM Assuntos WHERE ID_Assunto = @id",
                new Dictionary<string, object> { { "@id", id } });
        }
    }
}