using System.Threading.Tasks;
using WikiFCVS.Domain.Models;

namespace WikiFCVS.Domain.Intefaces.Repository
{
    public interface IEdicaoTemaRepository : IRepositoryBase<EdicaoTema>
    {
        Task<EdicaoTema> RetornaEdicaoTemaPorTemaIdAsync(int temaId);
    }
}
