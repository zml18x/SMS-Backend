using SpaManagementSystem.Application.Dto;
using SpaManagementSystem.Application.Requests.Address;
using SpaManagementSystem.Application.Requests.Salon;

namespace SpaManagementSystem.Application.Interfaces
{
    /// <summary>
    /// Defines the operations for managing salon-related data within the application.
    /// </summary>
    public interface ISalonService
    {
        /// <summary>
        /// Retrieves the detailed information about a salon by its unique identifier.
        /// </summary>
        /// <param name="salonId">The unique identifier of the salon.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the SalonDetailsDto, or null if not found.</returns>
        public Task<SalonDetailsDto> GetSalonDetailsByIdAsync(Guid salonId);

        /// <summary>
        /// Retrieves a collection of salons associated with a specific user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains an enumerable collection of SalonDto.</returns>
        public Task<IEnumerable<SalonDto>> GetAllSalonsByUserIdAsync(Guid userId);

        /// <summary>
        /// Creates a new salon associated with a specific user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user who owns the salon.</param>
        /// <param name="createSalonRequest">The request containing the details of the salon to create.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public Task CreateAsync(Guid userId, CreateSalonRequest createSalonRequest);

        /// <summary>
        /// Updates the details of an existing salon.
        /// </summary>
        /// <param name="salonId">The unique identifier of the salon to update.</param>
        /// <param name="request">The request containing the updated details of the salon.</param>
        /// <returns>A task that represents the asynchronous operation. The task result indicates whether the update was successful.</returns>
        public Task<bool> UpdateSalonAsync(Guid salonId, UpdateSalonDetailsRequest request);

        /// <summary>
        /// Updates the opening hours of an existing salon.
        /// </summary>
        /// <param name="salonId">The unique identifier of the salon for which to update opening hours.</param>
        /// <param name="request">The request containing the updated opening hours.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public Task UpdateOpeningHours(Guid salonId, UpdateSalonOpeningHoursRequest request);
        
        /// <summary>
        /// Adds a new address to the specified salon.
        /// </summary>
        /// <param name="salonId">The unique identifier of the salon to which the address will be added.</param>
        /// <param name="request">The request containing the details of the address to add.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public Task AddAddress(Guid salonId, CreateAddressRequest request);
        
        /// <summary>
        /// Updates the address of an existing salon.
        /// </summary>
        /// <param name="salonId">The unique identifier of the salon whose address will be updated.</param>
        /// <param name="request">The request containing the updated address details.</param>
        /// <returns>A task that represents the asynchronous operation.
        /// The task result indicates whether the update was successful.</returns>
        public Task<bool> UpdateAddress(Guid salonId, UpdateAddressRequest request);

        /// <summary>
        /// Deletes an existing salon by its unique identifier.
        /// </summary>
        /// <param name="salonId">The unique identifier of the salon to delete.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public Task DeleteAsync(Guid salonId);
    }
}