using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WikiFCVS.Api.Configuration;
//using WikiFCVS.Api.Extensions;
using WikiFCVS.Data.Context;
using WikiFCVS.Identity.Data;
using WikiFCVS.Identity.Extensions;


/*ComentarioTeste*/

namespace WikiFCVS.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {

            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<WikiFCVSContext>(options => options
                .UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
                .EnableSensitiveDataLogging()
                
            );

            //services.AddTransient<ApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
            services.AddDbContext<ApplicationDbContext>(options => options
                .UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
                .EnableSensitiveDataLogging()

            );

 
            /*Configuração para usara MySql*/
            //services.AddDbContext<WikiFCVSContext>(options => options.UseMySql(Configuration.GetConnectionString("DefaultConnection")));
            //services.AddDbContext<ApplicationDbContext>(options => options.UseMySql(Configuration.GetConnectionString("DefaultConnection")));
            /*Fim da configuração */

            //services.AddCors(options =>
            //{
            //    options.AddPolicy("AllowAll",
            //        builder =>
            //        {
            //            builder
            //            .AllowAnyOrigin()
            //            .AllowAnyMethod()
            //            .AllowAnyHeader()
            //            .AllowCredentials();
            //        });
            //});

            services.AddAutoMapper(typeof(Startup));

            services.WebApiConfig();

            services.AddIdentityConfiguration(Configuration);

            services.ResolveDependecies();



        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
 
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

            }
            if (env.IsProduction() || env.IsStaging() || env.IsEnvironment("Staging_2"))
            {
                app.UseExceptionHandler("/Error");

                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
                // todo: replace with app.UseHsts(); once the feature will be stable
                //app.UseRewriter(new RewriteOptions().AddRedirectToHttps(StatusCodes.Status301MovedPermanently, 443));
            }

            app.UseHttpsRedirection();

            //app.UseCors(builder => builder
            //             .AllowAnyOrigin()
            //             .AllowAnyMethod()
            //             .AllowAnyHeader()
            //             .AllowCredentials()
            //             );

            //app.UseCors("AllowAll");

            app.UseAuthentication();

            app.UseMvcConfirguarion();

        }
    }
}
