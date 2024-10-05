using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using SpaManagementSystem.Application.Common;
using SpaManagementSystem.Domain.Builders;
using SpaManagementSystem.Domain.Specifications;
using SpaManagementSystem.Domain.Interfaces;
using SpaManagementSystem.Domain.ValueObjects;
using SpaManagementSystem.Application.Dto;
using SpaManagementSystem.Application.Extensions;
using SpaManagementSystem.Application.Interfaces;
using SpaManagementSystem.Application.Requests.Salon;
using SpaManagementSystem.Application.Requests.Common;
using SpaManagementSystem.Application.Requests.Salon.Validators;

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
    
    public async Task<OperationResult> UpdateSalonAsync(Guid salonId, JsonPatchDocument<UpdateSalonRequest> patchDocument)
    {
        var existingSalon = await salonRepository.GetOrThrowAsync(() => salonRepository.GetByIdAsync(salonId));

        var request = mapper.Map<UpdateSalonRequest>(existingSalon);
        
        patchDocument.ApplyTo(request);

        var requestValidationResult = await new UpdateSalonRequestValidator().ValidateAsync(request);
        if (!requestValidationResult.IsValid)
        {
            var errors = requestValidationResult.Errors
                .GroupBy(error => error.PropertyName)
                .ToDictionary(
                    group => group.Key, 
                    group => group.Select(error => error.ErrorMessage).ToArray()
                );

            return OperationResult.ValidationFailed(errors);
        }
        
        if(!existingSalon.HasChanges(request))
            return OperationResult.NoChanges();

        var isUpdated = existingSalon.UpdateSalon(request.Name, request.Email, request.PhoneNumber, request.Description);
        if (!isUpdated)
            return OperationResult.NoChanges();
        
        var validationResult = new SalonSpecification().IsSatisfiedBy(existingSalon);
        if (!validationResult.IsValid)
            throw new InvalidOperationException($"Update failed: {string.Join(", ", validationResult.Errors)}");
        
        await salonRepository.SaveChangesAsync();

        return OperationResult.Success();
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
}