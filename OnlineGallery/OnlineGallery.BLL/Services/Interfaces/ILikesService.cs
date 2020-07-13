using System.Threading.Tasks;
using OnlineGallery.BLL.DTOs;

namespace OnlineGallery.BLL.Services.Interfaces
{
    public interface ILikesService
    {
        Task AddLike(string imageId, string userId);
        Task RemoveLike(string imageId, string userId);
        Task<bool> LikeExist(string imageId, string userId);
    }
}