using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using OnlineGallery.BLL.Helpers.Options;
using OnlineGallery.BLL.Services;
using OnlineGallery.BLL.Services.Interfaces;

namespace OnlineGallery.Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IImagesService, ImagesService>();
            services.AddScoped<IImageFilesService, ImageFilesService>();
            services.AddScoped<IUsersService, UsersService>();
            services.AddScoped<ILikesService, LikesService>();
        }

        public static void AddAppOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ImageFileOptions>(configuration.GetSection("ImageFileSettings"));
            services.Configure<JwtOptions>(configuration.GetSection("JwtSettings"));
        }

        public static void AddAppAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtOptionsSection = configuration.GetSection("JwtSettings");
            var jwtOptions = jwtOptionsSection.Get<JwtOptions>();

            var key = Encoding.ASCII.GetBytes(jwtOptions.Secret);
            services.AddAuthentication(o =>
            {
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = jwtOptions.Issuer,
                    ValidAudience = jwtOptions.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });
        }

    }
}