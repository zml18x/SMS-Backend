using AutoMapper;
using SpaManagementSystem.Domain.Entities;
using SpaManagementSystem.Domain.ValueObjects;
using SpaManagementSystem.Application.Dto;
using SpaManagementSystem.Application.Requests.Employee;
using SpaManagementSystem.Application.Requests.Salon;

namespace SpaManagementSystem.Application.Mappers;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Salon, SalonDto>();
        CreateMap<Salon, SalonDetailsDto>();
        CreateMap<OpeningHours, OpeningHoursDto>();
        CreateMap<Address, AddressDto>();
        CreateMap<Employee, EmployeeDto>();
        CreateMap<EmployeeProfile, EmployeeProfileDto>();
        CreateMap<Employee, EmployeeDetailsDto>().ConstructUsing((e, x) =>
            new EmployeeDetailsDto(x.Mapper.Map<EmployeeDto>(e), x.Mapper.Map<EmployeeProfileDto>(e.Profile)));
        
        CreateMap<Salon, UpdateSalonRequest>();
        CreateMap<Employee, UpdateEmployeeRequest>();
        CreateMap<Employee, UpdateEmployeeSelfRequest>();
        CreateMap<EmployeeProfile, UpdateEmployeeProfileRequest>();
        CreateMap<EmployeeProfile, UpdateEmployeeProfileSelfRequest>();
    }
}