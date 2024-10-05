using SpaManagementSystem.Application.Requests.Salon;
using SpaManagementSystem.Domain.Entities;

namespace SpaManagementSystem.Application.Extensions;

public static class SalonExtensions
{
    public static bool HasChanges(this Salon existingSalon, UpdateSalonRequest request)
        => existingSalon.Name != request.Name ||
           existingSalon.Email != request.Email ||
           existingSalon.PhoneNumber != request.PhoneNumber ||
           existingSalon.Description != request.Description;
}