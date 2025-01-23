using AutoMapper;
using Demo_03_.PL.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace Demo_03_.PL.MappingProfiles
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<IdentityRole, RoleViewModel>()
                .ForMember(d => d.RoleName , O => O.MapFrom(S => S.Name))  // To Bind Name in RoleName
                .ReverseMap();

                
        }
    }
}
