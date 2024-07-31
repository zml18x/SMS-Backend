using AutoMapper;
using SpaManagementSystem.Application.Dto;
using SpaManagementSystem.Domain.Entities;
using SpaManagementSystem.Domain.ValueObjects;

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
        CreateMap<UserProfile, UserProfileDto>();
        CreateMap<Salon, SalonDto>();
        CreateMap<Salon, SalonDetailsDto>();
        CreateMap<OpeningHours, OpeningHoursDto>();
        CreateMap<Address, AddressDto>();
    }
}