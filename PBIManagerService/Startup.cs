using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Serialization;
using PBIManagerService.Common;
using PBIManagerService.Contracts;
using PBIManagerService.Services;

namespace PBIManagerService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.Configure<AppConfigSettings>(Configuration.GetSection("ApplicationSettings"));

            services.AddTransient<AzureAuthenticationTokenHandler>();
            services.AddTransient<ITokenService, TokenService>();
            services.AddTransient<IPBIService, PBIService>();
            services.AddTransient<IAzureService, AzureService>();
            services.AddTransient<IWorkspaceService, WorkspaceService>();

            services.AddHttpClient<ICapacityService, CapacityService>()
                .AddHttpMessageHandler<AzureAuthenticationTokenHandler>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
