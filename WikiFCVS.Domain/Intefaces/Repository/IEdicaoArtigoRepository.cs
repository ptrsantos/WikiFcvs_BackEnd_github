using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WikiFCVS.Domain.Models;

namespace WikiFCVS.Domain.Intefaces.Repository
{
    public interface IEdicaoArtigoRepository : IRepositoryBase<EdicaoArtigo>
    {
        Task<EdicaoArtigo> RetornaEdicaoArtigoTemaHome();
        Task<EdicaoArtigo> RetornaEdicaoArtigoTemaPorArtigoId(int id);
        Task<EdicaoArtigo> RetornaEdicaoArtigoTemaPorTemaId(int id);
        Task<ICollection<EdicaoArtigo>> ListarTemasArtigos();
        Task<ICollection<EdicaoArtigo>> ListarArtigosEdicoesHistoricoPorPaginacao(int pagina, int tamanho);
        Task<int> RetornaQuantidadeArtigoEdicoes();
        Task<EdicaoArtigo> RetornaArtigoEdicao(int edicaoId);
        Task<EdicaoArtigo> RetornaArtigoEdicaoAnterior(EdicaoArtigo edicao);
        Task<EdicaoArtigo> RetornarEdicaoPorId(int id);
        Task<EdicaoArtigo> RetornaEdicaoPorNomeDoTema(string titulo);
        Task<EdicaoArtigo> RetornarEdicaoPorArtigoId(int id);
        Task<ICollection<EdicaoArtigo>> ListarTemasArtigosPorPaginacaoComFiltro(int pagina, int tamanho, string filtro);
        IQueryable<EdicaoArtigo> ListarTodasEdicoesArtigosTemas();
        Task<IEnumerable<EdicaoArtigo>> ListarIQueryableArtigosPorPaginacaoComFiltro(int pagina, int tamanho, string filtro);
        Task<ICollection<EdicaoArtigo>> ListarEdicoesIncluidasPorSemestre();
    }
}