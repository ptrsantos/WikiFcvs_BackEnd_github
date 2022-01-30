using WikiFCVS.Domain.Models;
using WikiFCVS.Domain.Intefaces.Repository;
using WikiFCVS.Data.Context;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace WikiFCVS.Data.Repository
{
    public class EdicaoTemaRepository : RepositoryBase<EdicaoTema>, IEdicaoTemaRepository
    {
        public EdicaoTemaRepository(WikiFCVSContext context) : base(context) { }

        public async Task<EdicaoTema> RetornaEdicaoTemaPorTemaIdAsync(int temaId)
        {
            return await DbSet.FirstOrDefaultAsync(e => e.TemaId == temaId);
        }
    }
}
