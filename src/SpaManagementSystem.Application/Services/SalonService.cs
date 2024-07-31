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

/// <summary>
/// Service responsible for salon managing. Implements <see cref="ISalonService"/>.
/// </summary>
public class SalonService : ISalonService
{
    private readonly ISalonRepository _salonRepository;
    private readonly IMapper _mapper;
        
        
        
    /// <summary>
    /// Initializes a new instance of the <see cref="SalonService"/> with a repository for handling salon data and a mapper for object-object mapping.
    /// </summary>
    /// <param name="salonRepository">The repository used for salon data operations.</param>
    /// <param name="mapper">The mapper to handle object-object mapping.</param>
    public SalonService(ISalonRepository salonRepository, IMapper mapper)
    {
        _salonRepository = salonRepository;
        _mapper = mapper;
    }

    
        
    /// <inheritdoc />
    public async Task<SalonDetailsDto> GetSalonDetailsByIdAsync(Guid salonId)
    {
        var salon = await _salonRepository.GetByIdOrFailAsync(salonId);

        return _mapper.Map<SalonDetailsDto>(salon);
    }
        
    /// <inheritdoc />
    public async Task<IEnumerable<SalonDto>> GetAllSalonsByUserIdAsync(Guid userId)
    {
        var salons = await _salonRepository.GetAllByUserIdAsync(userId);
  
        return _mapper.Map<IEnumerable<SalonDto>>(salons);
    }
        
    /// <inheritdoc />
    public async Task CreateAsync(Guid userId, CreateSalonRequest createSalonRequest)
    {
        var salon = new Salon(Guid.NewGuid(), userId, createSalonRequest.Name, createSalonRequest.Email,
            createSalonRequest.PhoneNumber);
            
        await _salonRepository.CreateAsync(salon);
        await _salonRepository.SaveChangesAsync();
    }
        
    /// <inheritdoc />
    public async Task<bool> UpdateSalonAsync(Guid salonId, UpdateSalonDetailsRequest request)
    {
        var salon = await _salonRepository.GetByIdOrFailAsync(salonId);
            
        var isUpdated = salon.UpdateSalon(request.Name, request.Email, request.PhoneNumber, request.Description);
            
        if (isUpdated)
        {
            _salonRepository.Update(salon);
            await _salonRepository.SaveChangesAsync();
        }

        return isUpdated;
    }
    
    /// <inheritdoc />
    public async Task AddOpeningHoursAsync(Guid salonId, OpeningHoursRequest request)
    {
        var salon = await _salonRepository.GetByIdOrFailAsync(salonId);

        salon.AddOpeningHours(new OpeningHours(request.DayOfWeek, request.OpeningTime, request.ClosingTime));

        _salonRepository.Update(salon);
        await _salonRepository.SaveChangesAsync();
    }
    
    /// <inheritdoc />
    public async Task UpdateOpeningHoursAsync(Guid salonId, OpeningHoursRequest request)
    {
        var salon = await _salonRepository.GetByIdOrFailAsync(salonId);

        salon.UpdateOpeningHours(new OpeningHours(request.DayOfWeek, request.OpeningTime, request.ClosingTime));
        _salonRepository.Update(salon);
        await _salonRepository.SaveChangesAsync();
    }
    
    /// <inheritdoc />
    public async Task RemoveOpeningHoursAsync(Guid salonId, DayOfWeek dayOfWeek)
    {
        var salon = await _salonRepository.GetByIdOrFailAsync(salonId);
        
        salon.RemoveOpeningHours(dayOfWeek);
        _salonRepository.Update(salon);
        await _salonRepository.SaveChangesAsync();
    }
    
    /// <inheritdoc />
    public async Task UpdateAddressAsync(Guid salonId, UpdateAddressRequest request)
    {
        var salon = await _salonRepository.GetByIdOrFailAsync(salonId);

        var address = new Address(request.Country, request.Region, request.City,
            request.PostalCode, request.Street, request.BuildingNumber);

        salon.UpdateAddress(address);

        _salonRepository.Update(salon);
        await _salonRepository.SaveChangesAsync();
    }
        
    /// <inheritdoc />
    public async Task DeleteAsync(Guid salonId)
    {
        var salon = await _salonRepository.GetByIdOrFailAsync(salonId);

        _salonRepository.Delete(salon);
        await _salonRepository.SaveChangesAsync();
    }
}