using AutoMapper;
using SpaManagementSystem.Domain.Builders;
using SpaManagementSystem.Domain.Specifications;
using SpaManagementSystem.Domain.Interfaces;
using SpaManagementSystem.Domain.ValueObjects;
using SpaManagementSystem.Application.Dto;
using SpaManagementSystem.Application.Extensions;
using SpaManagementSystem.Application.Interfaces;
using SpaManagementSystem.Application.Requests.Salon;
using SpaManagementSystem.Application.Requests.Common;
using SpaManagementSystem.Application.Requests.Employee;

namespace SpaManagementSystem.Application.Services;

public class SalonService(ISalonRepository salonRepository, IMapper mapper, SalonBuilder salonBuilder, AddressBuilder addressBuilder) : ISalonService
{
    public async Task<SalonDetailsDto> GetSalonDetailsByIdAsync(Guid salonId)
    {
        var salon = await salonRepository.GetOrThrowAsync(() => salonRepository.GetByIdAsync(salonId));

        return mapper.Map<SalonDetailsDto>(salon);
    }
    
    public async Task<IEnumerable<SalonDto>> GetAllSalonsByUserIdAsync(Guid userId)
    {
        var salons = await salonRepository.GetAllByUserIdAsync(userId);
  
        return mapper.Map<IEnumerable<SalonDto>>(salons);
    }
    
    public async Task<SalonDetailsDto> CreateAsync(Guid userId, CreateSalonRequest createSalonRequest)
    {
        var salon = salonBuilder
            .WithSalonId(Guid.NewGuid())
            .WithUserId(userId)
            .WithName(createSalonRequest.Name)
            .WithEmail(createSalonRequest.Email)
            .WithPhoneNumber(createSalonRequest.PhoneNumber)
            .Build();
            
        await salonRepository.CreateAsync(salon);
        await salonRepository.SaveChangesAsync();

        return mapper.Map<SalonDetailsDto>(salon);
    }
    
    public async Task UpdateSalonAsync(Guid salonId, UpdateSalonDetailsRequest request)
    {
        var salon = await salonRepository.GetOrThrowAsync(() => salonRepository.GetByIdAsync(salonId));
            
        var isUpdated = salon.UpdateSalon(request.Name, request.Email, request.PhoneNumber, request.Description);

        if (isUpdated)
        {
            var validationResult = new SalonSpecification().IsSatisfiedBy(salon);
            if (!validationResult.IsValid)
                throw new InvalidOperationException($"Update failed: {string.Join(", ", validationResult.Errors)}");
            
            await salonRepository.SaveChangesAsync();
        }
    }
    
    public async Task AddOpeningHoursAsync(Guid salonId, OpeningHoursRequest request)
    {
        var salon = await salonRepository.GetOrThrowAsync(() => salonRepository.GetByIdAsync(salonId));

        salon.AddOpeningHours(new OpeningHours(request.DayOfWeek, request.OpeningTime, request.ClosingTime));
        
        await salonRepository.SaveChangesAsync();
    }
    
    public async Task UpdateOpeningHoursAsync(Guid salonId, OpeningHoursRequest request)
    {
        var salon = await salonRepository.GetOrThrowAsync(() => salonRepository.GetByIdAsync(salonId));

        salon.UpdateOpeningHours(new OpeningHours(request.DayOfWeek, request.OpeningTime, request.ClosingTime));

        await salonRepository.SaveChangesAsync();
    }
    
    public async Task RemoveOpeningHoursAsync(Guid salonId, DayOfWeek dayOfWeek)
    {
        var salon = await salonRepository.GetOrThrowAsync(() => salonRepository.GetByIdAsync(salonId));
        
        salon.RemoveOpeningHours(dayOfWeek);

        await salonRepository.SaveChangesAsync();
    }
    
    public async Task UpdateAddressAsync(Guid salonId, UpdateAddressRequest request)
    {
        var salon = await salonRepository.GetOrThrowAsync(() => salonRepository.GetByIdAsync(salonId));

        var address = addressBuilder
            .WithCountry(request.Country)
            .WithRegion(request.Region)
            .WithCity(request.City)
            .WithPostalCode(request.PostalCode)
            .WithStreet(request.Street)
            .WithBuildingNumber(request.BuildingNumber)
            .Build();

        salon.SetAddress(address);
        
        await salonRepository.SaveChangesAsync();
    }
    
    public async Task DeleteAsync(Guid salonId)
    {
        var salon = await salonRepository.GetOrThrowAsync(() => salonRepository.GetByIdAsync(salonId));

        salonRepository.Delete(salon);
        await salonRepository.SaveChangesAsync();
    }

    public bool HasChanges(EmployeeDto existingEmployee, UpdateEmployeeRequest request)
    {
        throw new NotImplementedException();
    }

    public bool HasChanges(SalonDetailsDto existingSalon, UpdateSalonDetailsRequest request)
        => existingSalon.Name != request.Name ||
           existingSalon.Email != request.Email ||
           existingSalon.PhoneNumber != request.PhoneNumber ||
           existingSalon.Description != request.Description;
}