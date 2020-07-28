using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Moq;
using OnlineGallery.BLL.DTOs.ImageTransfer;
using OnlineGallery.BLL.Exceptions;
using OnlineGallery.BLL.Services;
using OnlineGallery.BLL.Helpers.Options;
using OnlineGallery.BLL.Helpers.MappingProfiles;
using OnlineGallery.DAL.FileWork;
using OnlineGallery.DAL.Models;
using OnlineGallery.DAL.Repositories.Interfaces;
using OnlineGallery.DAL.UnitOfWork;
using Xunit;

namespace OnlineGallery.BLLTests
{
    public class ImagesServiceTests
    {
        private readonly ImagesService _imagesService;
        private readonly IOptions<ImageFileOptions> _fileOptions = Options.Create(GetOptions());
        private readonly Mock<IImageProvider> _imageProviderMock = new Mock<IImageProvider>();
        private readonly IMapper _mapper = GetMapper();
        private readonly Mock<IUnitOfWork> _uowMock = new Mock<IUnitOfWork>();
        private readonly Mock<UserManager<User>> _userManagerMock = MockUserManager();
        private readonly Mock<IImageRepository> _imagesRepoMock = new Mock<IImageRepository>();

        private readonly List<ImagePostRequest> _postRequests = new List<ImagePostRequest>
        {
            new ImagePostRequest
            {
                File = new FormFile(new MemoryStream(), 1, 300000, null, "a.png"),
                UserId = Guid.NewGuid().ToString(),
                ShortDescription = "",
                Description = ""
            },

            new ImagePostRequest
            {
                File = new FormFile(new MemoryStream(), 1, 10485761, null, "a.png"),
                UserId = Guid.NewGuid().ToString(),
                ShortDescription = "",
                Description = ""
            },

            new ImagePostRequest
            {
                File = new FormFile(new MemoryStream(), 1, 10485761, null, "a"),
                UserId = Guid.NewGuid().ToString(),
                ShortDescription = "",
                Description = ""
            },

            new ImagePostRequest
            {
                File = new FormFile(new MemoryStream(), 1, 10485761, null, "a.exe"),
                UserId = Guid.NewGuid().ToString(),
                ShortDescription = "",
                Description = ""
            }
        }; 


        public ImagesServiceTests()
        {
            _imagesService = new ImagesService(
                _uowMock.Object,
                _fileOptions,
                _mapper,
                _imageProviderMock.Object,
                _userManagerMock.Object
            );
        }

        [Fact]
        public async Task AddImage_ValidRequest_ReturnsImageDto()
        {
            //Arrange
            var request = _postRequests[0];
            _userManagerMock.Setup(x => x.FindByIdAsync(request.UserId))
                .ReturnsAsync(new User { Id = request.UserId });
            _uowMock.Setup(x => x.ImageRepository)
                .Returns(_imagesRepoMock.Object);
            //Act
            var image = await _imagesService.AddImage(request);
            //Assert
            Assert.True(
              image.UserId == request.UserId &&
              image.Description == request.Description &&
              image.ShortDescription == request.ShortDescription
              );
        }

        [Fact]
        public async Task AddImage_UserWithProvidedIdNotFound_ThrowsObjectNoFoundException()
        {
            //Arrange
            var request = _postRequests[0];
            _userManagerMock.Setup(x => x.FindByIdAsync(request.UserId))
                .ReturnsAsync(() => null);
            _uowMock.Setup(x => x.ImageRepository)
                .Returns(_imagesRepoMock.Object);
            //Act
            Task Actual() => _imagesService.AddImage(request);
            //Assert
            await Assert.ThrowsAsync<ObjectNotFoundException>(Actual);
        }

        [Fact]
        public async Task AddImage_FileSizeExceedsTheMax_ThrowsInvalidOperationException()
        {
            //Arrange
            var request = _postRequests[1];
            _userManagerMock.Setup(x => x.FindByIdAsync(request.UserId))
                .ReturnsAsync(new User { Id = request.UserId });
            _uowMock.Setup(x => x.ImageRepository)
                .Returns(_imagesRepoMock.Object);
            //Act
            Task Actual() => _imagesService.AddImage(request);
            //Assert
            await Assert.ThrowsAsync<InvalidOperationException>(Actual);
        }

        [Fact]
        public async Task AddImage_FileNameHasNoExtension_ThrowsInvalidOperationException()
        {
            //Arrange
            var request = _postRequests[2];
            _userManagerMock.Setup(x => x.FindByIdAsync(request.UserId))
                .ReturnsAsync(new User { Id = request.UserId });
            _uowMock.Setup(x => x.ImageRepository)
                .Returns(_imagesRepoMock.Object);
            //Act
            Task Actual() => _imagesService.AddImage(request);
            //Assert
            await Assert.ThrowsAsync<InvalidOperationException>(Actual);
        }

        [Fact]
        public async Task AddImage_FileNameHasUnsupportedExtension_ThrowsInvalidOperationException()
        {
            //Arrange
            var request = _postRequests[3];
            _userManagerMock.Setup(x => x.FindByIdAsync(request.UserId))
                .ReturnsAsync(new User { Id = request.UserId });
            _uowMock.Setup(x => x.ImageRepository)
                .Returns(_imagesRepoMock.Object);
            //Act
            Task Actual() => _imagesService.AddImage(request);
            //Assert
            await Assert.ThrowsAsync<InvalidOperationException>(Actual);
        }


        private static ImageFileOptions GetOptions()
        {
            return new ImageFileOptions
            {
                DirectoryPath = "any",
                MaxSize = 10485760,
                SupportedExtensions = new[] { ".png", ".jpg", ".jpeg", ".gif" },
                MaxQuality = 720
            };
        }

        private static IMapper GetMapper()
        {
            return new MapperConfiguration(cfg => cfg.AddMaps(typeof(RequestToDomain)))
                .CreateMapper();
        }

        private static Mock<UserManager<User>> MockUserManager()
        {
            var store = new Mock<IUserStore<User>>();
            var mgr = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
            mgr.Object.UserValidators.Add(new UserValidator<User>());
            mgr.Object.PasswordValidators.Add(new PasswordValidator<User>());

            return mgr;
        }
    }
}
