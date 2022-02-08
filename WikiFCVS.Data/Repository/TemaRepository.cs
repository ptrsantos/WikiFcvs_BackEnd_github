using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using WikiFCVS.Data.Context;
using WikiFCVS.Domain.Intefaces.Repository;
using WikiFCVS.Domain.Models;
using System.Linq;


namespace WikiFCVS.Data.Repository
{
    public class TemaRepository : RepositoryBase<Tema>, ITemaRepository
    {
        public TemaRepository(WikiFCVSContext context) : base(context) { }

        public async Task<ICollection<Tema>> ListarTemasArtigos()
        {
            var lista = await DbSet.Include(t => t.Artigos).ToArrayAsync();
            return lista;
        }

        public async Task<ICollection<Tema>> ListarTodosTemas()
        {
            try
            {
                //ICollection<Tema> lista = await DbSet
                //          .Include(t => t.Artigos)
                //          .Include("Artigos.Edicoes")
                //          .ToListAsync();

                ICollection<Tema> lista = await DbSet
                                  .Include(t => t.Artigos)
                                  .ThenInclude(a => a.Edicoes)
                                  .ThenInclude(e => e.EdicaoEfetuada)
                                  .Include(t => t.Edicoes)
                                  .ThenInclude(ed => ed.EdicaoEfetuada)
                                  .ToListAsync();

                return lista;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

        }

        public async Task<Tema> RetornaTemaHome()
        {
            try
            {
                            var tema = await DbSet
                             .Include(t => t.Edicoes)
                             .ThenInclude(e => e.EdicaoEfetuada)
                             .Include(t => t.Artigos)
                             //.Include("Artigos.Edicoes")
                             .ThenInclude(a => a.Edicoes)
                             .ThenInclude(e => e.EdicaoEfetuada)
                             //.Include("Edicao.EdicaoEfetuada")
                             .FirstOrDefaultAsync(t => t.Id == 1);
            return tema;
            }
            catch (System.Exception ex)
            {

                throw ex;
            }

        }

        public async Task<Tema> RetornaTemaPorId(int temaId)
        {
            Tema tema = await DbSet
                    .Include(t => t.Artigos)
                    .ThenInclude(a => a.Edicoes)
                    .ThenInclude(e => e.EdicaoEfetuada)
                    .Include(t => t.Edicoes)
                    .ThenInclude(ed => ed.EdicaoEfetuada)
                    .Where(t => t.Id == temaId)
                    .FirstOrDefaultAsync();

            return tema;
        }

        public async Task<Tema> RetornaTemaPorTituloDaEdicao(EdicaoTema edicaoTema)
        {
            try
            {

                Tema tema = await DbSet
                                    .Include(t => t.Artigos)
                                    .ThenInclude(a => a.Edicoes)
                                    .ThenInclude(e => e.EdicaoEfetuada)
                                    .Include(t => t.Edicoes)
                                    .ThenInclude(ed => ed.EdicaoEfetuada)
                                    .Where(t => t.Edicoes.Any(e =>e.Titulo == edicaoTema.Titulo))
                                    .FirstOrDefaultAsync();

                return tema;
            }
            catch (System.Exception ex)
            {

                throw ex;
            }
        }
    }
}