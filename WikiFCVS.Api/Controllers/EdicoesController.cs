using AutoMapper;
using DevIO.Api.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WikiFCVS.Api.Dto;
using WikiFCVS.Identity.Extensions;
using WikiFCVS.Api.ViewModel;
using WikiFCVS.Domain.Intefaces.Notificacoes;
using WikiFCVS.Domain.Intefaces.Services;
using WikiFCVS.Domain.Models;
using WikiFCVS.Identity.Interfaces.User;

namespace WikiFCVS.Api.Controllers
{
    //[Authorize]
    [Route("api/edicoes")]
    public class EdicoesController : MainController
    {
        private readonly ITemaService TemaService;
        private readonly IEdicaoService EdicaoService;
        private readonly IMapper Mapper;
        private readonly IUser AppUser;

        public EdicoesController(IMapper mapper,
                                ITemaService temaService,
                                IEdicaoService edicaoService,
                                INotificador notificador, IUser userApp) : base(notificador, userApp)
        {
            TemaService = temaService;
            EdicaoService = edicaoService;
            Mapper = mapper;
            //ContextAccessor = contextAccessor;
            AppUser = userApp;

        }

        [HttpPost("SalvarInclusao")]
        public async Task<ActionResult> SalvarInclusao(InclusaoViewModel inclusaoModel)
        {
            try
            {
                InicializacaoHomeViewModel model = new InicializacaoHomeViewModel();


                EdicaoTema edicaoTema = MapearEdicaoTemaParaDominio(inclusaoModel.TemaInclusao.Edicao);
                EdicaoArtigo edicaoArtigo = MapearEdicaoArtigoParaDominio(inclusaoModel.TemaInclusao.Artigo.EdicaoArtigo);
                Guid usuarioId = AppUser.GetUserId();
                string usuarioEmail = AppUser.GetUserEmail();
                EdicaoArtigo edicaoDomain = await EdicaoService.SalvarInclusaoDados(edicaoTema, edicaoArtigo, usuarioId, usuarioEmail);
                ArtigoEdicaoDto artigoEdicaoDto = Mapper.Map<EdicaoArtigo, ArtigoEdicaoDto>(edicaoDomain);

                model.ArtigoEdicao = artigoEdicaoDto;
                ICollection<Tema> Temas = await TemaService.ListarTodosTemas();
                ICollection<TemaDto> TemasDtos = RetornaTemasDtoMapeado(Temas);
                model.Temas = TemasDtos;

                return CustomResponse(model);
            }
            catch (System.Exception ex)
            {
                NotificarErro(ex.Message);
                return CustomResponse();
            }

        }

        [HttpPost("SalvarEdicao")]
        public async Task<ActionResult> SalvarEdicao(EdicaoViewModel edicaoModel)
        {
            try
            {
                if (!ModelState.IsValid) return CustomResponse(ModelState);

                InicializacaoHomeViewModel model = new InicializacaoHomeViewModel();
                Artigo artigo = MapearArtigoDtoParaDominio(edicaoModel.Artigo);
                EdicaoArtigo edicaoArtigo = MapearEdicaoArtigoParaDominio(edicaoModel.Artigo.EdicaoArtigo);
                Guid usuarioId = AppUser.GetUserId();
                string usuarioEmail = AppUser.GetUserEmail();
                EdicaoArtigo edicaoDomain = await EdicaoService.SalvarArtigoEdicao(artigo, edicaoArtigo, usuarioId, usuarioEmail);

                ArtigoEdicaoDto artigoEdicaoDto = Mapper.Map<EdicaoArtigo, ArtigoEdicaoDto>(edicaoDomain);
                model.ArtigoEdicao = artigoEdicaoDto;
                ICollection<Tema> Temas = await TemaService.ListarTodosTemas();
                ICollection<TemaDto> TemasDtos = RetornaTemasDtoMapeado(Temas);
                model.Temas = TemasDtos;
                return CustomResponse(model);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Execption: ", ex.Message);
                return CustomResponse(ModelState);
                //NotificarErro($"{ex.Message});
                //return CustomResponse();
            }

        }



        //[AllowAnonymous]
        [HttpGet("ListarArtigosEdicoesHistorico")]
        public async Task<ActionResult> ListarArtigosEdicoesHistorico(int pagina, int tamanho)
        {
            try
            {
                var model = new PreenchimentoTabelaHistoricoViewModel();
                ICollection<EdicaoArtigo> edicoes = await EdicaoService.ListarArtigosEdicoesHistorico(pagina, tamanho);
                model.ListaArtigosEdicoes = MapearEdicaoParaTemaArtigoDataTable(edicoes);
                model.QuantidadeItens = await EdicaoService.RetornaQuantidadeArtigoEdicoes();
                return CustomResponse(model);
            }
            catch (Exception ex)
            {
                NotificarErro($"{ex.Message}");
                throw ex;
            }
        }


        [HttpGet("ListarArtigosEdicoesHistoricoComFiltro")]
        public async Task<ActionResult> ListarArtigosEdicoesHistoricoComFiltro(int pagina, int tamanho, string filtro)
        {
            try
            {
                var model = new PreenchimentoTabelaHistoricoViewModel();
                filtro = filtro == null ? "" : filtro;
                TransporteDadosTabela transporteDadosTabela = await EdicaoService.ListarArtigosEdicoesHistoricoComFiltro(pagina, tamanho, filtro);
                ICollection<EdicaoArtigo> edicoes = transporteDadosTabela.Edicoes;
                model.ListaArtigosEdicoes = MapearEdicaoParaTemaArtigoDataTable(edicoes);
                model.QuantidadeItens = transporteDadosTabela.TotalDeItens;
                return CustomResponse(model);
            }
            catch (Exception ex)
            {
                NotificarErro($"{ex.Message}, {ex.InnerException.Message}");
                throw ex;
            }
        }


        [HttpGet("RetornaArtigoEdicaoHistorico")]
        public async Task<ActionResult> RetornaArtigoEdicaoHistorico(int edicaoId)
        {
            try
            {
                InicializacaoHomeViewModel model = new InicializacaoHomeViewModel();

                EdicaoArtigo edicaoDomain = await EdicaoService.RetornaArtigoEdicaoHistorico(edicaoId);
                ArtigoEdicaoDto edicaoDto = RetornaComArtigoEdicaoDtoMapeado(edicaoDomain);

                model.ArtigoEdicao = edicaoDto;
                ICollection<Tema> Temas = await TemaService.ListarTodosTemas();
                ICollection<TemaDto> TemasDtos = RetornaTemasDtoMapeado(Temas);
                model.Temas = TemasDtos;
                return CustomResponse(model);
            }
            catch (Exception ex)
            {
                NotificarErro($"{ex.Message}, {ex.InnerException.Message}");
                throw ex;
            }
        }

        [HttpGet("RetornaArtigoEdicaoPorId")]
        public async Task<ActionResult> RetornaArtigoEdicaoPorId(int edicaoId)
        {
            try
            {
                InicializacaoHomeViewModel model = new InicializacaoHomeViewModel();

                EdicaoArtigo edicaoDomain = await EdicaoService.RetornaArtigoEdicaoPorId(edicaoId);
                ArtigoEdicaoDto edicaoDto = RetornaComArtigoEdicaoDtoMapeado(edicaoDomain);

                model.ArtigoEdicao = edicaoDto;
                ICollection<Tema> Temas = await TemaService.ListarTodosTemas();
                ICollection<TemaDto> TemasDtos = RetornaTemasDtoMapeado(Temas);
                model.Temas = TemasDtos;

                return CustomResponse(model);
            }
            catch (Exception ex)
            {
                NotificarErro($"{ex.Message}");
                throw ex;
            }
        }

        [HttpGet("RetornaArtigoEdicaoPorArtigoId")]
        public async Task<ActionResult> RetornaArtigoEdicaoPorArtigoId(int artigoId)
        {
            try
            {
                InicializacaoHomeViewModel model = new InicializacaoHomeViewModel();

                EdicaoArtigo edicaoDomain = await EdicaoService.RetornaArtigoEdicaoPorArtigoId(artigoId);
                ArtigoEdicaoDto edicaoComDto = RetornaComArtigoEdicaoDtoMapeado(edicaoDomain);

                model.ArtigoEdicao = edicaoComDto;
                ICollection<Tema> Temas = await TemaService.ListarTodosTemas();
                ICollection<TemaDto> TemasDtos = RetornaTemasDtoMapeado(Temas);
                model.Temas = TemasDtos;

                return CustomResponse(model);
            }
            catch (Exception ex)
            {
                NotificarErro($"{ex.Message}");
                throw ex;
            }
        }

        private Artigo MapearArtigoDtoParaDominio(ArtigoDto artigoDto)
        {
            return Mapper.Map<ArtigoDto, Artigo>(artigoDto); 
        }

        private ICollection<TemaArtigoDataTableDto> MapearEdicaoParaTemaArtigoDataTable(ICollection<EdicaoArtigo> edicoes)
        {
            return Mapper.Map<ICollection<EdicaoArtigo>, ICollection<TemaArtigoDataTableDto>>(edicoes);
        }

        private ArtigoEdicaoDto RetornaComArtigoEdicaoDtoMapeado(EdicaoArtigo edicaoDomain)
        {
            return Mapper.Map<EdicaoArtigo, ArtigoEdicaoDto>(edicaoDomain);
        }

        private Tema MapearTemaParaDominio(TemaDto temaDto)
        {
            return Mapper.Map<TemaDto, Tema>(temaDto);
        }

        private Tema MapearTemaIncluaoDtoParaDominio(TemaInclusaoDto temaDto)
        {
            return Mapper.Map<TemaInclusaoDto, Tema>(temaDto);
        }

        private Artigo MapearArtigoParaDominio(ArtigoDto artigoDto)
        {
            return Mapper.Map<ArtigoDto, Artigo>(artigoDto);
        }

        private EdicaoArtigo MapearEdicaoArtigoParaDominio(EdicaoArtigoDto edicaoDto)
        {
            return Mapper.Map<EdicaoArtigoDto, EdicaoArtigo>(edicaoDto);
        }

        private EdicaoTema MapearEdicaoTemaParaDominio(EdicaoTemaDto edicaoDto)
        {
            return Mapper.Map<EdicaoTemaDto, EdicaoTema>(edicaoDto);
        }

        private ICollection<TemaDto> RetornaTemasDtoMapeado(ICollection<Tema> temas)
        {
            return Mapper.Map<ICollection<Tema>, ICollection<TemaDto>>(temas);
        }



    }
}
