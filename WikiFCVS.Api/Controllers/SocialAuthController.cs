using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WikiFCVS.Api.Dto;
using WikiFCVS.Identity.Extensions;
using WikiFCVS.Domain.Intefaces.Notificacoes;
using WikiFCVS.Domain.Intefaces.Services;
using WikiFCVS.Identity.Interfaces.User;

namespace WikiFCVS.Api.Controllers
{
    [Route("api/socialauth")]
    [ApiController]
    public class SocialAuthController : MainController
    {
        private readonly SignInManager<IdentityUser> SignInManager;
        private readonly UserManager<IdentityUser> UserManager;
        private readonly AppSettings AppSettings;
        private readonly IArtigoService ArtigoService;
        private readonly IEdicaoService EdicaoService;
        private readonly IMapper Mapper;

        public SocialAuthController(IOptions<AppSettings> appSettings,
                                  SignInManager<IdentityUser> signInManager,
                                  UserManager<IdentityUser> userManager,
                                  IArtigoService artigoService,
                                  IEdicaoService edicaoService,
                                  IMapper mapper,
                                  INotificador notificador, IUser appUser) : base(notificador, appUser)
        {
            SignInManager = signInManager;
            UserManager = userManager;
            AppSettings = appSettings.Value;
            Mapper = mapper;
            ArtigoService = artigoService;
            EdicaoService = edicaoService;
        }
        [Route("GoogleLogin")]
        [AllowAnonymous]
        public IActionResult GoogleLogin()
        {
            var properties = new AuthenticationProperties { RedirectUri = Url.Action("GoogleResponse") };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        [Route("GoogleResponse")]
        [AllowAnonymous]
        public async Task<IActionResult> GoogleResponse()
        {
            ExternalLoginInfo info = await SignInManager.GetExternalLoginInfoAsync();
            if (info == null)
                //return RedirectToAction(nameof(Login));
                return CustomResponse();

            var result = await SignInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false);
            string[] userInfo = { info.Principal.FindFirst(ClaimTypes.Name).Value, info.Principal.FindFirst(ClaimTypes.Email).Value };
            if (result.Succeeded)
                //return View(userInfo);
                return CustomResponse();
            else
            {
                IdentityUser user = new IdentityUser
                {
                    Email = info.Principal.FindFirst(ClaimTypes.Email).Value,
                    UserName = info.Principal.FindFirst(ClaimTypes.Email).Value
                };

                IdentityResult identResult = await UserManager.CreateAsync(user);
                if (identResult.Succeeded)
                {
                    identResult = await UserManager.AddLoginAsync(user, info);
                    if (identResult.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, false);
                        return CustomResponse(userInfo);
                    }
                }
                return CustomResponse();
            }
        }
    }
}