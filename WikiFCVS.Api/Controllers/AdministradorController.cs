using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
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
    [ClaimsAuthorize("perfil","Administrador")]
    [Route("api/Administrador")]
    public class AdministradorController : MainController
    {

        private readonly IMapper Mapper;
        private readonly IAspNetUserService AspNetUserService;
        private readonly IUser AppUser;

        public AdministradorController(IMapper mapper, INotificador notificador, IUser appUser,
                                       IAspNetUserService aspNetUserService) : base(notificador, appUser)
        {
            Mapper = mapper;
            AspNetUserService = aspNetUserService;
            AppUser = appUser;
        }

        [HttpGet("ListarUsuarios")]
        public async Task<ActionResult> ListarUsuarios()
        {
            try
            {
                var listaUsuarios = await AspNetUserService.ListarUsuarios(AppUser);
                var listaUsuairosDto = RetornaListaUsuarioIdentityDtoMapeado(listaUsuarios);
                return CustomResponse(listaUsuairosDto);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Execption: ", ex.Message);
                return CustomResponse(ModelState);
            }
        }


        [HttpGet("bloqueio")]
        public ActionResult Bloqueio(string id)
        {
            try
            {
                UsuarioIdentity usuario = AspNetUserService.Bloqueio(id);
                UsuarioIdentityDto usuarioDto = RetornaUsuarioIdentityDtoMapeado(usuario);
                return CustomResponse(usuarioDto);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Execption: ", ex.Message);
                return CustomResponse(ModelState);
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
            catch (Exception ex)
            {
                ModelState.AddModelError("Execption: ", ex.Message);
                return CustomResponse(ModelState);
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
