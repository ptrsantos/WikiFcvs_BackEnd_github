using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WikiFCVS.Domain.Models;

namespace WikiFCVS.Domain.Intefaces.Services
{
    public interface IEstatisticaService : IDisposable
    {
        Task<ICollection<MesTotal>> ListarEstatisticaUsuarios();
        Task<ICollection<MesTotal>> ListarEstatisticaArtigos();
        Task<ICollection<MesTotal>> ListarEstatisticaEdicoes();
    }
}
