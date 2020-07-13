using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OnlineGallery.BLL.DTOs.Users.Authentication;
using OnlineGallery.BLL.Services.Interfaces;

namespace OnlineGallery.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser(RegistrationRequest request)
        {
            var user = await _accountService.Register(request);
            return Created(Url.Link("GetUser", new {user.Id}), user);
        }

        [HttpPost("login")]
        public async Task<IActionResult> AuthenticateUser(AuthenticationRequest request)
        {
            var result = await _accountService.Authenticate(request);
            return Ok(result);
        }
    }
}