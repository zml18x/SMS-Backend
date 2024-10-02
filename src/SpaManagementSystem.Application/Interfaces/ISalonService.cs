using SpaManagementSystem.Application.Dto;
using SpaManagementSystem.Application.Requests.Salon;
using SpaManagementSystem.Application.Requests.Common;

namespace SpaManagementSystem.Application.Interfaces;

public interface ISalonService
{
    public Task<SalonDetailsDto> GetSalonDetailsByIdAsync(Guid salonId);
    public Task<IEnumerable<SalonDto>> GetAllSalonsByUserIdAsync(Guid userId);
    public Task<SalonDetailsDto> CreateAsync(Guid userId, CreateSalonRequest createSalonRequest);
    public Task UpdateSalonAsync(Guid salonId, UpdateSalonDetailsRequest request);
    public Task AddOpeningHoursAsync(Guid salonId, OpeningHoursRequest request);
    public Task UpdateOpeningHoursAsync(Guid salonId, OpeningHoursRequest request);
    public Task RemoveOpeningHoursAsync(Guid salonId, DayOfWeek dayOfWeek);
    public Task UpdateAddressAsync(Guid salonId, UpdateAddressRequest request);
    public Task DeleteAsync(Guid salonId);
    public bool HasChanges(SalonDetailsDto existingSalon, UpdateSalonDetailsRequest request);
}