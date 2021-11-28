using System.Threading.Tasks;
using System.Collections.Generic;
using WikiFCVS.Domain.Intefaces.Services;
using WikiFCVS.Domain.Intefaces.Repository;
using WikiFCVS.Domain.Intefaces.Notificacoes;
using WikiFCVS.Domain.Models;

namespace WikiFCVS.Domain.Services
{
    public class ArtigoService : BaseService, IArtigoService
    {
        private readonly IArtigoRepository ArtigoRepository;
        private readonly IEdicaoArtigoRepository EdicaoRepository;


        public ArtigoService(IArtigoRepository artigoRepository,
                              INotificador notificador,
                              IEdicaoArtigoRepository edicaoRepository) : base(notificador)
        {
            ArtigoRepository = artigoRepository;
            EdicaoRepository = edicaoRepository;
        }


        public async Task<EdicaoArtigo> RetornaArtigoHome()
        {
            EdicaoArtigo edicao = await EdicaoRepository.RetornaEdicaoArtigoTemaHome();
            return edicao;
        }

        public async Task<ICollection<Artigo>> ListarTemasArtigos()
        {
            ICollection<Artigo> artigos = await ArtigoRepository.ListarTodosArtigos();
            return artigos;
        }

        public void Dispose()
        {
            ArtigoRepository?.Dispose();
        }

        public async Task ExcluirArtigo(int artigoId)
        {
            try
            {
                Artigo artigoDomain = await ArtigoRepository.RetornaArtigoPorId(artigoId);
                Tema temaDomain = artigoDomain.Tema;
                temaDomain.Artigos.Remove(artigoDomain);
                artigoDomain.Tema = null;
                await ArtigoRepository.Remover(artigoDomain);
                //throw new System.NotImplementedException();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }

        }
    }
}