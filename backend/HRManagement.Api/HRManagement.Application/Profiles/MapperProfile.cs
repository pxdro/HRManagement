using AutoMapper;
using HRManagement.Application.DTOs;
using HRManagement.Domain.Entities;

namespace HRManagement.Application.Profiles
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Department, DepartmentReturnDto>();
            CreateMap<Employee, EmployeeReturnDto>();
        }
    }
}
