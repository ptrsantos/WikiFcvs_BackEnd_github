using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using WikiFCVS.Data.Context;
using WikiFCVS.Domain.Intefaces.Repository;
using WikiFCVS.Domain.Models;
using System.Linq;
using System;

namespace WikiFCVS.Data.Repository
{
    public class EdicaoArtigoRepository : RepositoryBase<EdicaoArtigo>, IEdicaoArtigoRepository
    {
        public EdicaoArtigoRepository(WikiFCVSContext context) : base(context) { }

        public Task<ICollection<EdicaoArtigo>> ListarTemasArtigos()
        {
            throw new System.NotImplementedException();
        }

        public async Task<ICollection<EdicaoArtigo>> ListarArtigosEdicoesHistoricoPorPaginacao(int pagina, int tamanho)
        {
            var lista = await DbSet
                    .Include(e => e.EdicaoEfetuada)
                    .Include(e => e.Artigo)
                    .Include(e => e.Artigo.Tema)
                    //.Include(e => e.Artigo.Tema.Edicoes)
                    //.ThenInclude(ed => ed.EdicaoEfetuada)
                    .OrderByDescending(e => e.EdicaoEfetuada.EditadoEm)
                    .Skip(tamanho * (pagina - 1)).Take(tamanho).OrderBy(e => e.EdicaoEfetuada.EditadoEm)
                    .ToListAsync();
            return lista.OrderByDescending(e => e.EdicaoEfetuada.EditadoEm).ToList();
        }

        public async Task<ICollection<EdicaoArtigo>> ListarTemasArtigosPorPaginacaoComFiltro(int pagina, int tamanho, string filtro)
        {
            try
            {
                 var lista = await DbSet
                        .Include(e => e.EdicaoEfetuada)
                        .Include(e => e.Artigo)
                        .Include(e => e.Artigo.Tema)
                        .Where(e => e.Titulo.ToLower().Contains(filtro.ToLower()) || Convert.ToString(e.Conteudo).ToLower().Contains(filtro.ToLower()))
                        .OrderByDescending(e => e.EdicaoEfetuada.EditadoEm)
                        .Skip(tamanho * (pagina - 1)).Take(tamanho).OrderBy(e => e.EdicaoEfetuada.EditadoEm)
                        .ToListAsync();
                return lista.OrderByDescending(e => e.EdicaoEfetuada.EditadoEm).ToList();

            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<EdicaoArtigo>> ListarIQueryableArtigosPorPaginacaoComFiltro(int pagina, int tamanho, string filtro)
        {
            try
            {
                IEnumerable<EdicaoArtigo> lista = await DbSet
                       .Include(e => e.EdicaoEfetuada)
                       .Include(e => e.Artigo)
                       .Include(e => e.Artigo.Tema).OrderBy(e => e.EdicaoEfetuada.EditadoEm).ToListAsync();
        
                //.Where(e => e.Titulo.ToLower().Contains(filtro.ToLower()) || Convert.ToString(e.Conteudo).ToLower().Contains(filtro.ToLower()))
                //.OrderByDescending(e => e.EdicaoEfetuada.EditadoEm)
                //.Skip(tamanho * (pagina - 1)).Take(tamanho).OrderBy(e => e.EdicaoEfetuada.EditadoEm)
                //.ToListAsync();
                return lista; ;

            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public IQueryable<EdicaoArtigo> ListarTodasEdicoesArtigosTemas()
        {
            IQueryable<EdicaoArtigo> query =
                                           from e in Db.EdicoesArtigos.AsQueryable()
                                           join a in Db.Artigos.AsQueryable() on e.Artigo.Id equals a.Id into t2g
                                           from a in t2g.DefaultIfEmpty() //DefaultIfEmpty is used for left joins
                                           //join t in Db.Temas.AsQueryable() on a.TemaId equals t.Id
                                           select new EdicaoArtigo
                                           {
                                               Id = e.Id,
                                               Artigo = e.Artigo,
                                               //ArtigoId = e.ArtigoId,
                                               Conteudo = e.Conteudo,
                                               EdicaoEfetuada = e.EdicaoEfetuada,
                                               //EdicaoEfetuadaId = e.EdicaoEfetuadaId,
                                               Titulo = e.Titulo
                                           };

            return query;
        }

        public async Task<ICollection<EdicaoArtigo>> ListarTodasEdicoes()
        {
            var lista = await DbSet
                    .Include(e => e.EdicaoEfetuada)
                    .Include(e => e.Artigo)
                    //.Include(e => e.Artigo.EdicaoEfetuada)
                    //.Include(a => a.Artigo.Tema.EdicaoEfetuada)
                    .Include(e => e.Artigo.Tema).ToListAsync();
            return lista;
        }

        public async Task<EdicaoArtigo> RetornaArtigoEdicao(int edicaoId)
        {
            EdicaoArtigo edicao = await DbSet
                    .Include(e => e.EdicaoEfetuada)
                    .Include(e => e.Artigo)
                    //.Include(e => e.Artigo.EdicaoEfetuada)
                    //.Include(a => a.Artigo.Tema.EdicaoEfetuada)
                    .Include(e => e.Artigo.Tema)
                    .Where(e => e.Id == edicaoId).LastOrDefaultAsync();
            return edicao;
        }

        public async Task<EdicaoArtigo> RetornaArtigoEdicaoAnterior(EdicaoArtigo edicaoAnterior)
        {
            EdicaoArtigo edicao = await DbSet
                .Include(e => e.EdicaoEfetuada)
                .Include(e => e.Artigo)
                //.Include(e => e.Artigo.EdicaoEfetuada)
                //.Include(a => a.Artigo.Tema.EdicaoEfetuada)
                .Include(e => e.Artigo.Tema)
                .Where(e => e.Id < edicaoAnterior.Id && e.Artigo.Id == edicaoAnterior.Artigo.Id).LastOrDefaultAsync();
            return edicao;
        }

        public async Task<EdicaoArtigo> RetornaEdicaoArtigoTemaHome()
        {
            try
            {
                var edicao =  await DbSet
                                    .Include(e => e.EdicaoEfetuada)
                                    .Include(e => e.Artigo)
                                    .Include(e => e.Artigo.Tema)
                                    .Include(e => e.Artigo.Tema.Edicoes)
                                    .ThenInclude(ed => ed.EdicaoEfetuada)
                                    .Where(e => e.Artigo.Tema.Id == 1).FirstOrDefaultAsync();
                //return edicao; /*Método não é mais usado*/
                return null;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public async Task<EdicaoArtigo> RetornaEdicaoArtigoTemaPorArtigoId(int id)
        {
            EdicaoArtigo edicao = await DbSet
            .Include(e => e.EdicaoEfetuada)
            .Include(e => e.Artigo)
            //.Include(e => e.Artigo.EdicaoEfetuada)
            //.Include(a => a.Artigo.Tema.EdicaoEfetuada)
            .ThenInclude(a => a.Tema)
            .Where(a => a.Artigo.Id == id)
            .LastOrDefaultAsync();
            return edicao;
        }

        public async Task<EdicaoArtigo> RetornaEdicaoArtigoTemaPorTemaId(int id)
        {
            EdicaoArtigo edicao = await DbSet
                    .Include(e => e.EdicaoEfetuada)
                    .Include(e => e.Artigo)
                    //.Include(e => e.Artigo.EdicaoEfetuada)
                    //.Include(a => a.Artigo.Tema.EdicaoEfetuada)
                    .ThenInclude(a => a.Tema)
                    .Where(a => a.Artigo.Tema.Id == id)
                    .LastOrDefaultAsync();
            return edicao;
        }

        public async Task<EdicaoArtigo> RetornaEdicaoPorNomeDoTema(string titulo)
        {
            EdicaoArtigo edicao = await DbSet
                        .Include(e => e.EdicaoEfetuada)
                        .Include(e => e.Artigo)
                        //.Include(e => e.Artigo.EdicaoEfetuada)
                        //.Include(a => a.Artigo.Tema.EdicaoEfetuada)
                        .ThenInclude(a => a.Tema)
                        //.Where(a => a.Artigo.Tema.Titulo == titulo)
                        .LastOrDefaultAsync();
            return edicao;
        }

        public async Task<int> RetornaQuantidadeArtigoEdicoes()
        {
            int total = await DbSet.CountAsync();
            return total;
        }

        public async Task<EdicaoArtigo> RetornarEdicaoPorId(int id)
        {
            EdicaoArtigo edicao = await DbSet
                    .Include(e => e.EdicaoEfetuada)
                    .Include(e => e.Artigo)
                    //.Include(e => e.Artigo.EdicaoEfetuada)
                    //.Include(a => a.Artigo.Tema.EdicaoEfetuada)
                    .ThenInclude(a => a.Tema)
                    .Where(e => e.Id == id)
                    .LastOrDefaultAsync();
            return edicao;
        }

        public async Task<EdicaoArtigo> RetornarEdicaoPorArtigoId(int id)
        {
            EdicaoArtigo edicao = await DbSet
                    .Include(e => e.EdicaoEfetuada)
                    .Include(e => e.Artigo)
                    //.Include(e => e.Artigo.EdicaoEfetuada)
                    //.Include(a => a.Artigo.Tema.EdicaoEfetuada)
                    .ThenInclude(a => a.Tema)
                    .Where(e => e.Artigo.Id == id)
                    .LastOrDefaultAsync();
            return edicao;
        }

        public async Task<ICollection<EdicaoArtigo>> ListarEdicoesIncluidasPorSemestre()
        {
            int mes = DateTime.Now.Month;
            int ano = DateTime.Now.Year;
            //DateTime dataAtual = new DateTime(ano, mes, 1);
            DateTime dataAtual = DateTime.Now;
            DateTime dataAnterior = dataAtual.AddMonths(-6);

            ICollection<EdicaoArtigo> edicoes = await DbSet
                                    .Include(e => e.Artigo)
                                    .Include(e => e.EdicaoEfetuada)
                                    .Where(e => e.EdicaoEfetuada.EditadoEm <= dataAtual || e.EdicaoEfetuada.EditadoEm >= dataAtual)
                                    .OrderByDescending(e => e.EdicaoEfetuada.EditadoEm)
                                    .ToListAsync();

            return edicoes;
        }
    }
}