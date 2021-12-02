using WikiFCVS.Domain.Intefaces.Notificacoes;
using WikiFCVS.Domain.Services;
using WikiFCVS.Identity.Interfaces.Services;
using WikiFCVS.Identity.Data;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using WikiFCVS.Identity.Interfaces.User;
using WikiFCVS.Identity.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Claims;
using WikiFCVS.Domain.Notificacoes;

namespace WikiFCVS.Identity.Services
{
    public class AspNetUserService : BaseService, IAspNetUserService
    {
        private readonly ApplicationDbContext ApplicationDbContext;
        private IUser AspNetUser;
        private readonly UserManager<IdentityUser> UserManager;

        public AspNetUserService(IUser aspNetUser, ApplicationDbContext applicationDbContext,
                                UserManager<IdentityUser> userManager, INotificador notificador) : base(notificador)
        {
            ApplicationDbContext = applicationDbContext;
            AspNetUser = aspNetUser;
            UserManager = userManager;
        }

        public async Task<ICollection<UsuarioIdentity>> ListarUsuarios()
        {
            try
            {

                ICollection<UsuarioIdentity> listarUsuarios = new List<UsuarioIdentity>();
                //var usuarios = this.UserManager.Users.ToListAsync();
                var lista = await ApplicationDbContext.Users.ToListAsync();
                foreach (var user in lista)
                {
                    UsuarioIdentity usuarioIdentity = RetornaUsuarioIdentity(user);
                    listarUsuarios.Add(usuarioIdentity);
                }

                return listarUsuarios;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message, ex.InnerException.Message);
                throw ex;
            }

        }

        public UsuarioIdentity Bloqueio(string usuarioId)
        {
            try
            {
                var user = ApplicationDbContext.Users.FirstOrDefault(u => u.Id == usuarioId);
                if(user.LockoutEnd != null && user.LockoutEnd.Value.ToString().Substring(0, 10).Equals("09/09/9999"))
                {
                    return RetornaUsuarioIdentity(DesbloquearUsuario(user));
                }
                else
                {
                    return RetornaUsuarioIdentity(BloquearUsuario(user));
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IdentityUser BloquearUsuario(IdentityUser usuario)
        {
            try
            {
                usuario.LockoutEnabled = true;
                var dataPadraoBloqueio = Convert.ToDateTime("09/09/9999");
                usuario.LockoutEnd = dataPadraoBloqueio;
                ApplicationDbContext.Update(usuario);
                var retorno = ApplicationDbContext.SaveChanges();
                return usuario;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IdentityUser DesbloquearUsuario(IdentityUser usuario)
        {
            try
            {
                usuario.LockoutEnabled = true;
                usuario.LockoutEnd = null;
                ApplicationDbContext.Update(usuario);
                var retorno = ApplicationDbContext.SaveChanges();
                return usuario;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public UsuarioIdentity EditarPerfil(string id)
        {
            IdentityUser user  = ApplicationDbContext.Users.FirstOrDefault(u => u.Id == id);
            return EditarPerfil(user.Id, user.UserName);
        }

        public UsuarioIdentity EditarPerfil(string usuarioId, string usuarioNome)
        {
            try
            {
                IdentityUser identityUser = null;
                IdentityUserClaim<string> userClaim = ApplicationDbContext.UserClaims.FirstOrDefault(c => c.UserId == usuarioId);
                if (userClaim != null)
                {
                    if(userClaim.ClaimValue == null || userClaim.ClaimValue == "Usuario")
                    {
                        identityUser = DefinirComoAdministrador(userClaim, usuarioId, usuarioNome);
                    }
                    else
                    {
                        identityUser = DefinirComoUsuario(userClaim, usuarioId);
                    }
                }
                else
                {
                    identityUser = DefinirComoUsuario(userClaim, usuarioId);
                }

                return RetornaUsuarioIdentity(identityUser);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private IdentityUser DefinirComoAdministrador(IdentityUserClaim<string> userClaim, string userId, string userName)
        {
            try
            {
                if (userName.Contains("Facebook") || userName.Contains("Google"))
                {
                    throw new Exception("Usuários cadastrados pelo Google ou Facebook não podem ser administradores!");
                }

                if (userClaim == null)
                {
                    //Claim claim = new Claim("perfil", "Adminstrador");
                    IdentityUserClaim<string> userClaimDomain = new IdentityUserClaim<string>
                    {
                        UserId = userId,
                        ClaimType = "perfil",
                        ClaimValue = "Adminstrador"
                    };
                    ApplicationDbContext.Add(userClaimDomain);
                    ApplicationDbContext.SaveChanges();

                }
                else
                {
                    userClaim.ClaimType = "perfil";
                    userClaim.ClaimValue = "Administrador";
                    ApplicationDbContext.Update(userClaim);
                    ApplicationDbContext.SaveChanges();
                }

                return ApplicationDbContext.Users.FirstOrDefault(u => u.Id == userId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        private IdentityUser DefinirComoUsuario(IdentityUserClaim<string> userClaim, string userId)
        {
            if(userClaim != null)
            {
                //userClaim.ClaimType = null;
                //userClaim.ClaimValue = null;
                userClaim.ClaimType = "perfil";
                userClaim.ClaimValue = "Usuario";
                ApplicationDbContext.Update(userClaim);
                var retorno = ApplicationDbContext.SaveChanges();
            }
            else
            {
                IdentityUserClaim<string> userClaimDomain = new IdentityUserClaim<string>
                {
                    UserId = userId,
                    ClaimType = "perfil",
                    ClaimValue = "Usuario"
                };
                ApplicationDbContext.Add(userClaimDomain);
                ApplicationDbContext.SaveChanges();
            }

            return ApplicationDbContext.Users.FirstOrDefault(u => u.Id == userId);
        }

        private UsuarioIdentity RetornaUsuarioIdentity(IdentityUser identityUser)
        {
            UsuarioIdentity usuarioIdentity = new UsuarioIdentity(identityUser);
            IdentityUserClaim<string> userClaim = ApplicationDbContext.UserClaims.Where(c => c.UserId == identityUser.Id).FirstOrDefault();
            if (userClaim?.ClaimValue != null )
            {
                usuarioIdentity.InserirClaim(userClaim.ClaimType, userClaim.ClaimValue);
                //Claim claim = new Claim(userClaim.ClaimType, userClaim.ClaimValue);
            }
            return usuarioIdentity;
        }



        //public void CadastrarPerfilUsuario(string usuarioId)
        //{
        //    //try
        //    //{
        //    //    IdentityUser identityUser = null;
        //    //    IdentityUserClaim<string> userClaim = ApplicationDbContext.UserClaims.FirstOrDefault(c => c.UserId == usuarioId);
        //    //    identityUser = DefinirComoUsuario(userClaim, usuarioId);

        //    //}
        //    //catch (Exception ex)
        //    //{

        //    //    throw ex;
        //    //}
        //    throw new NotImplementedException();
        //}

    }
}
