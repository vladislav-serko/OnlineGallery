using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using OnlineGallery.BLL.Exceptions;

namespace OnlineGallery.Api.Extensions
{
    public static class ControllerBaseExtensions
    {
        public static string GetUserId(this ControllerBase controller)
        {
            return controller.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public static void CheckUser(this ControllerBase controller, string id)
        {
            if (controller.GetUserId() != id)
                throw new UserIdMismatchException(controller.GetUserId(), id);
        }
    }
}