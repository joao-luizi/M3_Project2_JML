using DalPro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XPTOBusiness.DTOs;
using static XPTOBusiness.Repositories.ILeitorRepository;

namespace XPTOBusiness.Repositories
{
    public class LeitorRepository : ILeitorRepository
    {
        public List<SituacaoLeitorDTO> GetSituacaoAtiva(int userId) =>
            DALPro.Query<SituacaoLeitorDTO>("EXEC sp_Leitor_Situacao @ID_Utilizador", new() { { "@ID_Utilizador", userId } });

        public List<HistoricoLeitorDTO> GetHistorico(int userId, int? nucleoId, DateTime? inicio, DateTime? fim)
        {
            // Exemplo de Query direta se preferir não criar SP para filtros dinâmicos complexos
            string sql = "SELECT O.Titulo, TN.Descricao as Nucleo, R.DataRequisicao, R.DataEntrega FROM Requisicoes R " +
                         "JOIN Exemplares E ON R.ID_Exemplar = E.ID_Exemplar " +
                         "JOIN Obras O ON E.ID_Obra = O.ID_Obra " +
                         "JOIN TipoNucleos TN ON E.ID_TipoNucleo = TN.ID_TipoNucleo " +
                         "WHERE R.ID_Utilizador = @uid";

            return DALPro.Query<HistoricoLeitorDTO>(sql, new() { { "@uid", userId } });
        }
    }
}