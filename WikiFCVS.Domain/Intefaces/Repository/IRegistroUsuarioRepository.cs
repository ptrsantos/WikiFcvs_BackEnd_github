using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WikiFCVS.Domain.Models;

namespace WikiFCVS.Domain.Intefaces.Repository
{
    public interface IRegistroUsuarioRepository : IRepositoryBase<RegistroUsuario>
    {
        Task<RegistroUsuario> RetornaUsuarioPorUsuarioId(string id);
        Task<ICollection<RegistroUsuario>> ListarRegistrosUsuariosPorSemestre();
    }
}
