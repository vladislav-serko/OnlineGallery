using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using OnlineGallery.BLL.DTOs.ImageTransfer;
using OnlineGallery.BLL.DTOs.Pagination;
using OnlineGallery.BLL.Exceptions;
using OnlineGallery.BLL.Helpers.Options;
using OnlineGallery.BLL.Services.Interfaces;
using OnlineGallery.DAL.FileWork;
using OnlineGallery.DAL.Models;
using OnlineGallery.DAL.Models.Pagination;
using OnlineGallery.DAL.UnitOfWork;

namespace OnlineGallery.BLL.Services
{
    public class ImagesService : IImagesService
    {
        private readonly ImageFileOptions _fileOptions;
        private readonly IImageProvider _imageProvider;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;

        public ImagesService(
            IUnitOfWork unitOfWork,
            IOptions<ImageFileOptions> fileOptions,
            IMapper mapper,
            IImageProvider imageProvider,
            UserManager<User> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _imageProvider = imageProvider;
            _userManager = userManager;
            _fileOptions = fileOptions.Value;
        }

        public async Task<ImageDto> AddImage(ImagePostRequest request)
        {
            await ValidateImage(request);

            var extension = GetExtension(request.File.FileName);

            var image = _mapper.Map<Image>(request);

            image.Id = Guid.NewGuid().ToString();
            image.Name = $"{image.Id}{extension}";
            image.NameForCompressed = $"{image.Id}_compressed{extension}";

            var pathFull = Path.Combine(_fileOptions.DirectoryPath,
                image.UserId, image.Name);
            var pathCompressed = Path.Combine(_fileOptions.DirectoryPath,
                image.UserId, image.NameForCompressed);

            await _imageProvider.AddImage(request.File.OpenReadStream(),
                pathFull);
            _imageProvider.OptimizeImage(pathFull,
                pathCompressed, _fileOptions.MaxQuality);

            _unitOfWork.ImageRepository.Add(image);
            await _unitOfWork.Commit();

            return _mapper.Map<ImageDto>(image);
        }

        private string GetExtension(string name)
        {
            var extension = Path.GetExtension(name);

            if (extension == null)
                throw new InvalidOperationException("Cant validate file extension");

            if (!_fileOptions.SupportedExtensions.Contains(extension))
                throw new InvalidOperationException("File extension is unsupported");

            return extension;
        }

        private async Task ValidateImage(ImagePostRequest request)
        {
            if (await _userManager.FindByIdAsync(request.UserId) == null)
                throw new ObjectNotFoundException($"Entity with id {request.UserId} not found");

            if (request.File.Length > _fileOptions.MaxSize)
                throw new InvalidOperationException($"The file is too large. Max size is {_fileOptions.MaxSize} bytes");
        }

        public async Task<ImageDto> GetImage(string id)
        {
            var image = await _unitOfWork.ImageRepository.Get(id);
            if (image == null)
                throw new ObjectNotFoundException($"Image with id:{id} not found");
            var dto = _mapper.Map<ImageDto>(image);
            dto.LikeCount = await _unitOfWork.ImageRepository.GetLikeCount(id);
            return dto;
        }

        public async Task<PaginationResponse<ImageDto>> GetImages(string userId, PaginationRequest request)
        {
            var options = _mapper.Map<PaginationOptions>(request);

            var pagedData = await _unitOfWork.ImageRepository.GetImagesByUser(userId, options);
            return await AddLikesCounts(pagedData);
        }

        public async Task<PaginationResponse<ImageDto>> SearchImages(string query, PaginationRequest request)
        {
            var options = _mapper.Map<PaginationOptions>(request);

            var pagedData = await _unitOfWork.ImageRepository.Search(query, options);
            return await AddLikesCounts(pagedData);
        }

        public async Task<ImageDto> Update(ImageUpdateRequest request)
        {
            var image = await _unitOfWork.ImageRepository.Get(request.Id);
            if (image == null)
                throw new ObjectNotFoundException($"Image with id:{request.Id} not found.");
            if (image.UserId != request.UserId)
                throw new ImageAccessException("This image cannot be accessed by this user.");

            var mapped = _mapper.Map(request, image);
            await _unitOfWork.Commit();
            return _mapper.Map<ImageDto>(mapped);
        }

        public async Task DeleteImage(string imageId, string userId, bool deletedByModerator)
        {
            var image = await _unitOfWork.ImageRepository.Get(imageId);
            if (image == null)
                throw new ObjectNotFoundException($"Image with id:{imageId} not found.");
            if (image.UserId != userId && !deletedByModerator)
                throw new ImageAccessException("This image cannot be accessed by this user.");

            var imagePath = Path.Combine(_fileOptions.DirectoryPath, userId);

            _imageProvider.DeleteImage(imagePath, image);
            _unitOfWork.ImageRepository.Delete(image);
            await _unitOfWork.Commit();
        }

        private async Task<PaginationResponse<ImageDto>> AddLikesCounts(PagedData<Image> pagedData)
        {
            var result = _mapper.Map<PaginationResponse<ImageDto>>(pagedData);
            foreach (var imageDto in result.Data)
            {
                imageDto.LikeCount = await _unitOfWork.ImageRepository.GetLikeCount(imageDto.Id);
            }

            return result;
        }

    }
}