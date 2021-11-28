using System.Collections.Generic;
using System.Threading.Tasks;
using WikiFCVS.Domain.Models;

namespace WikiFCVS.Domain.Intefaces.Repository
{
    public interface ITemaRepository : IRepositoryBase<Tema>
    {
        Task<ICollection<Tema>> ListarTemasArtigos();
        Task<ICollection<Tema>> ListarTodosTemas();
        Task<Tema> RetornaTemaHome();
        Task<Tema> RetornaTemaPorTituloDaEdicao(EdicaoTema edicaoTema);
        Task<Tema> RetornaTemaPorId(int temaId);
    }
}