using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using OnlineGallery.BLL.Exceptions;
using OnlineGallery.BLL.Options;
using OnlineGallery.BLL.Services.Interfaces;
using OnlineGallery.DAL.FileWork;
using OnlineGallery.DAL.UnitOfWork;

namespace OnlineGallery.BLL.Services
{
    public class ImageFilesService : IImageFilesService
    {
        private readonly ImageFileOptions _fileOptions;
        private readonly IImageProvider _imageProvider;
        private readonly IUnitOfWork _unitOfWork;

        public ImageFilesService(IUnitOfWork unitOfWork, IImageProvider imageProvider,
            IOptions<ImageFileOptions> fileOptions)
        {
            _unitOfWork = unitOfWork;
            _imageProvider = imageProvider;
            _fileOptions = fileOptions.Value;
        }

        public async Task<(Stream, string)> GetImageFile(string id)
        {
            var image = await _unitOfWork.ImageRepository.Get(id);
            if (image == null)
                throw new ObjectNotFoundException($"image for id {id} not found");

            var stream = _imageProvider.GetImage(Path.Combine(_fileOptions.DirectoryPath,
                image.UserId, image.NameForCompressed));

            return (stream, image.NameForCompressed);
        }

        public async Task<(Stream, string)> GetFullImageFile(string id)
        {
            var image = await _unitOfWork.ImageRepository.Get(id);
            if (image == null)
                throw new ObjectNotFoundException($"image for id {id} not found");

            var stream = _imageProvider.GetImage(Path.Combine(_fileOptions.DirectoryPath,
                image.UserId, image.Name));

            return (stream, image.Name);
        }
    }
}