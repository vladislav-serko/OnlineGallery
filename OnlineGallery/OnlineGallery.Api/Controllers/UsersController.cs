using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineGallery.Api.Extensions;
using OnlineGallery.BLL.DTOs.Pagination;
using OnlineGallery.BLL.DTOs.Users;
using OnlineGallery.BLL.Services.Interfaces;

namespace OnlineGallery.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;

        public UsersController(IUsersService usersService)
        {
            _usersService = usersService;
        }

        [Route("{id}", Name = "GetUser")]
        [HttpGet]
        public async Task<IActionResult> GetUser(string id)
        {
            var user = await _usersService.GetUser(id);
            return Ok(user);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser(UserUpdateRequest request)
        {
            this.CheckUser(request.Id);

            await _usersService.UpdateUserInformation(request);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            if (!User.IsInRole("Admin"))
                this.CheckUser(id);

            await _usersService.DeleteUser(id);
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("search")]
        public async Task<IActionResult> SearchUsers([Required] string query, [Required] int page,
            [Required] int itemCount)
        {
            var request = new PaginationRequest {ItemCount = itemCount, Page = page};
            var response = await _usersService.SearchUsersByName(query, request);
            return Ok(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("{id}/toModerator")]
        public async Task<IActionResult> UserToModerator(string id)
        {
            await _usersService.UserToModerator(id);
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("{id}/toUser")]
        public async Task<IActionResult> ModeratorToUser(string id)
        {
            await _usersService.ModeratorToUser(id);
            return NoContent();
        }
    }
}