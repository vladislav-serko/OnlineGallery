using AutoMapper;
using OnlineGallery.BLL.DTOs.ImageTransfer;
using OnlineGallery.BLL.DTOs.Pagination;
using OnlineGallery.BLL.DTOs.Users;
using OnlineGallery.BLL.DTOs.Users.Authentication;
using OnlineGallery.DAL.Models;
using OnlineGallery.DAL.Models.Pagination;

namespace OnlineGallery.BLL.MappingProfiles
{
    public class DomainToResponse : Profile
    {
        public DomainToResponse()
        {
            CreateMap<Image, ImageDto>();

            CreateMap<PagedData<Image>, PaginationResponse<ImageDto>>();

            CreateMap<PagedData<User>, PaginationResponse<UserWithRolesDto>>();

            CreateMap<User, AuthenticationResponse>();
            CreateMap<User, UserDto>();
            CreateMap<User, UserWithRolesDto>();
        }
    }
}