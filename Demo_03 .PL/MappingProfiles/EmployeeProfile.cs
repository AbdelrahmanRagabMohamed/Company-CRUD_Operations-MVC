using AutoMapper;
using Demo_03.DAL.Models;
using Demo_03_.PL.ViewModels;

namespace Demo_03_.PL.MappingProfiles
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            // Create Map
            CreateMap<Employee, EmployeeViewModel>().ReverseMap();  // (Simple Map)

            // ReverseMap() => map عشان تعكس ال 

        }
    }
}
           