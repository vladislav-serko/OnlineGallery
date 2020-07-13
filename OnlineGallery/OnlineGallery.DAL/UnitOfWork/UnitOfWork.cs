using System.Threading.Tasks;
using OnlineGallery.DAL.Repositories;
using OnlineGallery.DAL.Repositories.Interfaces;

namespace OnlineGallery.DAL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly OnlineGalleryContext _context;
        private IImageRepository _imageRepository;
        private ILikeRepository _likeRepository;

        public UnitOfWork(OnlineGalleryContext context)
        {
            _context = context;
        }

        public IImageRepository ImageRepository => _imageRepository ??= new ImageRepository(_context);

        public ILikeRepository LikeRepository => _likeRepository ??= new LikeRepository(_context);

        public async Task Commit()
        {
            await _context.SaveChangesAsync();
        }
    }
}