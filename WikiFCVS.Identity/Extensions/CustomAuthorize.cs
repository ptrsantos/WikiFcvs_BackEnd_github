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
            //foreach(var claim in context.User.Claims)
            //{
            //    var type = claim.Type;
            //    var values = claim.Value.Split(',');
            //}
            return context.User.Identity.IsAuthenticated && context.User.Claims.Any(c => c.Type == claimName && c.Value.Split(',').Contains(claimValue));
        }

    }

    public class ClaimsAuthorizeAttribute : TypeFilterAttribute
    {
         
        public ClaimsAuthorizeAttribute(string claimName1, string claimValue1, string claimName2 = "", string claimValue2 = "") : base(typeof(RequisitoClaimFilter))
        {
            //Arguments = new object[] { new ClaimsAuthorizeAttribute(claimName, claimValue) };
            if (claimName2 == "")
            {
                Arguments = new object[] { new Claim(claimName1, claimValue1), new Claim(claimName2, claimValue2) };
            }
            else
            {
                Arguments = new object[] { new Claim(claimName1, claimValue1) };
            }
            //if (claimName2 == "")
            //{
            //    Arguments = new object[] { new Claim(claimName1, claimValue1) };
            //}
            //else
            //{
            //    Arguments = new object[] { new Claim(claimName1, claimValue1), new Claim(claimName2, claimValue2) };
            //}
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
