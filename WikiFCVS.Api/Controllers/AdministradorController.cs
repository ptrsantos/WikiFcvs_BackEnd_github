using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WikiFCVS.Api.Dto.Identity;
using WikiFCVS.Domain.Intefaces.Notificacoes;
using WikiFCVS.Identity.Extensions;
using WikiFCVS.Identity.Interfaces.Services;
using WikiFCVS.Identity.Interfaces.User;
using WikiFCVS.Identity.Models;

namespace WikiFCVS.Api.Controllers
{
    [Authorize]
    [ClaimsAuthorize("perfil", "Administrador")]
    [Route("api/Administrador")]
    public class AdministradorController : MainController
    {

        private readonly IMapper Mapper;
        private readonly IAspNetUserService AspNetUserService;

        public AdministradorController(IMapper mapper, INotificador notificador, IUser AppUser,
                                       IAspNetUserService aspNetUserService) : base(notificador, AppUser)
        {
            Mapper = mapper;
            AspNetUserService = aspNetUserService;
        }

        [HttpGet("ListarUsuarios")]
        public async Task<ActionResult> ListarUsuarios()
        {
            try
            {
                var listaUsuarios = await AspNetUserService.ListarUsuarios();
                var listaUsuairosDto = RetornaListaUsuarioIdentityDtoMapeado(listaUsuarios);
                return CustomResponse(listaUsuairosDto);
            }
            catch (System.Exception ex)
            {
                string mensagem = $"{ex.Message}";
                NotificarErro(mensagem);
                return CustomResponse();
            }
        }


        [HttpGet("bloqueio")]
        public ActionResult Bloqueio(string id)
        {
            try
            {
                UsuarioIdentity usuario  = AspNetUserService.Bloqueio(id);
                UsuarioIdentityDto usuarioDto = RetornaUsuarioIdentityDtoMapeado(usuario);
                return CustomResponse(usuarioDto);
            }
            catch (System.Exception ex)
            {
                string mensagem = $"{ex.Message}";
                NotificarErro(mensagem);
                return CustomResponse();
            }
        }

        [HttpGet("alterarPerfil")]
        public ActionResult AlterarPerfil(string id)
        {
            try
            {
                UsuarioIdentity usuario = AspNetUserService.EditarPerfil(id);
                UsuarioIdentityDto usuarioDto = RetornaUsuarioIdentityDtoMapeado(usuario);
                return CustomResponse(usuarioDto);
            }
            catch (System.Exception ex)
            {
                string mensagem = $"{ex.Message}";
                NotificarErro(mensagem);
                return CustomResponse();
            }
        }


        private ICollection<UsuarioIdentityDto> RetornaListaUsuarioIdentityDtoMapeado(ICollection<UsuarioIdentity> listaUsuarios)
        {
            return Mapper.Map<ICollection<UsuarioIdentity>, ICollection<UsuarioIdentityDto>>(listaUsuarios);
        }

        private UsuarioIdentityDto RetornaUsuarioIdentityDtoMapeado(UsuarioIdentity usuario)
        {
            return Mapper.Map<UsuarioIdentity, UsuarioIdentityDto>(usuario);
        }
    }
}
