using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WikiFCVS.Domain.Intefaces.Notificacoes;
using WikiFCVS.Domain.Intefaces.Repository;
using WikiFCVS.Domain.Intefaces.Services;
using WikiFCVS.Domain.Models;
using System.Linq;

namespace WikiFCVS.Domain.Services
{
    public class TemaService : BaseService, ITemaService
    {
        private readonly ITemaRepository TemaRepository;
        private readonly IArtigoRepository ArtigoRepository;
        private readonly IEdicaoArtigoRepository EdicaoRepository;
        private readonly IEdicaoTemaRepository EdicaoTemaRepository;

        public TemaService(ITemaRepository temaRepository,
                            IEdicaoTemaRepository edicaoTemaRepository,
                            IArtigoRepository artigoRepository,
                            IEdicaoArtigoRepository edicaoRepository,
                            INotificador notificador
                            ) : base(notificador)
        {
            TemaRepository = temaRepository;
            EdicaoTemaRepository = edicaoTemaRepository;
            ArtigoRepository = artigoRepository;
            EdicaoRepository = edicaoRepository;
        }



        public async Task<ICollection<Tema>> ListarTemasArtigos()
        {
            return await TemaRepository.ListarTemasArtigos();
        }

        public async Task<ICollection<Tema>> ListarTodosTemas()
        {
            return await TemaRepository.ListarTodosTemas();
        }

        public void Dispose()
        {
            TemaRepository?.Dispose();
            TemaRepository?.Dispose();
        }

        public async Task<Tema> RetornaTemaHome()
        {
            try
            {
                Tema temaHome = await TemaRepository.RetornaTemaHome();
                return temaHome;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Tema> SalvarEdicaoTema(int temaId, string temaTitulo, Guid usuarioId, string usuarioEmail)
        {
            try
            {
                Tema temaDomain = await TemaRepository.RetornaTemaPorId(temaId);
                EdicaoTema edicaoDomain = new EdicaoTema(temaTitulo, usuarioId, usuarioEmail);
                temaDomain.AdicionarEdicaoNaLista(edicaoDomain);
                edicaoDomain.IncluirTema(temaDomain);
                await TemaRepository.Atualizar(temaDomain);
                return temaDomain;
            }
            catch (Exception ex)
            {
                this.Notificar($"{ex.Message}; {ex.InnerException.Message}");
                return null;
            }

        }

        public async Task<Tema> SalvarEdicaoVinculoTema(Tema temaInicial, Tema temaSecundario)
        {
            try
            {
                Tema temaInicialDomain = await TemaRepository.RetornaTemaPorId(temaInicial.Id);
                Tema temaSecundarioDomain = await TemaRepository.RetornaTemaPorId(temaSecundario.Id);
                AtualizarListaArtigos(temaInicial, temaInicialDomain, temaSecundario, temaSecundarioDomain);
                await TemaRepository.Atualizar(temaInicialDomain);
                await TemaRepository.Atualizar(temaSecundarioDomain);
                return temaInicialDomain;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            throw new NotImplementedException();
        }

        private void AtualizarListaArtigos(Tema temaInicialFrontEnd, Tema temaIncialDomain, Tema temaSecundarioFrontEnd, Tema temaSecundarioDomain)
        {
            List<Artigo> listaArtigosTotal = new List<Artigo>();
            temaIncialDomain.Artigos.ToList().ForEach(artigo => listaArtigosTotal.Add(artigo));
            temaSecundarioDomain.Artigos.ToList().ForEach(artigo => listaArtigosTotal.Add(artigo));
            listaArtigosTotal.Concat(temaSecundarioDomain.Artigos);
            //List<Artigo> listaArtigosTotal = temaIncialDomain.Artigos.Concat(temaSecundarioDomain.Artigos).ToList();
            ICollection<Artigo> listaArtigosTemaInicial = listaArtigosTotal.Where(artDom => temaInicialFrontEnd.Artigos.Any(art => art.Id == artDom.Id)).ToList();
            ICollection<Artigo> listaArtigosTemaSecundario = listaArtigosTotal.Where(artDom => temaSecundarioFrontEnd.Artigos.Any(art => art.Id == artDom.Id)).ToList();

            VincularObjetosNaLista(temaIncialDomain, listaArtigosTemaInicial);
            VincularObjetosNaLista(temaSecundarioDomain, listaArtigosTemaSecundario);

        }

        private void VincularObjetosNaLista(Tema temaDomain, ICollection<Artigo> listaArtigosDomain)
        {
            temaDomain.Artigos.Clear() ;
            //temaDomain.Artigos = new List<Artigo>();
            if(listaArtigosDomain.Count > 0)
            {
                foreach (Artigo artigo in listaArtigosDomain)
                {
                    temaDomain.AdicionarArtigoNaLista(artigo);
                    //artigo.Tema = null;
                    artigo.IncluirTema(temaDomain);
                }

            }

        }

        public async Task ExcluirTema(int temaId)
        {
            try
            {
                //ExcluirEdicaoTema(temaId);
                Tema temaDomiain = await TemaRepository.RetornaTemaPorId(temaId);
                await TemaRepository.Remover(temaDomiain);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //throw new NotImplementedException();
        }

        //public async Task ExcluirEdicaoTema(int temaId)
        //{
        //    try
        //    {
        //        EdicaoTema edicaoTemaDomiain = await EdicaoTemaRepository.RetornaEdicaoTemaPorTemaIdAsync(temaId);
        //        await EdicaoTemaRepository.Remover(edicaoTemaDomiain);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    //throw new NotImplementedException();
        //}
    }
}