using SpaManagementSystem.Domain.Entities;
using SpaManagementSystem.Domain.Interfaces;
using SpaManagementSystem.Application.Dto;
using SpaManagementSystem.Application.Exceptions;
using SpaManagementSystem.Application.Interfaces;
using SpaManagementSystem.Application.Requests.Address;
using SpaManagementSystem.Application.Requests.Salon;

namespace SpaManagementSystem.Application.Services;

/// <summary>
/// Provides services related to salon management, including CRUD operations and updating opening hours.
/// </summary>
public class SalonService : ISalonService
{
    private readonly ISalonRepository _salonRepository;
    private readonly IRepository<SalonAddress> _addressRepository;
        
        
        
    /// <summary>
    /// Initializes a new instance of the <see cref="SalonService"/> with a repository for handling salon data.
    /// </summary>
    /// <param name="salonRepository">The repository used for salon data operations.</param>
    public SalonService(ISalonRepository salonRepository, IRepository<SalonAddress> addressRepository)
    {
        _salonRepository = salonRepository;
        _addressRepository = addressRepository;
    }

    
        
    /// <inheritdoc />
    public async Task<SalonDetailsDto> GetSalonDetailsByIdAsync(Guid salonId)
    {
        var salon = await _salonRepository.GetWithDetailsByIdAsync(salonId);

        if (salon == null)
            throw new NotFoundException($"Salon with ID '{salonId}' does not found.");

        var addressDto = (salon.Address == null)
            ? null
            : new AddressDto(salon.Address.Country, salon.Address.Region, salon.Address.City,
                salon.Address.PostalCode, salon.Address.Street, salon.Address.BuildingNumber);

        return new SalonDetailsDto(salon.Id, salon.Name, salon.Email, salon.PhoneNumber, salon.Description,
            addressDto, salon.OpeningHours.Select(x =>
                new OpeningHoursDto(x.DayOfWeek, x.OpeningTime, x.ClosingTime, x.IsClosed)));
    }
        
    /// <inheritdoc />
    public async Task<IEnumerable<SalonDto>> GetAllSalonsByUserIdAsync(Guid userId)
    {
        var salons = await _salonRepository.GetAllByUserIdAsync(userId);

        return salons.Select(x => new SalonDto(x.Id, x.Name));
    }
        
    /// <inheritdoc />
    public async Task CreateAsync(Guid userId, CreateSalonRequest createSalonRequest)
    {
        var salon = new Salon(Guid.NewGuid(), userId, createSalonRequest.Name, createSalonRequest.Email,
            createSalonRequest.PhoneNumber);
            
        salon.SetDefaultOpeningHours();
            
        await _salonRepository.CreateAsync(salon);
        await _salonRepository.SaveChangesAsync();
    }
        
    /// <inheritdoc />
    /// <exception cref="NotFoundException">Thrown when the salon with the specified ID is not found.</exception>
    public async Task<bool> UpdateSalonAsync(Guid salonId, UpdateSalonDetailsRequest request)
    {
        var salon = await _salonRepository.GetByIdAsync(salonId);

        if (salon == null)
            throw new NotFoundException("");
            
        var isUpdated = salon.UpdateSalon(request.Name!, request.Email!, request.PhoneNumber!, request.Description);
            
        if (isUpdated)
        {
            _salonRepository.Update(salon);
            await _salonRepository.SaveChangesAsync();
        }

        return isUpdated;
    }
        
    /// <inheritdoc />
    /// <exception cref="NotFoundException">Thrown when the salon with the specified ID is not found.</exception>
    public async Task UpdateOpeningHours(Guid salonId, UpdateSalonOpeningHoursRequest request)
    {
        var salon = await _salonRepository.GetByIdAsync(salonId);
            
        if (salon == null)
            throw new NotFoundException($"The salon with id {salonId} was not found.");

        foreach (var openingHours in request.OpeningHours)
            salon.AddOrUpdateOpeningHours(openingHours.DayOfWeek, openingHours.OpeningTime,
                openingHours.ClosingTime, openingHours.IsClosed);
            
        _salonRepository.Update(salon);
        await _salonRepository.SaveChangesAsync();
    }
        
    /// <inheritdoc />
    /// <exception cref="NotFoundException">Thrown when the salon with the specified ID is not found.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the salon already has an address.</exception>
    public async Task AddAddress(Guid salonId, CreateAddressRequest request)
    {
        var salon = await _salonRepository.GetWithDetailsByIdAsync(salonId);
            
        if (salon == null)
            throw new NotFoundException($"The salon with id {salonId} was not found.");

        if (salon.Address != null)
            throw new InvalidOperationException($"Salon with ID '{salonId}' already has address.");

        var address = new SalonAddress(Guid.NewGuid(), salonId, request.Country, request.Region, request.City,
            request.PostalCode, request.Street, request.BuildingNumber);

        await _addressRepository.CreateAsync(address);
        await _addressRepository.SaveChangesAsync();
    }
        
    /// <inheritdoc />
    /// <exception cref="NotFoundException">Thrown when the salon with the specified ID is not found.</exception>
    /// <exception cref="NotFoundException">Thrown when the address for salon with the specified ID is not found.</exception>
    public async Task<bool> UpdateAddress(Guid salonId, UpdateAddressRequest request)
    {
        var salon = await _salonRepository.GetWithDetailsByIdAsync(salonId);
            
        if (salon == null)
            throw new NotFoundException($"The salon with id {salonId} was not found.");

        if (salon.Address == null)
            throw new NotFoundException($"The address for the salon with id {salonId} was not found.");

        var isUpdated = salon.Address.UpdateAddress(request.Country, request.Region, request.City, request.PostalCode,
            request.Street, request.BuildingNumber);
            
        if (isUpdated)
        {
            _salonRepository.Update(salon);
            await _salonRepository.SaveChangesAsync();
        }

        return isUpdated;
    }
        
    /// <inheritdoc />
    /// <exception cref="NotFoundException">Thrown when the salon with the specified ID is not found.</exception>
    public async Task DeleteAsync(Guid salonId)
    {
        var salon = await _salonRepository.GetByIdAsync(salonId);
            
        if (salon == null)
            throw new NotFoundException($"The salon with id {salonId} was not found.");

        _salonRepository.Delete(salon);
        await _salonRepository.SaveChangesAsync();
    }
}