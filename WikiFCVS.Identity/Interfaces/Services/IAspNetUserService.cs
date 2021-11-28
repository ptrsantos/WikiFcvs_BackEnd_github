using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;
using WikiFCVS.Identity.Models;

namespace WikiFCVS.Identity.Interfaces.Services
{
    public interface IAspNetUserService
    {
        Task<ICollection<UsuarioIdentity>> ListarUsuarios();
        UsuarioIdentity Bloqueio(string usuarioId);
        UsuarioIdentity EditarPerfil(string userId, string userName);
        UsuarioIdentity EditarPerfil(string id);
        //void CadastrarPerfilUsuario(string email);
    }
}
