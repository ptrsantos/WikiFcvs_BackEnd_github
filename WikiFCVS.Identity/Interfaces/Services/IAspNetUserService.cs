//using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;
using WikiFCVS.Identity.Interfaces.User;
using WikiFCVS.Identity.Models;

namespace WikiFCVS.Identity.Interfaces.Services
{
    public interface IAspNetUserService
    {
        Task<ICollection<UsuarioIdentity>> ListarUsuarios(IUser appUser);
        UsuarioIdentity Bloqueio(string usuarioId);
        UsuarioIdentity EditarPerfil(string userId, string userName);
        UsuarioIdentity EditarPerfil(string id);
        //void CadastrarPerfilUsuario(string email);
    }
}
