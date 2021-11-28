using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WikiFCVS.Domain.Models;

namespace WikiFCVS.Domain.Intefaces.Services
{
    public interface IArtigoService : IDisposable
    {
        //Task<ICollection<Artigo>> ListarTodosArtigos();
        Task<EdicaoArtigo> RetornaArtigoHome();
        Task<ICollection<Artigo>> ListarTemasArtigos();
        Task ExcluirArtigo(int temaId);
    }
}
