using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AmazonCognitoSpike.Services;
using AmazonCognitoSpike.Services.IAASServices;
using AmazonCognitoSpike.Services.IAASServices.Implementations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using AmazonCognitoSpike.Auth;
using Microsoft.Extensions.DependencyInjection;
using AmazonCognitoSpike.Data;

namespace AmazonCognitoSpike
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(Microsoft.Extensions.DependencyInjection.IServiceCollection services)
        {
            services.AddAuthentication("Bearer")
                .DSAddJwtBearer(options =>
                {
                    options.AudienceAuthorityResolver = new CognitoUserPoolResolver(new DataContext());
                });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddScoped<IIAASService, AWSCognitoService>();
            services.AddScoped<DataContext>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseAuthentication();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
