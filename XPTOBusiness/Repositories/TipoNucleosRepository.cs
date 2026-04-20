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
            string sql = "SELECT * FROM Tipo_Nucleos";
            DataTable dt = DalPro.DALPro.ExecuteQuery(sql);
            var lista = new List<TipoNucleo>();
            foreach (DataRow row in dt.Rows)
            {
                lista.Add(new TipoNucleo
                {
                    ID_TipoNucleo = Convert.ToByte(row["ID_Tipo_Nucleo"]),
                    Descricao = row["Descricao"].ToString()
                });
            }
            return lista;
        }

        public TipoNucleo GetById(byte id)
        {
            string sql = "SELECT * FROM Tipo_Nucleos WHERE ID_Tipo_Nucleo = @id";
            var p = new Dictionary<string, object> { { "@id", id } };
            DataTable dt = DalPro.DALPro.ExecuteQuery(sql, parameters: p);
            if (dt.Rows.Count == 0) return null;
            return new TipoNucleo
            {
                ID_TipoNucleo = Convert.ToByte(dt.Rows[0]["ID_Tipo_Nucleo"]),
                Descricao = dt.Rows[0]["Descricao"].ToString()
            };
        }

        public void Add(TipoNucleo tipo)
        {
            string sql = "INSERT INTO Tipo_Nucleos (Descricao) VALUES (@desc)";
            var p = new Dictionary<string, object> { { "@desc", tipo.Descricao } };
            DalPro.DALPro.ExecuteNonQuery(sql, parameters: p);
        }

        public void Update(TipoNucleo tipo)
        {
            string sql = "UPDATE Tipo_Nucleos SET Descricao = @desc WHERE ID_Tipo_Nucleo = @id";
            var p = new Dictionary<string, object> { { "@desc", tipo.Descricao }, { "@id", tipo.ID_TipoNucleo } };
            DalPro.DALPro.ExecuteNonQuery(sql, parameters: p);
        }

        public void Delete(byte id)
        {
            string sql = "DELETE FROM Tipo_Nucleos WHERE ID_Tipo_Nucleo = @id";
            var p = new Dictionary<string, object> { { "@id", id } };
            DalPro.DALPro.ExecuteNonQuery(sql, parameters: p);
        }
    }
}
