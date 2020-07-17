using System;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OnlineGallery.Api.Extensions;
using OnlineGallery.BLL.Helpers.Extensions;
using OnlineGallery.BLL.Helpers.MappingProfiles;

namespace OnlineGallery.Api
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
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .WithExposedHeaders("Content-Disposition"));
                    
            });

            services.AddControllers();

            services.AddOnlineGalleryContext(Configuration.GetConnectionString("Default"));

            services.AddIdentityForUser();

            services.AddDalDependencies();

            services.AddAppOptions(Configuration);

            //services.AddFilters();

            services.AddAppAuthentication(Configuration);

            services.AddAuthorization();

            services.AddAutoMapper(typeof(RequestToDomain));

            services.AddServices();
        }

        public void Configure(IApplicationBuilder app, IServiceProvider provider, IWebHostEnvironment env)
        {
            app.UseCors("CorsPolicy");

            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseErrorHandling();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            app.SeedUsersAndRoles(provider).Wait();
        }
    }
}