using WikiFCVS.Domain.Models;
using WikiFCVS.Domain.Intefaces.Repository;
using WikiFCVS.Data.Context;

namespace WikiFCVS.Data.Repository
{
    public class EdicaoTemaRepository : RepositoryBase<EdicaoTema>, IEdicaoTemaRepository
    {
        public EdicaoTemaRepository(WikiFCVSContext context) : base(context) { }
    }
}
