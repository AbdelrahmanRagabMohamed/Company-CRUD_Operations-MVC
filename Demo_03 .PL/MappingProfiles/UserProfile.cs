using AutoMapper;
using Demo_03.DAL.Models;
using Demo_03_.PL.ViewModels;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Map ApplicationUser to UserViewModel
        CreateMap<ApplicationUser, UserViewModel>()
            .ForMember(dest => dest.Roles, opt => opt.Ignore()); // تجاهل الحقول التي تحتاج إلى معالجة خاصة
    }
}
