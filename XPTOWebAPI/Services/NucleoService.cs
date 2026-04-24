using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPTOBusiness.Services
{
    using global::XPTOBusiness.DTOs;
    using global::XPTOBusiness.Models;
    using global::XPTOBusiness.Repositories;
    using System.Data;
    using XPTOWebAPI.Services;

    namespace XPTOBusiness.Services
    {
        public interface INucleoService
        {
            public IEnumerable<NucleoDTO> ObterTodos(string tag);
            public void TransferirExemplares(TransferenciaExemplaresDTO dados, string tag);
            public IEnumerable<ResumoRequisicoesDTO> ObterResumoRequisicoes(DateTime inicio, DateTime fim, string tag);
            public IEnumerable<DisponibilidadeDTO> ObterDisponibilidade(bool porAssunto, string tag);
            public void CriarNucleo(SaveNucleoDTO dto, string tag);
            public dynamic ObterDadosDecisao(string tag);
        }

        public class NucleoService : INucleoService
        {
            private readonly INucleoRepository _nucleoRepo;
            private readonly ITipoNucleoRepository _tipoRepo;
            private readonly IExemplaresNucleosRepository _exemplaresRepos;
            private readonly ILogger _logger;

            public NucleoService(ILogger<NucleoService> logger, INucleoRepository nucleoRepo, 
                ITipoNucleoRepository tipoRepo, IExemplaresNucleosRepository exemplaresRepos)
            {
                _logger = logger;
                _nucleoRepo = nucleoRepo;
                _tipoRepo = tipoRepo;
                _exemplaresRepos = exemplaresRepos;
            }

            public IEnumerable<NucleoDTO> ObterTodos(string tag)
            {
                var nucleos = _nucleoRepo.GetAll(tag);
                var tipos = _tipoRepo.GetAll(tag);

                return nucleos.Select(n => new NucleoDTO(
                    n.ID_Nucleo,
                    n.Nome,
                    n.Local,
                    tipos.FirstOrDefault(t => t.ID_TipoNucleo == n.ID_TipoNucleo)?.Descricao ?? "N/A"
                ));
            }

            //pontos 7 e 9
            public void TransferirExemplares(TransferenciaExemplaresDTO dados, string tag)
            {
                foreach (var idExemplar in dados.IdsExemplares)
                {
                    var vinculoAtual = _exemplaresRepos.GetByExemplarId(idExemplar, tag);
                    var totalNoNucleo = _exemplaresRepos.GetAll(tag)
                                        .Count(x => x.ID_Nucleo == vinculoAtual.ID_Nucleo);

                    if (totalNoNucleo <= 1)
                    {
                        throw new Exception($"Transferência negada: O núcleo {vinculoAtual.ID_Nucleo} " +
                                            "não pode ficar com menos de 1 exemplar para consulta.");
                    }
                }

                string listaCsv = string.Join(",", dados.IdsExemplares);
                _nucleoRepo.TransferirExemplares(listaCsv, dados.IdNucleoDestino, tag);
            }

            public IEnumerable<ResumoRequisicoesDTO> ObterResumoRequisicoes(DateTime inicio, DateTime fim, string tag)
            {
                var rows = _nucleoRepo.GetRequisicoesPorPeriodo(inicio, fim, tag);

                var lista = new List<ResumoRequisicoesDTO>();

                foreach (dynamic row in rows)
                {
                    lista.Add(new ResumoRequisicoesDTO(
                        row.Nome?.ToString() ?? "",
                        row.Local?.ToString() ?? "",
                        Convert.ToInt32(row.Requisições_no_Período) // adjust if property name changed
                    ));
                }

                return lista;
            }

            public IEnumerable<DisponibilidadeDTO> ObterDisponibilidade(bool porAssunto, string tag)
            {
                DataTable dt = porAssunto
                    ? _nucleoRepo.GetDisponibilidadePorNucleoeAssunto(tag)
                    : _nucleoRepo.GetDisponibilidadePorNucleo(tag);

                var lista = new List<DisponibilidadeDTO>();

                foreach (DataRow row in dt.Rows)
                {
                    lista.Add(new DisponibilidadeDTO(
                        row["Titulo"].ToString() ?? "",
                        row["Autor"].ToString() ?? "",
                        row["Nucleo"].ToString() ?? "",
                        porAssunto ? row["Assunto"].ToString() : null,
                        Convert.ToInt32(row["Total"]),
                        Convert.ToInt32(row["Requisitadas"]),
                        Convert.ToInt32(row["PresencaObrigatoria"]),
                        Convert.ToInt32(row["DisponiveisParaRequisicao"])
                    ));
                }
                return lista;
            }

            public void CriarNucleo(SaveNucleoDTO dto, string tag)
            {
                var model = new Nucleo
                {
                    Nome = dto.Nome,
                    Local = dto.Local,
                    ID_TipoNucleo = dto.IdTipoNucleo
                };
                _nucleoRepo.Add(model, tag);
            }

            //ponto 13
            public dynamic ObterDadosDecisao(string tag)
            {
                var disponibilidade = ObterDisponibilidade(false, tag);

                return new
                {
                    NucleosParaReforco = disponibilidade.Where(d => d.DisponiveisParaRequisicao < 2),
                    NucleosCandidatosEncerramento = disponibilidade.Where(d => d.Total < 5),
                    SugestoesLeitura = _nucleoRepo.GetRequisicoesPorPeriodo(DateTime.Now.AddMonths(-1), DateTime.Now, tag)
                };
            }
        }
    }
}
