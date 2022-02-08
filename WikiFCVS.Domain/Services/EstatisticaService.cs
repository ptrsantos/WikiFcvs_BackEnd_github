using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WikiFCVS.Domain.Intefaces.Notificacoes;
using WikiFCVS.Domain.Intefaces.Repository;
using WikiFCVS.Domain.Intefaces.Services;
using WikiFCVS.Domain.Models;
using System.Linq;

namespace WikiFCVS.Domain.Services
{
    public class EstatisticaService : BaseService, IEstatisticaService
    {
        private readonly IEdicaoArtigoRepository EdicaoRepository;
        private readonly IArtigoRepository ArtigoRepository;
        private readonly IRegistroUsuarioRepository RegistroUsuarioRepository;

        private ICollection<Artigo> ListaArtigosGlobal;
        private ICollection<EdicaoArtigo> ListaEdicoesGlobal;
        private ICollection<RegistroUsuario> ListaRegistroUsuariosGloboal;


        public EstatisticaService(IEdicaoArtigoRepository edicaoRepository,
                                  IArtigoRepository artigoRepository,
                                  IRegistroUsuarioRepository registroUsuarioRepository,
                                  INotificador notificador) : base(notificador)
        {
            EdicaoRepository = edicaoRepository;
            ArtigoRepository = artigoRepository;
            RegistroUsuarioRepository = registroUsuarioRepository;
        }



        public async Task<ICollection<MesTotal>> ListarEstatisticaArtigos()
        {
            this.ListaArtigosGlobal = await ArtigoRepository.ListarArtigosIncluidosPorSemestre();
            ICollection<MesTotal> quantidadeArtigosPorMes = CapturaQuantidadePorPeriodo("artigo");
            return quantidadeArtigosPorMes;
        }

        public async Task<ICollection<MesTotal>> ListarEstatisticaEdicoes()
        {
            this.ListaEdicoesGlobal = await EdicaoRepository.ListarEdicoesIncluidasPorSemestre();
            ICollection<MesTotal> quantidadeEdicoesPorMes = CapturaQuantidadePorPeriodo("edicao");
            return quantidadeEdicoesPorMes;
        }

        public async Task<ICollection<MesTotal>> ListarEstatisticaUsuarios()
        {
            this.ListaRegistroUsuariosGloboal = await RegistroUsuarioRepository.ListarRegistrosUsuariosPorSemestre();
            ICollection<MesTotal> quantidadeRegistrosUsuariosPorMes =  CapturaQuantidadePorPeriodo("registroUsuario");
            return quantidadeRegistrosUsuariosPorMes;
        }

        public void Dispose()
        {
            ArtigoRepository?.Dispose();
            EdicaoRepository?.Dispose();
            RegistroUsuarioRepository?.Dispose();
        }

        private ICollection<MesTotal> CapturaQuantidadePorPeriodo(string tipo)
        {
            try
            {
                ICollection<MesTotal> quantidadePorMes = new List<MesTotal>();

                switch (tipo)
                {
                    case "registroUsuario":
                        for (int diferenca = -7; diferenca <= 0; diferenca++)
                        {
                            var dataProcurada = DateTime.Now.AddMonths(diferenca);
                            ICollection<RegistroUsuario> retorno = this.ListaRegistroUsuariosGloboal
                                                                       .Where(ru => ru.DataRegistro.Month == dataProcurada.Month
                                                                              && ru.DataRegistro.Year == dataProcurada.Year).ToList();
                            MesTotal mesTotal = new MesTotal
                            {
                                DataRegistro = dataProcurada,
                                MesNumerico = dataProcurada.Month,
                                AnoMumerico = dataProcurada.Year,
                                MesLiteral = RetornaMesLiteral(dataProcurada.Month),
                                Total = retorno.Count()
                            };
                            quantidadePorMes.Add(mesTotal);
                        };
                        break;
                    case "edicao":
                        for (int diferenca = -7; diferenca <= 0; diferenca++)
                        {
                            var dataProcurada = DateTime.Now.AddMonths(diferenca);
                            ICollection<EdicaoArtigo> retorno = this.ListaEdicoesGlobal
                                                                    .Where(e => e.EdicaoEfetuada.EditadoEm.Month == dataProcurada.Month
                                                                           && e.EdicaoEfetuada.EditadoEm.Year == dataProcurada.Year).ToList();
                            MesTotal mesTotal = new MesTotal
                            {
                                DataRegistro = dataProcurada,
                                MesNumerico = dataProcurada.Month,
                                AnoMumerico = dataProcurada.Year,
                                MesLiteral = RetornaMesLiteral(dataProcurada.Month),
                                Total = retorno.Count()
                            };
                            quantidadePorMes.Add(mesTotal);
                        };
                        break;
                    case "artigo":
                        for (int diferenca = -7; diferenca <= 0; diferenca++)
                        {
                            var dataProcurada = DateTime.Now.AddMonths(diferenca);
                            ICollection<Artigo> retorno = ListaArtigosGlobal.Distinct()
                                                          .Where(a => a.Edicoes.Any(ed => ed.EdicaoEfetuada.EditadoEm.Month == dataProcurada.Month
                                                                 && ed.EdicaoEfetuada.EditadoEm.Year == dataProcurada.Year)).ToList();
                            MesTotal mesTotal = new MesTotal
                            {
                                DataRegistro = dataProcurada,
                                MesNumerico = dataProcurada.Month,
                                AnoMumerico = dataProcurada.Year,
                                MesLiteral = RetornaMesLiteral(dataProcurada.Month),
                                Total = retorno.Count()
                            };
                            quantidadePorMes.Add(mesTotal);
                        };
                        break;
                }

                return quantidadePorMes.OrderBy(e => e.MesNumerico).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string RetornaMesLiteral(int mes)
        {
            string mesLiteral = "";
            switch (mes)
            {
                case 1: mesLiteral = "janeiro"; break;
                case 2: mesLiteral = "feveiro"; break;
                case 3: mesLiteral = "março"; break;
                case 4: mesLiteral = "abril"; break;
                case 5: mesLiteral = "maio"; break;
                case 6: mesLiteral = "junho"; break;
                case 7: mesLiteral = "julho"; break;
                case 8: mesLiteral = "agosto"; break;
                case 9: mesLiteral = "setembro"; break;
                case 10: mesLiteral = "outubro"; break;
                case 11: mesLiteral = "novembro"; break;
                case 12: mesLiteral = "dezembro"; break;
            }
            return mesLiteral;
        }


    }
}
