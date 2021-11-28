using System.Collections.Generic;
using System.Threading.Tasks;
using WikiFCVS.Domain.Models;

namespace WikiFCVS.Domain.Intefaces.Repository
{
    public interface IArtigoRepository : IRepositoryBase<Artigo>
    {
        Task<ICollection<Artigo>> ListarTodosArtigos();
        Task<Artigo> RetornaArtigoPorId(int id);
        Task<ICollection<Artigo>> ListarArtigos();
        Task<ICollection<Artigo>> ListarArtigosIncluidosPorSemestre();
    }
}