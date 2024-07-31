using SpaManagementSystem.Application.Dto;
using SpaManagementSystem.Application.Requests.Salon;
using SpaManagementSystem.Application.Requests.Common;

namespace SpaManagementSystem.Application.Interfaces;

/// <summary>
/// Defines the operations for managing salon-related data within the application.
/// </summary>
public interface ISalonService
{
    /// <summary>
    /// Retrieves the detailed information about a salon by its unique identifier.
    /// </summary>
    /// <param name="salonId">The unique identifier of the salon.</param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains a <see cref="SalonDetailsDto"/> object with the salon details.
    /// </returns>
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
    /// Adds opening hours to a specific salon.
    /// </summary>
    /// <param name="salonId">The unique identifier of the salon to which the opening hours will be added.</param>
    /// <param name="request">The request containing the details of the opening hours to add.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public Task AddOpeningHoursAsync(Guid salonId, OpeningHoursRequest request);
    
    /// <summary>
    /// Updates the opening hours of a specific salon.
    /// </summary>
    /// <param name="salonId">The unique identifier of the salon whose opening hours will be updated.</param>
    /// <param name="request">The request containing the updated opening hours details.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public Task UpdateOpeningHoursAsync(Guid salonId, OpeningHoursRequest request);
    
    /// <summary>
    /// Removes the opening hours for a specific day of the week from a salon.
    /// </summary>
    /// <param name="salonId">The unique identifier of the salon from which the opening hours will be removed.</param>
    /// <param name="dayOfWeek">The day of the week for which the opening hours will be removed.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public Task RemoveOpeningHoursAsync(Guid salonId, DayOfWeek dayOfWeek);
    
    /// <summary>
    /// Updates the address of a specific salon.
    /// </summary>
    /// <param name="salonId">The unique identifier of the salon whose address will be updated.</param>
    /// <param name="request">The request containing the updated address details.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public Task UpdateAddressAsync(Guid salonId, UpdateAddressRequest request);

    /// <summary>
    /// Deletes an existing salon by its unique identifier.
    /// </summary>
    /// <param name="salonId">The unique identifier of the salon to delete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public Task DeleteAsync(Guid salonId);
}