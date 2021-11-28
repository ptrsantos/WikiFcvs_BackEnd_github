using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using WikiFCVS.Api.Controllers;
using WikiFCVS.Api.Dto;
using WikiFCVS.Domain.Intefaces.Notificacoes;
using WikiFCVS.Domain.Intefaces.Repository;
using WikiFCVS.Domain.Intefaces.Services;
using WikiFCVS.Domain.Models;
using WikiFCVS.Identity.Interfaces.User;
using WikiFCVS.Api.ViewModel;
using System.Collections.Generic;
using WikiFCVS.Identity.Extensions;

namespace DevIO.Api.Controllers
{
    [Authorize]
    [ClaimsAuthorize("perfil", "Administrador")]
    [Route("api/temas")]
    public class TemasController : MainController
    {
        private readonly ITemaService TemaService;
        private readonly IArtigoRepository ArtigoRepository;
        private readonly IMapper Mapper;
        private readonly IUser AppUser;

        public TemasController(IMapper mapper,
                            ITemaService temaService,
                            IArtigoRepository artigoRepository,
                            INotificador notificador, IUser userApp) : base(notificador, userApp)
        {
            TemaService = temaService;
            ArtigoRepository = artigoRepository;
            Mapper = mapper;

            AppUser = userApp;

        }

        [HttpPost("SalvarEdicaoTema")]
        public async Task<ActionResult> SalvarEdicaoTema(TemaEdicaoDto temaEdicao)
        {
            try
            {
                TemaEdicaoViewModel model = new TemaEdicaoViewModel();
                Guid usuarioId = AppUser.GetUserId();
                string usuarioEmail = AppUser.GetUserEmail();
                Tema temaRetorno = await TemaService.SalvarEdicaoTema(temaEdicao.Id, temaEdicao.Titulo, usuarioId, usuarioEmail);
                TemaDto temaDtoRetono = MapearTemaParaDto(temaRetorno);

                model.Tema = temaDtoRetono;

                ICollection<Tema> temas = await TemaService.ListarTodosTemas();
                ICollection<TemaDto> temasDtos = RetornaTemasDtoMapeado(temas);

                model.ListaTemas = temasDtos;

                return CustomResponse(model);
            }
            catch (Exception ex)
            {
                NotificarErro($"{ex.Message}, {ex.InnerException.Message}");
                throw ex;
            }
        }

        [HttpPost("SalvarEdicaoVinculoTema")]
        public async Task<ActionResult> SalvarEdicaoVinculoTema(SalvarEdicaoVinculoTema vinculoTituloModel)
        {
            try
            {
                TemaEdicaoViewModel model = new TemaEdicaoViewModel();
                Guid usuarioId = AppUser.GetUserId();
                string usuarioEmail = AppUser.GetUserEmail();

                TemaDto temaInicialDto = vinculoTituloModel.TemaInicial;
                Tema temaInicial = MapearTemaParaDominio(temaInicialDto);
                //temaInicial.Artigos = MapearArtigoExibicaoParaDominio(temaInicialDto.Artigos);

                TemaDto temaSecundarioDto = vinculoTituloModel.TemaSecundario;
                Tema temaSecundario = MapearTemaParaDominio(temaSecundarioDto);
                //temaSecundario.Artigos = MapearArtigoExibicaoParaDominio(temaSecundarioDto.Artigos);

                Tema temaRetorno = await TemaService.SalvarEdicaoVinculoTema(temaInicial, temaSecundario);
                TemaDto temaDtoRetono = MapearTemaParaDto(temaRetorno);

                model.Tema = temaDtoRetono;

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

        [HttpDelete("ExcluirTema")]
        public async Task<ActionResult> ExcluirTema(int temaId)
        {
            try
            {
                var tema = temaId;
                TemaEdicaoViewModel model = new TemaEdicaoViewModel();
                Guid usuarioId = AppUser.GetUserId();
                string usuarioEmail = AppUser.GetUserEmail();

                //TemaDto temaInicialDto = null;// vinculoTituloModel.TemaInicial;
                //Tema temaInicial = MapearTemaParaDominio(temaInicialDto);
                ////temaInicial.Artigos = MapearArtigoExibicaoParaDominio(temaInicialDto.Artigos);

                //TemaDto temaSecundarioDto = null;// vinculoTituloModel.TemaSecundario;
                //Tema temaSecundario = MapearTemaParaDominio(temaSecundarioDto);
                ////temaSecundario.Artigos = MapearArtigoExibicaoParaDominio(temaSecundarioDto.Artigos);

                await TemaService.ExcluirTema(temaId);
                //TemaDto temaDtoRetono = MapearTemaParaDto(temaRetorno);

                //model.Tema = temaDtoRetono;

                ICollection<Tema> temas = await TemaService.ListarTodosTemas();
                ICollection<TemaDto> temasDtos = RetornaTemasDtoMapeado(temas);

                model.ListaTemas = temasDtos;

                return CustomResponse(model);
                //return CustomResponse();
            }
            catch (Exception ex)
            {
                string mensagem = $"{ex.Message}";
                NotificarErro(mensagem);
                return CustomResponse();
            }
        }

        private Tema MapearTemaParaDominio(TemaDto temaDto)
        {
            return Mapper.Map<TemaDto, Tema>(temaDto);
        }

        private ICollection<Artigo> MapearArtigoExibicaoParaDominio(ICollection<ArtigoExibicaoDto> artigosExibicoesDto)
        {
            return Mapper.Map<ICollection<ArtigoExibicaoDto>, ICollection<Artigo>>(artigosExibicoesDto);
        }

        private TemaDto MapearTemaParaDto(Tema tema)
        {
            return Mapper.Map<Tema, TemaDto>(tema);
        }

        private ICollection<TemaDto> RetornaTemasDtoMapeado(ICollection<Tema> temas)
        {
            return Mapper.Map<ICollection<Tema>, ICollection<TemaDto>>(temas);
        }
    }
}
