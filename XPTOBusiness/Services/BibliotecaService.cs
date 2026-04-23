using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XPTOBusiness.DTOs;
using XPTOBusiness.Repositories;

namespace XPTOBusiness.Services
{
    public interface IBibliotecaService
    {
        public List<SituacaoLeitorDTO> ObterSituacaoLeitor(int userId, string tag);


    }
    public class BibliotecaService : IBibliotecaService
    {
        private readonly ILeitorRepository _leitorRepo;

        public BibliotecaService(ILeitorRepository leitorRepo) => _leitorRepo = leitorRepo;

        public List<SituacaoLeitorDTO> ObterSituacaoLeitor(int userId, string tag)
        {
            var dados = _leitorRepo.GetSituacaoAtiva(userId, tag);
            var agora = DateTime.Now;

            return dados.Select(d =>
            {
                
                var diff = (d.DataLimite - agora).TotalDays;
                string status = "Devolver em breve";

                if (diff < 0) status = "ATRASO";
                else if (diff < 3) status = "Devolução URGENTE";
                else if (diff <= 5) status = "Devolver em breve";
                else status = "Regular";

                return new SituacaoLeitorDTO
                {
                    Titulo = d.Titulo,
                    Nucleo = d.Nucleo,
                    DataLimite = d.DataLimite,
                    Status = status
                };
            }).ToList();
        }
    }
}
