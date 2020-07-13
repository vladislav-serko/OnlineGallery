using System.Threading.Tasks;
using OnlineGallery.BLL.DTOs.Users;
using OnlineGallery.BLL.DTOs.Users.Authentication;

namespace OnlineGallery.BLL.Services.Interfaces
{
    public interface IAccountService
    {
        Task<UserDto> Register(RegistrationRequest request);
        Task<AuthenticationResponse> Authenticate(AuthenticationRequest request);
    }
}