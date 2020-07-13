using System.Threading.Tasks;
using OnlineGallery.DAL.Repositories.Interfaces;

namespace OnlineGallery.DAL.UnitOfWork
{
    public interface IUnitOfWork
    {
        IImageRepository ImageRepository { get; }
        ILikeRepository LikeRepository { get; }
        Task Commit();
    }
}