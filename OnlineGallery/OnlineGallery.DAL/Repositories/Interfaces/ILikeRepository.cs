using System.Threading.Tasks;
using OnlineGallery.DAL.Models;

namespace OnlineGallery.DAL.Repositories.Interfaces
{
    public interface ILikeRepository
    {
        void AddLike(Like like);
        void RemoveLike(Like like);
        Task<bool> Exist(Like like);
    }
}