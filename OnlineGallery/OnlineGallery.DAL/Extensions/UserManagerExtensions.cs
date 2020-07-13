using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using OnlineGallery.DAL.Models;
using OnlineGallery.DAL.Models.Pagination;

namespace OnlineGallery.DAL.Extensions
{
    public static class UserManagerExtensions
    {
        public static async Task<PagedData<User>> SearchByUsername(this UserManager<User> userManager, string query,
            PaginationOptions options)
        {
            var queryable = userManager.Users.Where(u =>
                u.UserName.Contains(query) || u.FirstName.Contains(query) || u.LastName.Contains(query));
            var data = new PagedData<User>();
            return await data.Create(queryable, options);
        }
    }
}