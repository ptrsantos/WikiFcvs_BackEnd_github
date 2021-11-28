using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using WikiFCVS.Data.Context;
using WikiFCVS.Data.Repository;
using WikiFCVS.Domain.Intefaces.Notificacoes;
using WikiFCVS.Domain.Intefaces.Repository;
using WikiFCVS.Domain.Intefaces.Services;
using WikiFCVS.Domain.Notificacoes;
using WikiFCVS.Domain.Services;
using WikiFCVS.Identity.Extensions;
using WikiFCVS.Identity.Interfaces.User;
using WikiFCVS.Identity.Services;
using WikiFCVS.Identity.Interfaces.Services;
using WikiFCVS.Identity.Data;

namespace WikiFCVS.Api.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection ResolveDependecies(this IServiceCollection services)
        {
            services.AddScoped<WikiFCVSContext>();//ApplicationDbContext
            services.AddScoped<ApplicationDbContext>();
            services.AddScoped<IArtigoRepository, ArtigoRepository>();
            services.AddScoped<ITemaRepository, TemaRepository>();
            services.AddScoped<IEdicaoArtigoRepository, EdicaoArtigoRepository>();
            services.AddScoped<IEdicaoTemaRepository, EdicaoTemaRepository>();
            services.AddScoped<IRegistroUsuarioRepository, RegistroUsuarioRepository>();

            services.AddScoped<ITemaService, TemaService>();
            services.AddScoped<IArtigoService, ArtigoService>();
            services.AddScoped<IEdicaoService, EdicaoService>();
            services.AddScoped<IAspNetUserService, AspNetUserService>();
            services.AddScoped<IRegistroUsuarioService, RegistroUsuarioService>();
            services.AddScoped<IEstatisticaService, EstatisticaService>();

            services.AddScoped<INotificador, Notificador>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IUser, AspNetUser>();

            return services;
        }
    }
}
