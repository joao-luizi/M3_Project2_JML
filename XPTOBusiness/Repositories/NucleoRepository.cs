using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XPTOBusiness.Models;
using Microsoft.Data.SqlClient;
using DalPro;
using System.Data;

namespace XPTOBusiness.Repositories
{
    public class NucleoRepository : INucleoRepository
    {
        public void Add(Nucleo nucleo)
        {
            string sql = "INSERT INTO Nucleos (Nome, Local, ID_Tipo_Nucleo) VALUES (@nome, @local, @tipo)";
            var p = new Dictionary<string, object> {
                { "@nome", nucleo.Nome },
                { "@local", nucleo.Local },
                { "@tipo", nucleo.ID_TipoNucleo }
            };
        DALPro.ExecuteSP(sql, parameters: p);
        }

        public void Update(Nucleo nucleo)
        {
            string sql = "UPDATE Nucleos SET Nome=@nome, Local=@local, ID_Tipo_Nucleo=@tipo WHERE ID_Nucleo=@id";
            var p = new Dictionary<string, object> {
                { "@id", nucleo.ID_Nucleo },
                { "@nome", nucleo.Nome },
                { "@local", nucleo.Local },
                { "@tipo", nucleo.ID_TipoNucleo }
            };
            DALPro.ExecuteSP(sql, parameters: p);
        }

        public void Delete(int id)
        {
            string sql = "DELETE FROM Nucleos WHERE ID_Nucleo = @id";
            var p = new Dictionary<string, object> { { "@id", id } };
            DALPro.ExecuteSP(sql, parameters: p);
        }

        public void TransferirExemplares(string listaIds, long idDestino)
        {
            var p = new Dictionary<string, object> {
                { "@ListaIDsExemplares", listaIds },
                { "@ID_NucleoDestino", idDestino }
            };
            DALPro.ExecuteSP("Nucleos_TransferirExemplares", parameters: p);
        }

        public DataTable GetRequisicoesPorPeriodo(DateTime inicio, DateTime fim)
        {
            var p = new Dictionary<string, object> {
                { "@DataInicio", inicio },
                { "@DataFim", fim }
            };
            return DALPro.ExecuteSP("Nucleos_MostrarRequisicoes", parameters: p);
        }
        public IEnumerable<Nucleo> GetAll() { return new List<Nucleo>(); }
        public Nucleo GetById(int id) { return null; }
    }
}
