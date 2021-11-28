using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace WikiFCVS.Identity.Models
{
    public class UsuarioIdentity
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public Claim Claim { get; set; }
        public string DataBloqueio { get; set; }

        public UsuarioIdentity()
        {
        }


        public UsuarioIdentity(string id, string userName, string email, string dataBloqueio)
        {
            Id = new Guid(id);
            UserName = userName;
            Email = email;
            DataBloqueio = dataBloqueio;
        }

        public UsuarioIdentity(IdentityUser user)
        {
            Id = new Guid(user.Id);
            UserName = user.UserName;
            Email = user.Email;
            DataBloqueio = user.LockoutEnd.Equals(null) ? "" : user.LockoutEnd.ToString().Substring(0, 10);
        }

        public void InserirClaim(string tipo, string valor)
        {
            Claim = new Claim(tipo, valor);
        }

        public bool UsuarioBloqueado() {
            return (DataBloqueio.Length > 0);
        }
    }
}
