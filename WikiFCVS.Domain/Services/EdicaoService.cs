using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WikiFCVS.Domain.Intefaces.Notificacoes;
using WikiFCVS.Domain.Intefaces.Repository;
using WikiFCVS.Domain.Intefaces.Services;
using WikiFCVS.Domain.Models;
using System.Linq;
using System.IO;
using DiffMatchPatch;
using WikiFCVS.Domain.Notificacoes;
using WikiFCVS.Domain.Handle;
using HtmlAgilityPack;
using System.Text.RegularExpressions;

namespace WikiFCVS.Domain.Services
{
    public class EdicaoService : BaseService, IEdicaoService
    {
        private readonly ITemaRepository TemaRepository;
        private readonly IArtigoRepository ArtigoRepository;
        private readonly IEdicaoArtigoRepository EdicaoArtigoRepository;

        public EdicaoService(ITemaRepository temaRepository,
                            IArtigoRepository artigoRepository,
                            IEdicaoArtigoRepository edicaoArtigoRepository,
                            INotificador notificador
                            ) : base(notificador)
        {
            TemaRepository = temaRepository;
            ArtigoRepository = artigoRepository;
            EdicaoArtigoRepository = edicaoArtigoRepository;
        }



        public void Dispose()
        {
            TemaRepository?.Dispose();
        }

        public async Task<ICollection<EdicaoArtigo>> ListarArtigosEdicoesHistorico(int pagina, int tamanho)
        {
            ICollection<EdicaoArtigo> edicoes = await EdicaoArtigoRepository.ListarArtigosEdicoesHistoricoPorPaginacao(pagina, tamanho);
            return edicoes;
        }

        public async Task<TransporteDadosTabela> ListarArtigosEdicoesHistoricoComFiltro(int pagina, int tamanho, string filtro)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine(filtro.ToLower());
                //ICollection<Edicao> edicoes = await EdicaoArtigoRepository.ListarTemasArtigosPorPaginacaoComFiltro(pagina, tamanho, filtro);
                IEnumerable<EdicaoArtigo> edicoes = await EdicaoArtigoRepository.ListarIQueryableArtigosPorPaginacaoComFiltro(pagina, tamanho, filtro);
                IEnumerable<EdicaoArtigo> totalListaEdicoes = from edicao in edicoes
                                                              where
                                                              (edicao.RetornaConteudoEmTextoPuro().ToLower().Contains(filtro.ToLower()) ||
                                                              edicao.RetornaTituloEmTextoPuro().ToLower().Contains(filtro.ToLower()))
                                                              select edicao;


                ICollection<EdicaoArtigo> listaEdicoesComPaginacao = totalListaEdicoes.Skip(tamanho * (pagina - 1)).Take(tamanho).ToList();

                TransporteDadosTabela transporteDadosTabela = new TransporteDadosTabela();
                transporteDadosTabela.Edicoes = listaEdicoesComPaginacao;
                transporteDadosTabela.TotalDeItens = totalListaEdicoes.Count();

                return transporteDadosTabela;
            }
            catch (Exception ex)
            {
                this.Notificar($"{ex.Message}; {ex.InnerException.Message}");
                throw ex;
            }
        }

        private void PalavraEncontrada(string textoProcurado, string textFonte)
        {
            //var source = textFonte;
            //Regex regex2 = new Regex("\\(([A-Za-z0-9çãàáâéêíóôõúÂÃÁÀÉÊÍÓÔÕÚÇ\"'!?$%:;,º°ª ]+')\\)");
            //GroupCollection capturas = regex2.Match(source).Groups;
            //System.Diagnostics.Debug.WriteLine(capturas["output"]);
            //System.Diagnostics.Debug.WriteLine(capturas["output"].Value);
            //System.Diagnostics.Debug.WriteLine(capturas["output"].Captures.Count);
            //System.Diagnostics.Debug.WriteLine(capturas["output"]);

            //if (capturas["output"].Captures.Count > 0)
            //{
            //    System.Diagnostics.Debug.WriteLine(textFonte);
            //}


            var withoutSpecial = new string(textFonte.Where(c => Char.IsLetterOrDigit(c)
                                            || Char.IsWhiteSpace(c)).ToArray());

            Regex regex1 = new Regex($"^~{textoProcurado}(?<output>\\([a-zA-Z0-9]+)/*/.*$\\)");
            GroupCollection capturas2 = regex1.Match(withoutSpecial).Groups;
            System.Diagnostics.Debug.WriteLine(capturas2["output"]);
            System.Diagnostics.Debug.WriteLine(capturas2["output"].Value);
            System.Diagnostics.Debug.WriteLine(capturas2["output"].Captures.Count);
            System.Diagnostics.Debug.WriteLine(textFonte);
            if (capturas2["output"].Captures.Count > 0)
            {
                System.Diagnostics.Debug.WriteLine(textFonte); ;
            }
            var fonteTratada = withoutSpecial.ToLower();
            var procuradoTratado = textoProcurado.ToLower();
            var retorno = fonteTratada.ToLower().IndexOf(procuradoTratado.ToLower(), 0, withoutSpecial.Length);
            System.Diagnostics.Debug.WriteLine(retorno);
        }


        public async Task<EdicaoArtigo> RetornaEdicaoPaginaHome()
        {
            EdicaoArtigo edicao = await EdicaoArtigoRepository.RetornaEdicaoArtigoTemaHome();
            return edicao;
        }

        public async Task<int> RetornaQuantidadeArtigoEdicoes()
        {
            return await EdicaoArtigoRepository.RetornaQuantidadeArtigoEdicoes();
        }

        public async Task<EdicaoArtigo> RetornaArtigoEdicao(int edicaoId)
        {
            return await EdicaoArtigoRepository.RetornaArtigoEdicao(edicaoId);
        }

        public async Task<EdicaoArtigo> RetornaArtigoEdicaoHistorico(int edicaoId)
        {
            EdicaoArtigo edicaoAtual = await EdicaoArtigoRepository.RetornaArtigoEdicao(edicaoId);
            EdicaoArtigo edicaoAnterior = await EdicaoArtigoRepository.RetornaArtigoEdicaoAnterior(edicaoAtual);
            if (edicaoAnterior != null)
            {
                EdicaoArtigo edicaoHistorico = RetornaArtigoEdicaoComAlteracoes(edicaoAtual, edicaoAnterior);
                return edicaoHistorico;
            }
            else
            {
                DiffMatchPatch.diff_match_patch xdiff_Titulo = new diff_match_patch();
                List<DiffMatchPatch.Diff> diffs_Titulo = xdiff_Titulo.diff_main(edicaoAtual.RetornaTituloEmTextoPuro(), edicaoAtual.RetornaTituloEmTextoPuro(), true);
                xdiff_Titulo.diff_cleanupSemantic(diffs_Titulo);
                string html_Titulo = xdiff_Titulo.diff_prettyHtml(diffs_Titulo);
                edicaoAtual.Titulo = html_Titulo;

                DiffMatchPatch.diff_match_patch xdiff_Conteudo = new diff_match_patch();
                List<DiffMatchPatch.Diff> diffs_Conteudo = xdiff_Conteudo.diff_main(edicaoAtual.RetornaConteudoEmTextoPuro(), edicaoAtual.RetornaConteudoEmTextoPuro(), true);
                xdiff_Conteudo.diff_cleanupSemantic(diffs_Conteudo);
                string html_Conteudo = xdiff_Conteudo.diff_prettyHtml(diffs_Conteudo);
                //html_Conteudo = html_Conteudo.Replace("<del style=\"background:#ffe6e6;\">", "<del style='background:#228b22;'>"); //.Replace("<ins style=\"background:#e6ffe6;\">", "<ins style='background:#A9F5E1;'>");
                edicaoAtual.Conteudo = html_Conteudo.Replace("&para;", "");
                return edicaoAtual;
            }
        }

        private EdicaoArtigo RetornaArtigoEdicaoComAlteracoes(EdicaoArtigo edicaoAtual, EdicaoArtigo edicaoAnterior)
        {
            //Edicao edicaoHistorico = new Edicao()

            try
            {
                //Pass the strings to be matched for Diff
                DiffMatchPatch.diff_match_patch xdiff_Titulo = new diff_match_patch();
                List<DiffMatchPatch.Diff> diffs_Titulo = xdiff_Titulo.diff_main(edicaoAnterior.RetornaTituloEmTextoPuro(), edicaoAtual.RetornaTituloEmTextoPuro(), true);
                xdiff_Titulo.diff_cleanupSemantic(diffs_Titulo);
                string html_Titulo = xdiff_Titulo.diff_prettyHtml(diffs_Titulo);
                edicaoAtual.Titulo = html_Titulo;

                DiffMatchPatch.diff_match_patch xdiff_Conteudo = new diff_match_patch();
                List<DiffMatchPatch.Diff> diffs_Conteudo = xdiff_Conteudo.diff_main(edicaoAnterior.RetornaConteudoEmTextoPuro(), edicaoAtual.RetornaConteudoEmTextoPuro(), true);
                xdiff_Conteudo.diff_cleanupSemantic(diffs_Conteudo);
                string html_Conteudo = xdiff_Conteudo.diff_prettyHtml(diffs_Conteudo);
                //html_Conteudo = html_Conteudo.Replace("<del style=\"background:#ffe6e6;\">", "<del style='background:#228b22;'>"); //.Replace("<ins style=\"background:#e6ffe6;\">", "<ins style='background:#A9F5E1;'>");
                edicaoAtual.Conteudo = html_Conteudo.Replace("&para;", "");
                //edicao1.ConteudoTextoPuroParaHtml();

                return edicaoAtual;

            }
            catch (Exception ex)
            {
                this.Notificar($"{ex.Message}; {ex.InnerException.Message}");
                throw ex;
            }
        }

        //private bool comparaConteudoEdicaoEmTextoPuroComFiltro(EdicaoArtigo edicao, string filtro)
        //{
        //    DiffMatchPatch.diff_match_patch xdiff_Titulo = new diff_match_patch();
        //    List<DiffMatchPatch.Diff> diffs_Titulo = xdiff_Titulo.diff_main(edicao.RetornaTituloEmTextoPuro(), filtro, true);
        //    xdiff_Titulo.diff_cleanupSemantic(diffs_Titulo);
        //    string titulo = xdiff_Titulo.diff_text2(diffs_Titulo);
        //    string titulo2 = xdiff_Titulo.diff_text1(diffs_Titulo);
        //    var teste = xdiff_Titulo.


        //    DiffMatchPatch.diff_match_patch xdiff_Conteudo = new diff_match_patch();
        //    List<DiffMatchPatch.Diff> diffs_Conteudo = xdiff_Conteudo.diff_main(edicao.RetornaConteudoEmTextoPuro(), filtro, true);
        //    xdiff_Conteudo.diff_cleanupSemantic(diffs_Conteudo);
        //    string conteudo = xdiff_Conteudo.diff_text2(diffs_Conteudo);
        //    string conteudo2 = xdiff_Conteudo.diff_text1(diffs_Conteudo);

        //    if (titulo.Length == 0 || conteudo.Length == 0)
        //    {
        //        var edicaoAchada = edicao;
        //    }

        //    return false;
        //}


        public async Task<EdicaoArtigo> SalvarEdicao(Tema tema, Artigo artigo, EdicaoArtigo edicao, Guid usuarioId, string usuarioEmail)
        {
            try
            {
                Artigo artigoDomain = await ArtigoRepository.RetornaArtigoPorId(artigo.Id);
                //artigoDomain.Titulo = artigo.Titulo;
                EdicaoArtigo edicaoDomain = new EdicaoArtigo(edicao, usuarioId, usuarioEmail);
                artigoDomain.AdicionarEdicaoNaLista(edicaoDomain);
                edicaoDomain.IncluirArtigo(artigoDomain);
                //await ArtigoRepository.Atualizar(artigoDomain);
                await EdicaoArtigoRepository.Salvar(edicaoDomain);
                //return edicao;

                //Edicao edicaoDomain = await EdicaoArtigoRepository.RetornarEdicaoPorId(edicao.Id);
                //edicaoDomain.AtualizarConteudo(usuarioId, usuarioEmail, edicao.Conteudo);
                //edicaoDomain.Artigo.Titulo = artigo.Titulo;
                //await EdicaoArtigoRepository.Atualizar(edicaoDomain);
                return edicaoDomain;
            }
            catch (Exception ex)
            {
                this.Notificar($"{ex.Message}; {ex.InnerException.Message}");
                throw ex;
            }
        }


        public async Task<EdicaoArtigo> RetornaArtigoEdicaoPorId(int edicaoId)
        {
            EdicaoArtigo edicao = await EdicaoArtigoRepository.RetornarEdicaoPorId(edicaoId);
            return edicao;
        }

        public async Task<EdicaoArtigo> RetornaArtigoEdicaoPorArtigoId(int artigoId)
        {
            EdicaoArtigo edicao = await EdicaoArtigoRepository.RetornaEdicaoArtigoTemaPorArtigoId(artigoId);
            return edicao;
        }


        public async Task<EdicaoArtigo> SalvarInclusaoDados(EdicaoTema edicaoTema, EdicaoArtigo edicaoArtigo, Guid usuarioId, string usuarioEmail)
        {
            try
            {
                EdicaoArtigo edicaoArtigoDomain;
                Artigo artigoDomain;
                EdicaoTema edicaoTemaDomain;
                Tema temaDomain;

                Tema temaHome = await TemaRepository.RetornaTemaHome();
                temaDomain = await TemaRepository.RetornaTemaPorTituloDaEdicao(edicaoTema);


                if (temaHome == null || temaDomain == null)
                {
                    if (temaDomain == null && temaHome != null)
                    {
                        if (temaHome.Edicoes.ToList().LastOrDefault().Titulo == edicaoTema.Titulo)
                        {
                            Exception ex = new Exception("O nome do título do tema já está sendo usado pela página inicial. Escolha outro título para o Tema.");
                            throw ex;
                        }
                    }

                    edicaoArtigoDomain = new EdicaoArtigo(edicaoArtigo, usuarioId, usuarioEmail);
                    artigoDomain = new Artigo();
                    edicaoArtigoDomain.IncluirArtigo(artigoDomain);
                    artigoDomain.AdicionarEdicaoNaLista(edicaoArtigoDomain);

                    temaDomain = new Tema();
                    edicaoTemaDomain = new EdicaoTema(edicaoTema, usuarioId, usuarioEmail);
                    temaDomain.AdicionarArtigoNaLista(artigoDomain);
                    temaDomain.AdicionarEdicaoNaLista(edicaoTemaDomain);
                    edicaoTemaDomain.IncluirTema(temaDomain);
                    artigoDomain.IncluirTema(temaDomain);

                    await TemaRepository.Salvar(temaDomain);

                    return edicaoArtigoDomain;
                }
                else
                {
                    edicaoArtigoDomain = new EdicaoArtigo(edicaoArtigo, usuarioId, usuarioEmail);
                    artigoDomain = new Artigo();
                    edicaoArtigoDomain.IncluirArtigo(artigoDomain);
                    artigoDomain.AdicionarEdicaoNaLista(edicaoArtigoDomain);

                    artigoDomain.IncluirTema(temaDomain);
                    temaDomain.AdicionarArtigoNaLista(artigoDomain);

                    await TemaRepository.Atualizar(temaDomain);

                    return edicaoArtigoDomain;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public async Task<EdicaoArtigo> SalvarArtigoEdicao(Artigo artigo, EdicaoArtigo edicaoArtigo, Guid usuarioId, string usuarioEmail)
        {
            try
            {
                EdicaoArtigo edicaoDomain = new EdicaoArtigo(edicaoArtigo, usuarioId, usuarioEmail);
                Artigo artigoDomain = await ArtigoRepository.RetornaArtigoPorId(artigo.Id);
                artigoDomain.AdicionarEdicaoNaLista(edicaoDomain);
                edicaoDomain.IncluirArtigo(artigoDomain);
                await EdicaoArtigoRepository.Salvar(edicaoDomain);
                //await ArtigoRepository.Atualizar(artigoDomain);
                return edicaoDomain;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}