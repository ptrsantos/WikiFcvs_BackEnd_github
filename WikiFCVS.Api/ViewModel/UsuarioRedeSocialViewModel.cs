using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WikiFCVS.Api.Dto;

namespace WikiFCVS.Api.ViewModel
{
    public class UsuarioRedeSocialViewModel
    {
        public SocialUserDto Usuario { get; set; }
        public string token { get; set; }
    }
}
