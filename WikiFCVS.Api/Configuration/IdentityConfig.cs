using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using WikiFCVS.Identity.Data;
using WikiFCVS.Identity.Extensions;
using Microsoft.Extensions.Logging.Console;
//using WikiFCVS.Api.Data;
//using WikiFCVS.Api.Extensions;

namespace WikiFCVS.Api.Configuration
{
    public static class IdentityConfig
    {
        public static IServiceCollection AddIdentityConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<IdentityUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddErrorDescriber<IdentityMensagensPortugues>()
                .AddDefaultTokenProviders();

            // JWT
            var appSettingsSection = configuration.GetSection("AppSettings");//Vai no appsettings.json pegar a sessão 'AppSettings'
            services.Configure<AppSettings>(appSettingsSection);//Inicializa a configuração da sessão do appsettings.json

            var appSettings = appSettingsSection.Get<AppSettings>();//Obtem os dados da classe appsettings.json que foi configurada (AppSettings é uma classe que representa um trecho da sessão appsettings.json )
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);//Define uma chave com base na propriedade 'Secret' do appsettings.josn

            //Adciona uma autenticação
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;//Informa qual o tipo de autenticação do sistema
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;//Idem
            }).AddJwtBearer(x => {
                x.RequireHttpsMetadata = false;//Caso seja https deve ser true
                x.SaveToken = true;//O token deve ser guardo (true - sim)
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,//Vai validar se o emissor é o mesmo com base no nome e chave
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true, 
                    ValidateAudience = true, 
                    ValidAudience = appSettings.ValidoEm,//Valida onde o token é válido
                    ValidIssuer = appSettings.Emissor//Valida o emissor
                };
            });

            return services;
        }
    }
}
