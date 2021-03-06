using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Security.Claims;

namespace WikiFCVS.Identity.Extensions
{
    public class CustomAuthorization
    {
        public static bool ValidarClaimsUsuario(HttpContext context, string claimName, string claimValue)
        {
            //return context.User.Identity.IsAuthenticated && context.User.Claims.Any(c => c.Type == claimName && c.Value.Split(',').Contains(claimValue));
            return context.User.Identity.IsAuthenticated && context.User.Claims.Any(c => c.Type == claimName && claimValue.Split(',').Contains(c.Value));
        }

    }

    public class ClaimsAuthorizeAttribute : TypeFilterAttribute
    {
        public ClaimsAuthorizeAttribute(string claimName, string claimValue) : base(typeof(RequisitoClaimFilter))
        {
            Arguments = new object[] { new Claim(claimName, claimValue) };
        }
    }

    public class RequisitoClaimFilter : IAuthorizationFilter
    {
        private readonly Claim Claim;

        public RequisitoClaimFilter(Claim claim)
        {
            Claim = claim;
        }

        public RequisitoClaimFilter()
        {

        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                context.Result = new StatusCodeResult(401);
            }

            if(!CustomAuthorization.ValidarClaimsUsuario(context.HttpContext, Claim.Type, Claim.Value))
            {
                context.Result = new StatusCodeResult(403);
            }
        }
    }
}
