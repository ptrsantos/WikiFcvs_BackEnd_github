using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using WikiFCVS.Identity.Interfaces.User;

namespace WikiFCVS.Identity.Extensions
{
    public class AspNetUser : IUser
    {
        private readonly IHttpContextAccessor Accessor;

        public AspNetUser(IHttpContextAccessor accessor)
        {
            Accessor = accessor;
        }

        public string Name => Accessor.HttpContext.User.Identity.Name;

        public Guid GetUserId()
        {
            return IsAuthenticated() ? Guid.Parse(Accessor.HttpContext.User.GetUserId()) : Guid.NewGuid();
        }

        public string GetUserEmail()
        {
            return IsAuthenticated() ? Accessor.HttpContext.User.GetUserEmail() : "";
        }

        public string GetUserName()
        {
            return IsAuthenticated() ? Accessor.HttpContext.User.Identity.Name : "";
        }

        public bool IsAuthenticated()
        {
            return Accessor.HttpContext.User.Identity.IsAuthenticated;
        }

        public bool IsInRole(string role)
        {
            return Accessor.HttpContext.User.IsInRole(role);
        }

        public IEnumerable<Claim> GetClaimsIdentity()
        {
            return Accessor.HttpContext.User.Claims;
        }

        public void GetUserById(string id) 
        {
            var retorno = Accessor.HttpContext.User.Claims;
        }
    }

    public static class ClaimsPrincipalExtensions
    {
        public static string GetUserId(this ClaimsPrincipal principal)
        {
            if(principal == null)
            {
                throw new ArgumentException(nameof(principal));
            }

            var claim = principal.FindFirst(ClaimTypes.NameIdentifier);
            return claim?.Value;
        }

        public static string GetUserEmail(this ClaimsPrincipal principal)
        {
            if(principal == null)
            {
                throw new ArgumentException(nameof(principal));
            }

            var claim = principal.FindFirst(ClaimTypes.Email);
            return claim?.Value;
        }
    }
}
