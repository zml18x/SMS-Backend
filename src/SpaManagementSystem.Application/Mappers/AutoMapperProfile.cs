using AutoMapper;
using SpaManagementSystem.Domain.Entities;
using SpaManagementSystem.Domain.ValueObjects;
using SpaManagementSystem.Application.Dto;

namespace SpaManagementSystem.Application.Mappers;

/// <summary>
/// Defines the mapping profile for AutoMapper, mapping domain entities/value objects to data transfer objects (DTOs).
/// </summary>
public class AutoMapperProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AutoMapperProfile"/> class and configures the mappings.
    /// </summary>
    public AutoMapperProfile()
    {
        CreateMap<Salon, SalonDto>();
        CreateMap<Salon, SalonDetailsDto>();
        CreateMap<OpeningHours, OpeningHoursDto>();
        CreateMap<Address, AddressDto>();
    }
}