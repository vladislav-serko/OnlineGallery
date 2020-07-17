using AutoMapper;
using OnlineGallery.BLL.DTOs.ImageTransfer;
using OnlineGallery.BLL.DTOs.Pagination;
using OnlineGallery.BLL.DTOs.Users;
using OnlineGallery.BLL.DTOs.Users.Authentication;
using OnlineGallery.DAL.Models;
using OnlineGallery.DAL.Models.Pagination;

namespace OnlineGallery.BLL.Helpers.MappingProfiles
{
    public class RequestToDomain : Profile
    {
        public RequestToDomain()
        {
            CreateMap<RegistrationRequest, User>();

            CreateMap<UserUpdateRequest, User>();

            CreateMap<ImagePostRequest, Image>();
            CreateMap<ImageUpdateRequest, Image>();

            CreateMap<PaginationRequest, PaginationOptions>();
        }
    }
}