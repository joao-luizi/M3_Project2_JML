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

    namespace XPTOBusiness.Services
    {
        public class NucleoService
        {
            private readonly INucleoRepository _nucleoRepo;
            private readonly ITipoNucleoRepository _tipoRepo;

            public NucleoService(INucleoRepository nucleoRepo, ITipoNucleoRepository tipoRepo)
            {
                _nucleoRepo = nucleoRepo;
                _tipoRepo = tipoRepo;
            }

            public IEnumerable<NucleoDTO> ObterTodos()
            {
                var nucleos = _nucleoRepo.GetAll();
                var tipos = _tipoRepo.GetAll();

                return nucleos.Select(n => new NucleoDTO(
                    n.ID_Nucleo,
                    n.Nome,
                    n.Local,
                    tipos.FirstOrDefault(t => t.ID_TipoNucleo == n.ID_TipoNucleo)?.Descricao ?? "N/A"
                ));
            }

            //pontos 7 e 9
            //public void TransferirExemplares(TransferenciaExemplaresDTO dados)
            //{
            //    foreach (var idExemplar in dados.IdsExemplares)
            //    {
            //        var vinculoAtual = _exemplaresRepo.GetByExemplarId(idExemplar);
            //        var totalNoNucleo = _exemplaresRepo.GetAll()
            //                            .Count(x => x.ID_Nucleo == vinculoAtual.ID_Nucleo);

            //        if (totalNoNucleo <= 1)
            //        {
            //            throw new Exception($"Transferência negada: O núcleo {vinculoAtual.ID_Nucleo} " +
            //                                "não pode ficar com menos de 1 exemplar para consulta.");
            //        }
            //    }

            //    string listaCsv = string.Join(",", dados.IdsExemplares);
            //    _nucleoRepo.TransferirExemplares(listaCsv, dados.IdNucleoDestino);
            //}

            public IEnumerable<ResumoRequisicoesDTO> ObterResumoRequisicoes(DateTime inicio, DateTime fim)
            {
                DataTable dt = _nucleoRepo.GetRequisicoesPorPeriodo(inicio, fim);
                var lista = new List<ResumoRequisicoesDTO>();

                foreach (DataRow row in dt.Rows)
                {
                    lista.Add(new ResumoRequisicoesDTO(
                        row["Nome"].ToString() ?? "",
                        row["Local"].ToString() ?? "",
                        Convert.ToInt32(row["Requisições no Período:"])
                    ));
                }
                return lista;
            }

            public IEnumerable<DisponibilidadeDTO> ObterDisponibilidade(bool porAssunto)
            {
                DataTable dt = porAssunto
                    ? _nucleoRepo.GetDisponibilidadePorNucleoeAssunto()
                    : _nucleoRepo.GetDisponibilidadePorNucleo();

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

            public void CriarNucleo(SaveNucleoDTO dto)
            {
                var model = new Nucleo
                {
                    Nome = dto.Nome,
                    Local = dto.Local,
                    ID_TipoNucleo = dto.IdTipoNucleo
                };
                _nucleoRepo.Add(model);
            }

            //ponto 13
            public dynamic ObterDadosDecisao()
            {
                var disponibilidade = ObterDisponibilidade(false);

                return new
                {
                    NucleosParaReforco = disponibilidade.Where(d => d.DisponiveisParaRequisicao < 2),
                    NucleosCandidatosEncerramento = disponibilidade.Where(d => d.Total < 5),
                    SugestoesLeitura = _nucleoRepo.GetRequisicoesPorPeriodo(DateTime.Now.AddMonths(-1), DateTime.Now)
                };
            }
        }
    }
}
