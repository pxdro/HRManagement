using AutoMapper;
using HRManagement.Application.DTOs;
using HRManagement.Domain.Entities;

namespace HRManagement.Application.Profiles
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Department, DepartmentReturnDto>()
                .ForMember(dest => dest.RowVersion, opt => opt.MapFrom(src => Convert.ToBase64String(src.RowVersion)));
            CreateMap<Employee, EmployeeReturnDto>()
                .ForMember(dest => dest.RowVersion, opt => opt.MapFrom(src => Convert.ToBase64String(src.RowVersion)));
        }
    }
}
