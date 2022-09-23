using AutoMapper;
using Fruitkha.Core.Dtos.Catalog;
using Fruitkha.Core.Dtos.User;
using Fruitkha.Core.Entities;

namespace Fruitkha.Core.Helpers
{
    public class ApplicationProfile : Profile
    {
        public ApplicationProfile()
        {
            CreateMap<User, UserPersonalInfoDto>()
                .ForMember(dest => dest.Email, act => act.MapFrom(src => src.Email))
                .ForMember(dest => dest.FirstName, act => act.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, act => act.MapFrom(src => src.LastName))
                .ForMember(dest => dest.UserName, act => act.MapFrom(src => src.UserName));

            CreateMap<UserChangeInfoDto, User>();
            CreateMap<Category, CategoryDto>();
        }
    }
}
