using AutoMapper;
using LibraryWebApp.AuthService.Application.DTOs;
using LibraryWebApp.AuthService.Domain.Entities;

namespace LibraryWebApp.AuthService.Application.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<UserDTO, User>()
                        .ForMember(dest => dest.HashedPassword, opt => opt.MapFrom(src => src.Password));
        }
    }
}
