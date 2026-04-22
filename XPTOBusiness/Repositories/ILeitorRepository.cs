using DalPro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XPTOBusiness.DTOs;

namespace XPTOBusiness.Repositories
{
    public interface ILeitorRepository
    {
        public List<SituacaoLeitorDTO> GetSituacaoAtiva(int userId);

        public List<HistoricoLeitorDTO> GetHistorico(int userId, int? nucleoId, DateTime? inicio, DateTime? fim);
    }
}