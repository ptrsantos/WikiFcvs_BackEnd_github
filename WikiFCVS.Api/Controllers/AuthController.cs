using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DevIO.Api.ViewModel;
using Facebook;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WikiFCVS.Api.Dto;
using WikiFCVS.Api.ViewModel;
using WikiFCVS.Domain.Intefaces.Notificacoes;
using WikiFCVS.Domain.Intefaces.Services;
using WikiFCVS.Domain.Models;
using WikiFCVS.Identity.Extensions;
using WikiFCVS.Identity.Interfaces.Services;
using WikiFCVS.Identity.Interfaces.User;
using static Google.Apis.Auth.GoogleJsonWebSignature;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace WikiFCVS.Api.Controllers
{
    [Route("api/auth")]
    public class AuthController : MainController
    {
        private readonly SignInManager<IdentityUser> SignInManager;
        private readonly UserManager<IdentityUser> UserManager;
        private readonly AppSettings AppSettings;
        private readonly IArtigoService ArtigoService;
        private readonly IEdicaoService EdicaoService;
        private readonly ITemaService TemaService;
        private readonly IMapper Mapper;

        private readonly string IdGoogle;
        private readonly string IdFacebook;
        private readonly string SecretFacebook;

        private readonly IAspNetUserService AspNetUserService;
        private readonly IRegistroUsuarioService RegistroUsuarioService;

        private readonly RoleManager<IdentityRole> RoleManager;

        public AuthController(IOptions<AppSettings> appSettings,
                              SignInManager<IdentityUser> signInManager,
                              UserManager<IdentityUser> userManager,
                              IArtigoService artigoService,
                              IEdicaoService edicaoService,
                              ITemaService temaService,
                              IMapper mapper, IConfiguration configuration,
                              INotificador notificador, IUser appUser,
                              RoleManager<IdentityRole> roleManager,
                              IAspNetUserService aspNetUserService,
                              IRegistroUsuarioService registroUsuarioService
                              ) : base(notificador, appUser)
        {

            SignInManager = signInManager;
            UserManager = userManager;
            AppSettings = appSettings.Value;
            Mapper = mapper;
            ArtigoService = artigoService;
            EdicaoService = edicaoService;
            TemaService = temaService;

            IdGoogle = configuration.GetValue<string>("Parametros:IdGoogle");
            IdFacebook = configuration.GetValue<string>("Parametros:IdFacebook");
            SecretFacebook = configuration.GetValue<string>("Parametros:SecretFacebook");

            RoleManager = roleManager;
            AspNetUserService = aspNetUserService;
            RegistroUsuarioService = registroUsuarioService;
        }


        [HttpPost("sigin-facebook")]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> SigInFacebook(SocialUserDto socialUser)
        {

            try
            {
                InicializacaoHomeViewModel model = new InicializacaoHomeViewModel();

                bool retorno = VerifyFacebookToken(socialUser);
                if (retorno == false)
                {
                    NotificarErro("Invalid External Authentication.");
                    return CustomResponse();
                }

                var info = new UserLoginInfo(socialUser.Provider, socialUser.Id, socialUser.Provider);
                IdentityUser user = await UserManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
                user = (user == null) ? null : user.UserName.Contains("Facebook") ? user : null;

                if (user == null)
                {
                    user = await UserManager.FindByNameAsync($"{socialUser.Email}-Facebook");
                    user = (user == null) ? null : user.UserName.Contains("Facebook") ? user : null;

                    if (user == null)
                    {
                        user = new IdentityUser
                        {
                            Email = socialUser.Email,
                            UserName = $"{socialUser.Email}-Facebook",
                            EmailConfirmed = true
                        };

                        var result = await UserManager.CreateAsync(user);
                        if (result.Succeeded)
                        {

                            IdentityRole identityRole = await ConfirmIdentityRoleViewer();
                            await UserManager.AddToRoleAsync(user, "Viewer");
                            await UserManager.AddLoginAsync(user, info);
                            var usuario = AspNetUserService.EditarPerfil(user.Id, user.UserName);
                            await RegistroUsuarioService.SalvarRegistroDaInclusaoDoUsuario(user.Id);
                        }

                    }
                    else
                    {
                        await UserManager.AddLoginAsync(user, info);

                    }
                }

                if (user == null)
                {
                    NotificarErro("Invalid External Authentication.");
                    return CustomResponse();
                }



                model.LoginResponsavel = await GerarJwt(user.Email, user.UserName);

                Tema TemaHome = await TemaService.RetornaTemaHome();
                ArtigoEdicaoDto artigoEdicaoDto = RetornaArtigoEdicaoDtoMapeadoDoTema(TemaHome);
                model.ArtigoEdicao = artigoEdicaoDto;

                ICollection<Tema> Temas = await TemaService.ListarTodosTemas();
                ICollection<TemaDto> TemasDtos = RetornaTemasDtoMapeado(Temas);
                model.Temas = TemasDtos;

                return CustomResponse(model);


            }
            catch (Exception ex)
            {
                NotificarErro($"{ex.Message}");
                return CustomResponse();
            }

        }



        public bool VerifyFacebookToken(SocialUserDto socialUser)
        {
            try
            {
                var fb = new FacebookClient(socialUser.AuthToken);
                var retorno = fb.Get("/me");

                fb.AppId = IdFacebook;
                fb.AppSecret = SecretFacebook;

                dynamic result = fb.Get("/me");

                if (result.id == socialUser.Id && result.name == socialUser.Name)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                NotificarErro($"{ex.Message}");
                return false;
            }
        }


        [HttpPost("sigin-google")]
        public async Task<ActionResult> SigInGoogle(SocialUserDto socialUser)
        {

            try
            {
                var payload = await VerifyGoogleToken(socialUser);
                if (payload == null)
                {
                    NotificarErro("Invalid External Authentication.");
                    return CustomResponse();
                }

                var model = new InicializacaoHomeViewModel();
                var info = new UserLoginInfo(socialUser.Provider, payload.Subject, socialUser.Provider);
                IdentityUser user = await UserManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
                user = (user == null) ? null : user.UserName.Contains("Google") ? user : null;
                if (user == null)
                {
                    user = await UserManager.FindByNameAsync($"{socialUser.Email}-Google");
                    user = (user == null) ? null : user.UserName.Contains("Google") ? user : null;
                    if (user == null)
                    {
                        user = new IdentityUser
                        {
                            Email = payload.Email,
                            UserName = $"{payload.Email}-Google",
                            EmailConfirmed = true
                        };
                        var result = await UserManager.CreateAsync(user);
                        if (result.Succeeded)
                        {
                            IdentityRole identityRole = await ConfirmIdentityRoleViewer();
                            await UserManager.AddToRoleAsync(user, "Viewer");
                            await UserManager.AddLoginAsync(user, info);
                            var usuario = AspNetUserService.EditarPerfil(user.Id, user.UserName);
                            await RegistroUsuarioService.SalvarRegistroDaInclusaoDoUsuario(user.Id);
                        }
       
                    }
                    else
                    {
                        await UserManager.AddLoginAsync(user, info);
                    }
                }

                if (user == null)
                {
                    NotificarErro("Invalid External Authentication.");
                    return CustomResponse();
                }

                Tema TemaHome = await TemaService.RetornaTemaHome();
                ArtigoEdicaoDto artigoEdicaoDto = RetornaArtigoEdicaoDtoMapeadoDoTema(TemaHome);
                model.ArtigoEdicao = artigoEdicaoDto;

                ICollection<Tema> Temas = await TemaService.ListarTodosTemas();
                ICollection<TemaDto> TemasDtos = RetornaTemasDtoMapeado(Temas);
                model.Temas = TemasDtos;

                model.LoginResponsavel = await GerarJwt(user.Email, user.UserName);

                return CustomResponse(model);

            }
            catch (Exception ex)
            {
                NotificarErro($"{ex.Message}");
                return CustomResponse();
            }

        }

        public async Task<Payload> VerifyGoogleToken(SocialUserDto socialUser)
        {
            try
            {
                var settings = new ValidationSettings()
                {
                    Audience = new[] { IdGoogle }
                };
                var payload = await GoogleJsonWebSignature.ValidateAsync(socialUser.IdToken, settings);
                return payload;
            }
            catch (Exception ex)
            {
                NotificarErro($"{ex.Message}, {ex.InnerException.Message}");
                return null;
            }
        }

        private async Task<IdentityRole> ConfirmIdentityRoleViewer()
        {
            IdentityRole identityRole;
            identityRole = await RoleManager.FindByNameAsync("Viewer");
            if (identityRole == null)
            {
                identityRole = new IdentityRole
                {
                    Name = "Viewer"
                };

                IdentityResult result = await RoleManager.CreateAsync(identityRole);
            }
            return identityRole;
        }

        [HttpPost("register")]
        public async Task<ActionResult> RegisterUser(RegisterUserViewModel registerUser)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            IdentityUser user = await UserManager.FindByEmailAsync(registerUser.Email);

            if(user != null && (user.UserName.Contains("Google") || user.UserName.Contains("Facebook")))
            {
                user = null;
            }
            

            if (user != null)
            {
                if (user.UserName == registerUser.Email)
                {
                    NotificarErro("Usuário já possui cadastro no sistema");
                    return CustomResponse(user);
                }
            }

            user = new IdentityUser
            {
                UserName = registerUser.Email,
                Email = registerUser.Email,
                EmailConfirmed = true
            };

            try
            {
                var result = await UserManager.CreateAsync(user, registerUser.Password);
                if (result.Succeeded)
                {

                    await SignInManager.SignInAsync(user, false);

                    var usuario = AspNetUserService.EditarPerfil(user.Id, user.UserName);

                    var model = new InicializacaoHomeViewModel();
                    model.LoginResponsavel = await GerarJwt(user.Email, user.UserName);

                    await RegistroUsuarioService.SalvarRegistroDaInclusaoDoUsuario(user.Id);

                    Tema TemaHome = await TemaService.RetornaTemaHome();
                    ArtigoEdicaoDto artigoEdicaoDto = RetornaArtigoEdicaoDtoMapeadoDoTema(TemaHome);
                    model.ArtigoEdicao = artigoEdicaoDto;

                    ICollection<Tema> Temas = await TemaService.ListarTodosTemas();
                    ICollection<TemaDto> TemasDtos = RetornaTemasDtoMapeado(Temas);
                    model.Temas = TemasDtos;

                    return CustomResponse(model);

                }

                foreach (var error in result.Errors)
                {
                    NotificarErro(error.Description);
                }

                return CustomResponse(registerUser);

            }
            catch (Exception ex)
            {
                NotificarErro($"{ex.Message}");
                throw ex;
            }

        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginUserViewModel loginUser)
        {

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            try
            {

                var result = await SignInManager.PasswordSignInAsync(loginUser.Email, loginUser.Password, false, true);

                if (result.Succeeded)
                {
                    var model = new InicializacaoHomeViewModel();
                    model.LoginResponsavel = await GerarJwt(loginUser.Email, "");

                    Tema TemaHome = await TemaService.RetornaTemaHome();
                    ArtigoEdicaoDto artigoEdicaoDto = RetornaArtigoEdicaoDtoMapeadoDoTema(TemaHome);
                    model.ArtigoEdicao = artigoEdicaoDto;

                    ICollection<Tema> Temas = await TemaService.ListarTodosTemas();
                    ICollection<TemaDto> TemasDtos = RetornaTemasDtoMapeado(Temas);
                    model.Temas = TemasDtos;

                    //ICollection<Artigo> artigos = await ArtigoService.ListarArtigosEdicoesHistorico();
                    //var TemasArtgiosDto = RetornaTemasArtigosDtoMapeado(artigos);
                    //model.TemasArtigos = TemasArtgiosDto;

                    return CustomResponse(model);
                }

                if (result.IsLockedOut)
                {
                    string dataBloqueio = UserManager.Users.FirstOrDefault(u => u.Email == loginUser.Email).LockoutEnd.ToString().Substring(0, 10);
                    if (dataBloqueio.Equals("09/09/9999"))
                    {
                        NotificarErro("Usuário bloqueado pelo adminsitrador");
                        return CustomResponse(loginUser);
                    }
                    else
                    {
                        NotificarErro("Usuário bloqueado temporariamente por tentativas inválidas");
                        return CustomResponse(loginUser);
                    }

                }

                NotificarErro("Usuário ou Senha incorretos");
                return CustomResponse(loginUser);
            }
            catch (Exception ex)
            {
                NotificarErro($"{ex.Message}, {ex.InnerException.Message}");
                throw ex;
            }


        }

        private ArtigoEdicaoDto RetornaArtigoEdicaoDtoMapeadoDoTema(Tema temaHome)
        {
            return Mapper.Map<Tema, ArtigoEdicaoDto>(temaHome);
        }

        private ICollection<TemaArtigoDto> RetornaTemasArtigosDtoMapeado(ICollection<Artigo> artigos)
        {
            return Mapper.Map<ICollection<Artigo>, ICollection<TemaArtigoDto>>(artigos);
        }

        private ArtigoEdicaoDto RetornaArtigoEdicaoDtoMapeado(EdicaoArtigo edicaoDomain)
        {
            return Mapper.Map<EdicaoArtigo, ArtigoEdicaoDto>(edicaoDomain);
        }

        private ICollection<TemaDto> RetornaTemasDtoMapeado(ICollection<Tema> temas)
        {
            return Mapper.Map<ICollection<Tema>, ICollection<TemaDto>>(temas);
        }

        private async Task<LoginResponsavelViewModel> GerarJwt(string email, string userName)
        {
            IdentityUser user = null;
            IdentityUser userPorNome = null;
            IdentityUser userPorEmail = await UserManager.FindByEmailAsync(email);
            if(userName.Length > 0)
            {
                userPorNome = await UserManager.FindByNameAsync(userName);
                if (!userPorEmail.Equals(userPorNome))
                {
                    user = userPorNome;
                }
                else
                {
                    user = userPorEmail;
                }
            }
            else
            {
                user = userPorEmail;
            }
            var claims = await UserManager.GetClaimsAsync(user);
            var userRoles = await UserManager.GetRolesAsync(user);

            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, ToUnixEpochDate(DateTime.UtcNow).ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.UtcNow).ToString(), ClaimValueTypes.Integer64));

            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim("role", userRole));
            }

            var identityClaims = new ClaimsIdentity();
            identityClaims.AddClaims(claims);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(AppSettings.Secret);

            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = AppSettings.Emissor,
                Audience = AppSettings.ValidoEm,
                Subject = identityClaims,
                Expires = DateTime.UtcNow.AddHours(AppSettings.ExpiracaoHoras),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            });

            var encodedToken = tokenHandler.WriteToken(token);

            var response = new LoginResponsavelViewModel
            {
                AccessToken = encodedToken,
                ExpiresIn = TimeSpan.FromHours(AppSettings.ExpiracaoHoras).TotalSeconds,
                UserToken = new UserTokenViewModel
                {
                    Id = user.Id,
                    Email = user.Email,
                    UserName = user.UserName,
                    Claims = claims.Select(c => new ClaimViewModel { Type = c.Type, Value = c.Value })
                }
            };

            return response;
        }



        private static long ToUnixEpochDate(DateTime date)
        {
            return (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);
        }

        //private static long ToUnixEpochDate(DateTime date)
        //   => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);

    }

    //private readonly IConfiguration _configuration;
    //private readonly IConfigurationSection _jwtSettings;
    //private readonly IConfigurationSection _goolgeSettings;
    //private readonly UserManager<IdentityUser> _userManager;
    //public JwtHandler(IConfiguration configuration, UserManager<IdentityUser> userManager)
    //{
    //    _userManager = userManager;
    //    _configuration = configuration;
    //    _jwtSettings = _configuration.GetSection("JwtSettings");
    //    _goolgeSettings = _configuration.GetSection("GoogleAuthSettings");
    //}
}