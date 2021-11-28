using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WikiFCVS.Domain.Models;

namespace WikiFCVS.Domain.Intefaces.Services
{
    public interface ITemaService : IDisposable
    {
        Task<ICollection<Tema>> ListarTemasArtigos();
        Task<ICollection<Tema>> ListarTodosTemas();
        Task<Tema> RetornaTemaHome();
        Task<Tema> SalvarEdicaoTema(int id, string titulo, Guid usuarioId, string usuarioEmail);
        Task<Tema> SalvarEdicaoVinculoTema(Tema temaInicial, Tema temaSecundario);
        Task ExcluirTema(int temaId);
    }
}