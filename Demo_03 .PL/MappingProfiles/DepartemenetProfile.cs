using AutoMapper;
using Demo_03.DAL.Models;
using Demo_03_.PL.ViewModels;

namespace Demo_03_.PL.MappingProfiles
{
    public class DepartemenetProfile : Profile
    {
        public DepartemenetProfile()
        {
            // Create Map
            CreateMap<Departement, DepartementViewModel>().ReverseMap(); // (Simple Map)

            // ReverseMap() => map عشان تعكس ال 

        }
    }
}
