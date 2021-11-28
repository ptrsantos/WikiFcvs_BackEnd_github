using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WikiFCVS.Api.Dto.Identity
{
    public class UsuarioIdentityDto
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
        public bool Bloqueado { get; set; }
    }
}
