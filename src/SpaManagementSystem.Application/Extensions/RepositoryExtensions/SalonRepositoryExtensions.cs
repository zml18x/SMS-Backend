using SpaManagementSystem.Domain.Entities;
using SpaManagementSystem.Domain.Interfaces;
using SpaManagementSystem.Application.Exceptions;

namespace SpaManagementSystem.Application.Extensions.RepositoryExtensions;

/// <summary>
/// Provides extension methods for the <see cref="ISalonRepository"/> interface.
/// </summary>
public static class SalonRepositoryExtensions
{
    /// <summary>
    /// Retrieves a salon by its unique identifier.
    /// </summary>
    /// <param name="repository">The salon repository.</param>
    /// <param name="salonId">The unique identifier of the salon.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains the <see cref="Salon"/> entity.
    /// </returns>
    /// <exception cref="ArgumentException">Thrown when the salon ID is empty.</exception>
    /// <exception cref="NotFoundException">Thrown when the salon is not found.</exception>
    public static async Task<Salon> GetByIdOrFailAsync(this ISalonRepository repository, Guid salonId)
    {
        if (salonId == Guid.Empty)
            throw new ArgumentException("The salon id cannot be empty", nameof(salonId));

        var salon = await repository.GetByIdAsync(salonId);
        if (salon == null)
            throw new NotFoundException($"The salon with id {salonId} was not found.");

        return salon;
    }
}