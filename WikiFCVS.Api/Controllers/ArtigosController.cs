using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WikiFCVS.Api.Dto;
using WikiFCVS.Api.ViewModel;
using WikiFCVS.Domain.Intefaces.Notificacoes;
using WikiFCVS.Domain.Intefaces.Repository;
using WikiFCVS.Domain.Intefaces.Services;
using WikiFCVS.Domain.Models;
using WikiFCVS.Identity.Extensions;
using WikiFCVS.Identity.Interfaces.User;

namespace WikiFCVS.Api.Controllers
{
    [Route("api/artigos")]
    public class ArtigosController : MainController
    {
        private readonly IArtigoService ArtigoService;
        private readonly ITemaService TemaService;
        private readonly IUser AppUser;
        private readonly IMapper Mapper;

        public ArtigosController(IArtigoService artigoService, ITemaService temaService,
                                  IMapper mapper, INotificador notificador, IUser appUser) : base (notificador, appUser)
        {
            ArtigoService = artigoService;
            TemaService = temaService;
            Mapper = mapper;
            AppUser = appUser;
        }

        [ClaimsAuthorize("perfil", "Administrador")]
        [HttpDelete("ExcluirArtigo")]
        public async Task<ActionResult> ExcluirArtigo(int artigoId)
        {
            try
            {
                var teste = artigoId;
                TemaEdicaoViewModel model = new TemaEdicaoViewModel();
                Guid usuarioId = AppUser.GetUserId();
                string usuarioEmail = AppUser.GetUserEmail();

                await ArtigoService.ExcluirArtigo(artigoId);

                ICollection<Tema> temas = await TemaService.ListarTodosTemas();
                ICollection<TemaDto> temasDtos = RetornaTemasDtoMapeado(temas);

                model.ListaTemas = temasDtos;

                return CustomResponse(model);

            }
            catch (Exception ex)
            {
                string mensagem = $"{ex.Message}";
                NotificarErro(mensagem);
                return CustomResponse();
            }
        }


        private ICollection<TemaDto> RetornaTemasDtoMapeado(ICollection<Tema> temas)
        {
            return Mapper.Map<ICollection<Tema>, ICollection<TemaDto>>(temas);
        }

    }
}
