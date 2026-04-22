using DalPro;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XPTOBusiness.Models;

namespace XPTOBusiness.Repositories
{
    public class TipoNucleoRepository : ITipoNucleoRepository
    {
        public IEnumerable<TipoNucleo> GetAll()
        {
            string sql = "SELECT * FROM TipoNucleos";
            var lista = DALPro.Query<TipoNucleo>(sql);
            return lista;
        }

        public TipoNucleo? GetById(byte id)
        {
            string sql = "SELECT * FROM Tipo_Nucleos WHERE ID_TipoNucleo = @id";
            var p = new Dictionary<string, object> { { "@id", id } };
            var lista = DALPro.Query<TipoNucleo>(sql, parameters: p);
            return lista.FirstOrDefault();
        }

        public void Add(TipoNucleo tipo)
        {
            string sql = "INSERT INTO TipoNucleos (Descricao) VALUES (@desc)";
            var p = new Dictionary<string, object> { { "@desc", tipo.Descricao } };
            DALPro.Execute(sql, parameters: p);
        }

        public void Update(TipoNucleo tipo)
        {
            string sql = "UPDATE TipoNucleos SET Descricao = @desc WHERE ID_TipoNucleo = @id";
            var p = new Dictionary<string, object> { { "@desc", tipo.Descricao }, { "@id", tipo.ID_TipoNucleo } };
            DALPro.Execute(sql, parameters: p);
        }

        public void Delete(byte id)
        {
            string sql = "DELETE FROM TipoNucleos WHERE ID_TipoNucleo = @id";
            var p = new Dictionary<string, object> { { "@id", id } };
            DALPro.Execute(sql, parameters: p);
        }
    }
}
