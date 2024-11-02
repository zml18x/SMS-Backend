using AutoMapper;
using SpaManagementSystem.Domain.Entities;
using SpaManagementSystem.Domain.ValueObjects;
using SpaManagementSystem.Application.Dto;
using SpaManagementSystem.Application.Requests.Employee;
using SpaManagementSystem.Application.Requests.Product;
using SpaManagementSystem.Application.Requests.Salon;
using SpaManagementSystem.Application.Requests.Service;
using SpaManagementSystem.Domain.Enums;

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
        CreateMap<Employee, EmployeeSummaryDto>()
            .ConstructUsing(src => new EmployeeSummaryDto(
                src.Id,
                src.SalonId,
                src.Position,
                src.EmploymentStatus,
                src.Code,
                src.Color,
                src.Profile.FirstName,
                src.Profile.LastName,
                src.Profile.Gender,
                src.Profile.Email,
                src.Profile.PhoneNumber
            ));

        CreateMap<Service, ServiceDto>();
        
        CreateMap<Product, ProductDto>()
            .ForMember(dest => dest.PurchasePriceWithTax, 
                opt => opt.MapFrom(src => src.PurchasePriceWithTax))
            .ForMember(dest => dest.SalePriceWithTax, 
                opt => opt.MapFrom(src => src.SalePriceWithTax));

        CreateMap<Salon, UpdateSalonRequest>();
            
        CreateMap<Employee, UpdateEmployeeRequest>();
        CreateMap<Employee, UpdateEmployeeSelfRequest>();
        CreateMap<EmployeeProfile, UpdateEmployeeProfileRequest>();
        CreateMap<EmployeeProfile, UpdateEmployeeProfileSelfRequest>();
        
        CreateMap<Service, UpdateServiceRequest>();
        
        CreateMap<Product, UpdateProductRequest>();
    }
}