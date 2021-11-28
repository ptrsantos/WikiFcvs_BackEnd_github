using AutoMapper;
using WikiFCVS.Api.Dto;
using System.Linq;
using WikiFCVS.Api.Dto.Identity;
using WikiFCVS.Identity.Models;
using WikiFCVS.Domain.Models;

namespace WikiFCVS.Api.Configuration
{
    public class AutomapperConfig : Profile
    {
        public AutomapperConfig()
        {
            CreateMap<Artigo, ArtigoDto>();
            //.ForMember(dest => dest.Titulo, opt => opt.MapFrom(src => src.RetornaUltimaEdicao().Titulo))
            //.ForMember(dest => dest.Titulo, opt => opt.MapFrom(src => src.Edicoes.LastOrDefault().Titulo))
            //.ForMember(dest => dest.Conteudo, opt => opt.MapFrom(src => src.RetornaUltimaEdicao().Conteudo));

            CreateMap<Artigo, ArtigoExibicaoDto>()
                .ForMember(dest => dest.Titulo, opt => opt.MapFrom(src => src.Edicoes.LastOrDefault().Titulo))
                .ForMember(dest => dest.AutorUltimaEdicao, opt => opt.MapFrom(src => src.Edicoes.LastOrDefault().EdicaoEfetuada.EditadoPorEmail))
                .ForMember(dest => dest.DataUltimaEdicao, opt => opt.MapFrom(src => src.Edicoes.LastOrDefault().EdicaoEfetuada.EditadoEm));


            CreateMap<Tema, TemaInclusaoDto>();

            CreateMap<Tema, TemaDto>()
                .ForMember(dest => dest.Id, map => map.MapFrom(src => src.Id))
                .ForMember(dest => dest.Titulo, map => map.MapFrom(src => src.RetornaUltimaEdicaoDoTema().Titulo))
                .ForMember(dest => dest.AutorId, map => map.MapFrom(src => src.RetornaUltimaEdicaoDoTema().EdicaoEfetuada.EditadoPorId))
                .ForMember(dest => dest.AutorUltimaEdicao, map => map.MapFrom(src => src.RetornaUltimaEdicaoDoTema().EdicaoEfetuada.EditadoPorEmail))
                .ForMember(dest => dest.DataUltimaEdicao, map => map.MapFrom(src => src.RetornaUltimaEdicaoDoTema().EdicaoEfetuada.EditadoEm));

            CreateMap<Tema, TemaCompletoDto>();
                //.ForMember(dest => dest.EditadoPorId, opt => opt.MapFrom(src => src.EdicaoEfetuada.EditadoPorId))
                //.ForMember(dest => dest.EditadoPorEmail, opt => opt.MapFrom(src => src.EdicaoEfetuada.EditadoPorEmail))
                //.ForMember(dest => dest.EditadoEm, opt => opt.MapFrom(src => src.EdicaoEfetuada.EditadoEm));

            CreateMap<EdicaoArtigo, ArtigoEdicaoDto>()
                .ForMember(dest => dest.EdicaoId, map => map.MapFrom(src => src.Id))
                .ForMember(dest => dest.EdicaoConteudo, map => map.MapFrom(src => src.Conteudo))
                .ForMember(dest => dest.EdicaoData, map => map.MapFrom(src => src.EdicaoEfetuada.EditadoEm))
                .ForMember(dest => dest.ReponsavelEmail, map => map.MapFrom(src => src.EdicaoEfetuada.EditadoPorEmail))
                .ForMember(dest => dest.ArtigoId, map => map.MapFrom(src => src.Artigo.Id))
                .ForMember(dest => dest.ArtigoTitulo, map => map.MapFrom(src => src.Titulo))
                //.ForMember(dest => dest.TemaTitulo, map => map.MapFrom(src => src.Artigo.Tema.Titulo))
                .ForMember(dest => dest.TemaId, map => map.MapFrom(src => src.Artigo.Tema.Id));

            CreateMap<Tema, ArtigoEdicaoDto>()
                .ForMember(dest => dest.EdicaoId, map => map.MapFrom(src => src.RetornaUltimaEdicaoDoUltimoArtigo().Id))
                .ForMember(dest => dest.EdicaoConteudo, map => map.MapFrom(src => src.RetornaUltimaEdicaoDoUltimoArtigo().Conteudo))
                .ForMember(dest => dest.EdicaoData, map => map.MapFrom(src => src.RetornaUltimaEdicaoDoUltimoArtigo().EdicaoEfetuada.EditadoEm))
                .ForMember(dest => dest.ReponsavelEmail, map => map.MapFrom(src => src.RetornaUltimaEdicaoDoUltimoArtigo().EdicaoEfetuada.EditadoPorEmail))
                .ForMember(dest => dest.ArtigoId, map => map.MapFrom(src => src.RetornaUltimoArtigo().Id))
                .ForMember(dest => dest.ArtigoTitulo, map => map.MapFrom(src => src.RetornaUltimaEdicaoDoUltimoArtigo().Titulo))
                .ForMember(dest => dest.TemaTitulo, map => map.MapFrom(src => src.RetornaUltimaEdicaoDoTema().Titulo))
                .ForMember(dest => dest.TemaId, map => map.MapFrom(src => src.Id));


            CreateMap<Artigo, ArtigoCompletoDto>();
                //.ForMember(dest => dest.EditadoPorId, opt => opt.MapFrom(src => src.EdicaoEfetuada.EditadoPorId))
                //.ForMember(dest => dest.EditadoPorEmail, opt => opt.MapFrom(src => src.EdicaoEfetuada.EditadoPorEmail))
                //.ForMember(dest => dest.EditadoEm, opt => opt.MapFrom(src => src.EdicaoEfetuada.EditadoEm));

            CreateMap<Artigo, TemaArtigoDto>()
                //.ForMember(dest => dest.TemaTitulo, opt => opt.MapFrom(src => src.Tema.Titulo))
                .ForMember(dest => dest.TemaId, opt => opt.MapFrom(src => src.Tema.Id))
                .ForMember(dest => dest.ArtigoId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ArtigoTitulo, opt => opt.MapFrom(src => src.Edicoes.LastOrDefault().Titulo));
            //.ForMember(dest => dest.ArtigoDescricao, opt => opt.MapFrom(src => src.Descricao));


            CreateMap<EdicaoArtigo, EdicaoCompletaDto>()
                .ForMember(dest => dest.EditadoPorId, opt => opt.MapFrom(src => src.EdicaoEfetuada.EditadoPorId))
                .ForMember(dest => dest.EditadoPorEmail, opt => opt.MapFrom(src => src.EdicaoEfetuada.EditadoPorEmail))
                .ForMember(dest => dest.EditadoEm, opt => opt.MapFrom(src => src.EdicaoEfetuada.EditadoEm));



            CreateMap<EdicaoArtigo, TemaArtigoDataTableDto>()
                .ForMember(dest => dest.EdicaoId, map => map.MapFrom(src => src.Id))
                .ForMember(dest => dest.EdicaoConteudoResumo, map => map.MapFrom(src => src.RetornaResumoDoConteudo()))
                .ForMember(dest => dest.EdicaoData, map => map.MapFrom(src => src.EdicaoEfetuada.EditadoEm))
                .ForMember(dest => dest.ResponsavelEmail, map => map.MapFrom(src => src.EdicaoEfetuada.EditadoPorEmail))
                .ForMember(dest => dest.ArtigoTitulo, map => map.MapFrom(src => src.Titulo))
                .ForMember(dest => dest.ArtigoId, map => map.MapFrom(src => src.Artigo.Id));

            CreateMap<UsuarioIdentity, UsuarioIdentityDto>()
                .ForMember(dest => dest.ClaimType, map => map.MapFrom(src => src.Claim.Type))
                .ForMember(dest => dest.ClaimValue, map => map.MapFrom(src => src.Claim.Value))
                .ForMember(dest => dest.Bloqueado, map => map.MapFrom(src => src.UsuarioBloqueado()));

            CreateMap<MesTotal, MesTotalDto>();






            CreateMap<ArtigoDto, Artigo>();

            //CreateMap<ArtigoInclusaoDto, Artigo>()
            //    .ForPath(dest => dest.Edicoes.ToList()[0].Titulo, map => map.MapFrom(src => src.Edicao.Titulo))
            //    .ForPath(dest => dest.Edicoes.ToList()[0].Conteudo, map => map.MapFrom(src => src.Edicao.Conteudo));

            CreateMap<TemaDto, Tema>();
                //.ForMember(dest => dest.Artigos.Select(a => a.Id), map => map.MapFrom(src => src.Artigos.Select(a => a.Id)))
                //.ForPath(dest => dest.Edicoes.LastOrDefault().Titulo, map => map.MapFrom(src => src.Titulo));

            //CreateMap<TemaInclusaoDto, Tema>()
            //    .ForMember(dest => dest.Edicoes.ToList()[0].Titulo, map => map.MapFrom(src => src.Edicao.Titulo))
            //    .ForMember(dest => dest.Edicoes.ToList()[0].Conteudo, map => map.MapFrom(src => src.Edicao.Conteudo));

            CreateMap<EdicaoArtigoDto, EdicaoArtigo>()
                .ForMember(dest => dest.Conteudo, opt => opt.MapFrom(src => src.Conteudo));

            CreateMap<EdicaoTemaDto, EdicaoTema>();


            CreateMap<ArtigoExibicaoDto, Artigo>();


        }
    }
}
