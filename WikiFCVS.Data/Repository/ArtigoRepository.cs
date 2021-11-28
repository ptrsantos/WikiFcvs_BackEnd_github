using System.Collections.Generic;
using System.Threading.Tasks;
using WikiFCVS.Data.Context;
using WikiFCVS.Domain.Intefaces.Repository;
using WikiFCVS.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;

namespace WikiFCVS.Data.Repository
{
    public class ArtigoRepository : RepositoryBase<Artigo>, IArtigoRepository
    {
        public ArtigoRepository(WikiFCVSContext context) : base(context)
        {
        }

        public async Task<ICollection<Artigo>> ListarArtigos()
        {
            ICollection<Artigo> artigos = await DbSet
                                .Include(a => a.Tema)
                                //.Include(a => a.Tema.EdicaoEfetuada)
                                //.Include(a => a.EdicaoEfetuada)
                                .Include(a => a.Edicoes)
                                .ThenInclude(a => a.Select(e => e.EdicaoEfetuada))
                                .ToListAsync();

            return artigos;
        }


        public async Task<ICollection<Artigo>> ListarTodosArtigos()
        {
            try
            {
                var lista = await DbSet
                                    //.Include(a => a.EdicaoEfetuada)
                                    .Include(a => a.Tema)
                                    //.Include(a => a.Tema.EdicaoEfetuada)
                                    .ToListAsync();
                return lista;
            }
            catch (System.Exception ex)
            {

                throw ex;
            }

        }

        public async  Task<Artigo> RetornaArtigoPorId(int id)
        {
            Artigo artigo = await DbSet
                                    .Include(a => a.Edicoes)
                                    .ThenInclude(e => e.EdicaoEfetuada)
                                    .Include(a => a.Tema)
                                    .Include(a => a.Tema.Edicoes)
                                    .ThenInclude(e => e.EdicaoEfetuada)
                                    .Where(a => a.Id == id).FirstOrDefaultAsync();

            return artigo;
        }

        public async Task<ICollection<Artigo>> ListarArtigosIncluidosPorSemestre()
        {
            try
            {

                int mes = DateTime.Now.Month;
                int ano = DateTime.Now.Year;
                //DateTime dataAtual = new DateTime(ano, mes, 1);
                DateTime dataAtual = DateTime.Now;
                DateTime dataAnterior = dataAtual.AddMonths(-6);

                ICollection<Artigo> artigos = await DbSet
                                                    .Distinct()
                                                    .Include(a => a.Edicoes)
                                                    .ThenInclude(e => e.EdicaoEfetuada)
                                                    .Where(a => a.Edicoes.OrderByDescending(e => e.EdicaoEfetuada.EditadoEm).Any(e => 
                                                            e.EdicaoEfetuada.EditadoEm <= dataAtual || e.EdicaoEfetuada.EditadoEm >= dataAnterior )
                                                           )
                                                    .ToListAsync();

                return artigos;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}