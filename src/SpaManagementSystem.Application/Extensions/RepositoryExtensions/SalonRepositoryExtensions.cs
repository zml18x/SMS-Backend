using SpaManagementSystem.Domain.Entities;
using SpaManagementSystem.Domain.Interfaces;
using SpaManagementSystem.Application.Exceptions;

namespace SpaManagementSystem.Application.Extensions.RepositoryExtensions;

public static class SalonRepositoryExtensions
{
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