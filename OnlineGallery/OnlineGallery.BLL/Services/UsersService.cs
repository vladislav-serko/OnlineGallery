using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using OnlineGallery.BLL.DTOs.Pagination;
using OnlineGallery.BLL.DTOs.Users;
using OnlineGallery.BLL.Exceptions;
using OnlineGallery.BLL.Helpers.Options;
using OnlineGallery.BLL.Services.Interfaces;
using OnlineGallery.DAL.Extensions;
using OnlineGallery.DAL.FileWork;
using OnlineGallery.DAL.Models;
using OnlineGallery.DAL.Models.Pagination;
using OnlineGallery.DAL.UnitOfWork;

namespace OnlineGallery.BLL.Services
{
    public class UsersService : IUsersService
    {
        private readonly ImageFileOptions _fileOptions;
        private readonly IImageProvider _imageProvider;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;

        public UsersService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IImageProvider imageProvider,
            IOptions<ImageFileOptions> fileOptions,
            UserManager<User> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _imageProvider = imageProvider;
            _userManager = userManager;
            _fileOptions = fileOptions.Value;
        }

        public async Task<UserDto> GetUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                throw new ObjectNotFoundException($"user with id {id} not found");

            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> UpdateUserInformation(UserUpdateRequest request)
        {
            var user = await _userManager.FindByIdAsync(request.Id);
            user = _mapper.Map(request, user);
            await _userManager.UpdateAsync(user);
            return _mapper.Map<UserDto>(user);
        }

        public async Task DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                throw new ObjectNotFoundException($"user with id {id} not found");
            var images = await _unitOfWork.ImageRepository.GetImagesByUser(id);

            var path = Path.Combine(_fileOptions.DirectoryPath, id);
            _imageProvider.DeleteDirectory(path);

            await _userManager.DeleteAsync(user);
        }

        public async Task UserToModerator(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var result = await _userManager.AddToRoleAsync(user, "Moderator");
            if (!result.Succeeded) throw new IdentityException(result.Errors);
        }

        public async Task ModeratorToUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var result = await _userManager.RemoveFromRoleAsync(user, "Moderator");
            if (!result.Succeeded) throw new IdentityException(result.Errors);
        }

        public async Task<PaginationResponse<UserWithRolesDto>> SearchUsersByName(string query,
            PaginationRequest request)
        {
            var options = _mapper.Map<PaginationOptions>(request);
            var pagedData = await _userManager.SearchByUsername(query, options);

            return await ConvertToDtoWithRoles(pagedData);
        }

        private async Task<PaginationResponse<UserWithRolesDto>> ConvertToDtoWithRoles(PagedData<User> users)
        {
            var paginationResponse = _mapper.Map<PaginationResponse<UserWithRolesDto>>(users);

            for (var i = 0; i < users.Data.Count(); i++)
            {
                var roles = await _userManager.GetRolesAsync(users.Data[i]);
                paginationResponse.Data[i].Roles = roles;
            }

            return paginationResponse;
        }
    }
}