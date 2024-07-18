using SpaManagementSystem.Application.Dto;
using SpaManagementSystem.Application.Requests.Salon;

namespace SpaManagementSystem.Application.Interfaces
{
    public interface ISalonService
    {
        public Task<SalonDetailsDto?> GetSalonDetailsByIdAsync(Guid salonId);
        public Task<IEnumerable<SalonDto>> GetAllSalonsByUserIdAsync(Guid userId);
        public Task CreateAsync(Guid userId, CreateSalonRequest createSalonRequest);
        public Task<bool> UpdateSalonAsync(Guid salonId, UpdateSalonDetailsRequest request);
        public Task UpdateOpeningHours(Guid salonId, UpdateSalonOpeningHoursRequest request);
        public Task DeleteAsync(Guid salonId);
    }
}