using System.Threading.Tasks;
using OnlineGallery.BLL.DTOs.Pagination;
using OnlineGallery.BLL.DTOs.Users;

namespace OnlineGallery.BLL.Services.Interfaces
{
    public interface IUsersService
    {
        Task<UserDto> GetUser(string id);
        Task<UserDto> UpdateUserInformation(UserUpdateRequest request);
        Task DeleteUser(string id);
        Task UserToModerator(string id);
        Task ModeratorToUser(string id);
        Task<PaginationResponse<UserWithRolesDto>> SearchUsersByName(string username, PaginationRequest request);
    }
}