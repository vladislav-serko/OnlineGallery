using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OnlineGallery.DAL;
using OnlineGallery.DAL.FileWork;
using OnlineGallery.DAL.Models;
using OnlineGallery.DAL.UnitOfWork;

namespace OnlineGallery.BLL.Helpers.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddOnlineGalleryContext(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<OnlineGalleryContext>(options => options.UseSqlServer(connectionString));
        }

        public static void AddIdentityForUser(this IServiceCollection services)
        {
            services.AddIdentityCore<User>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<OnlineGalleryContext>();
        }

        public static void AddDalDependencies(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IImageProvider, ImageProvider>();
        }
    }
}