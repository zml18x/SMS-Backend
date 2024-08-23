using AutoMapper;
using SpaManagementSystem.Domain.Entities;
using SpaManagementSystem.Domain.Interfaces;
using SpaManagementSystem.Domain.ValueObjects;
using SpaManagementSystem.Application.Dto;
using SpaManagementSystem.Application.Interfaces;
using SpaManagementSystem.Application.Requests.Salon;
using SpaManagementSystem.Application.Requests.Common;
using SpaManagementSystem.Application.Extensions.RepositoryExtensions;

namespace SpaManagementSystem.Application.Services;

public class SalonService(ISalonRepository salonRepository, IMapper mapper) : ISalonService
{
    public async Task<SalonDetailsDto> GetSalonDetailsByIdAsync(Guid salonId)
    {
        var salon = await salonRepository.GetByIdOrFailAsync(salonId);

        return mapper.Map<SalonDetailsDto>(salon);
    }
    
    public async Task<IEnumerable<SalonDto>> GetAllSalonsByUserIdAsync(Guid userId)
    {
        var salons = await salonRepository.GetAllByUserIdAsync(userId);
  
        return mapper.Map<IEnumerable<SalonDto>>(salons);
    }
    
    public async Task CreateAsync(Guid userId, CreateSalonRequest createSalonRequest)
    {
        var salon = new Salon(Guid.NewGuid(), userId, createSalonRequest.Name, createSalonRequest.Email,
            createSalonRequest.PhoneNumber);
            
        await salonRepository.CreateAsync(salon);
        await salonRepository.SaveChangesAsync();
    }
    
    public async Task<bool> UpdateSalonAsync(Guid salonId, UpdateSalonDetailsRequest request)
    {
        var salon = await salonRepository.GetByIdOrFailAsync(salonId);
            
        var isUpdated = salon.UpdateSalon(request.Name, request.Email, request.PhoneNumber, request.Description);
            
        if (isUpdated)
        {
            salonRepository.Update(salon);
            await salonRepository.SaveChangesAsync();
        }

        return isUpdated;
    }
    
    public async Task AddOpeningHoursAsync(Guid salonId, OpeningHoursRequest request)
    {
        var salon = await salonRepository.GetByIdOrFailAsync(salonId);

        salon.AddOpeningHours(new OpeningHours(request.DayOfWeek, request.OpeningTime, request.ClosingTime));

        salonRepository.Update(salon);
        await salonRepository.SaveChangesAsync();
    }
    
    public async Task UpdateOpeningHoursAsync(Guid salonId, OpeningHoursRequest request)
    {
        var salon = await salonRepository.GetByIdOrFailAsync(salonId);

        salon.UpdateOpeningHours(new OpeningHours(request.DayOfWeek, request.OpeningTime, request.ClosingTime));
        salonRepository.Update(salon);
        await salonRepository.SaveChangesAsync();
    }
    
    public async Task RemoveOpeningHoursAsync(Guid salonId, DayOfWeek dayOfWeek)
    {
        var salon = await salonRepository.GetByIdOrFailAsync(salonId);
        
        salon.RemoveOpeningHours(dayOfWeek);
        salonRepository.Update(salon);
        await salonRepository.SaveChangesAsync();
    }
    
    public async Task UpdateAddressAsync(Guid salonId, UpdateAddressRequest request)
    {
        var salon = await salonRepository.GetByIdOrFailAsync(salonId);

        var address = new Address(request.Country, request.Region, request.City,
            request.PostalCode, request.Street, request.BuildingNumber);

        salon.UpdateAddress(address);

        salonRepository.Update(salon);
        await salonRepository.SaveChangesAsync();
    }
    
    public async Task DeleteAsync(Guid salonId)
    {
        var salon = await salonRepository.GetByIdOrFailAsync(salonId);

        salonRepository.Delete(salon);
        await salonRepository.SaveChangesAsync();
    }
}