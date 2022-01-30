using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WikiFCVS.Api.Dto;
using WikiFCVS.Api.ViewModel;
using WikiFCVS.Domain.Intefaces.Notificacoes;
using WikiFCVS.Domain.Intefaces.Services;
using WikiFCVS.Domain.Models;
using WikiFCVS.Identity.Extensions;
using WikiFCVS.Identity.Interfaces.User;

namespace WikiFCVS.Api.Controllers
{
    [Authorize]
    [ClaimsAuthorize("perfil", "Gestor")]
    [Route("api/Estatisticas")]
    public class EstatisticasController : MainController
    {
        private readonly IEstatisticaService EstatisticaService;
        private readonly IMapper Mapper;

        public EstatisticasController( IMapper mapper,
                                       IEstatisticaService estatisticaService, 
                                       INotificador notificador, 
                                       IUser appUser) : base(notificador, appUser)
        {
            EstatisticaService = estatisticaService;
            Mapper = mapper;
        }


        [HttpGet("ListarDadosEstitisticos")]
        public async Task<ActionResult> ListarDadosEstitisticos()
        {
            EstatisticaViewModel model = new EstatisticaViewModel();
    
            model.ListaEstatisticaUsuarios = MapearListaMesTotalDominioParaDto(await EstatisticaService.ListarEstatisticaUsuarios());
            model.ListaEstatisticaArtigos = MapearListaMesTotalDominioParaDto(await EstatisticaService.ListarEstatisticaArtigos());
            model.ListaEstatisticaEdicoes = MapearListaMesTotalDominioParaDto(await EstatisticaService.ListarEstatisticaEdicoes());
            return CustomResponse(model);
        }

        private ICollection<MesTotalDto> MapearListaMesTotalDominioParaDto(ICollection<MesTotal> lista)
        {
            return Mapper.Map<ICollection<MesTotal>, ICollection<MesTotalDto>>(lista);
        }

        private MesTotalDto MapearMesTotalDominioParaDto(MesTotal mesTotal)
        {
            return Mapper.Map<MesTotal, MesTotalDto>(mesTotal);
        }
    }
}