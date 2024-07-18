using SpaManagementSystem.Domain.Entities;
using SpaManagementSystem.Domain.Interfaces;
using SpaManagementSystem.Application.Dto;
using SpaManagementSystem.Application.Exceptions;
using SpaManagementSystem.Application.Interfaces;
using SpaManagementSystem.Application.Requests.Salon;

namespace SpaManagementSystem.Application.Services
{
    public class SalonService : ISalonService
    {
        private readonly ISalonRepository _salonRepository;
        
        
        
        public SalonService(ISalonRepository salonRepository)
        {
            _salonRepository = salonRepository;
        }



        public async Task<SalonDetailsDto?> GetSalonDetailsByIdAsync(Guid salonId)
        {
            var salon = await _salonRepository.GetByIdAsync(salonId);

            if (salon == null)
                return null;

            return new SalonDetailsDto(salon.Id, salon.Name, salon.Email, salon.PhoneNumber, salon.Description,
                salon.OpeningHours.Select(x =>
                    new OpeningHoursDto(x.DayOfWeek, x.OpeningTime, x.ClosingTime, x.IsClosed)));
        }
        
        public async Task<IEnumerable<SalonDto>> GetAllSalonsByUserIdAsync(Guid userId)
        {
            var salons = await _salonRepository.GetAllByUserIdAsync(userId);

            return salons.Select(x => new SalonDto(x.Id, x.Name));
        }
        
        public async Task CreateAsync(Guid userId, CreateSalonRequest createSalonRequest)
        {
            var salon = new Salon(Guid.NewGuid(), userId, createSalonRequest.Name, createSalonRequest.Email,
                createSalonRequest.PhoneNumber);
            
            salon.SetDefaultOpeningHours();
            
            await _salonRepository.CreateAsync(salon);
            await _salonRepository.SaveChangesAsync();
        }
        
        public async Task<bool> UpdateSalonAsync(Guid salonId, UpdateSalonDetailsRequest request)
        {
            var salon = await _salonRepository.GetByIdAsync(salonId);

            if (salon == null)
                throw new NotFoundException($"The salon with id {salonId} was not found.");

            var isUpdated = salon.UpdateSalon(request.Name!, request.Email!, request.PhoneNumber!, request.Description);
            
            if (isUpdated)
            {
                _salonRepository.Update(salon);
                await _salonRepository.SaveChangesAsync();
            }

            return isUpdated;
        }

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

        public async Task DeleteAsync(Guid salonId)
        {
            var salon = await _salonRepository.GetByIdAsync(salonId);
            
            if (salon == null)
                throw new NotFoundException($"The salon with id {salonId} was not found.");

            _salonRepository.Delete(salon);
            await _salonRepository.SaveChangesAsync();
        }
    }
}