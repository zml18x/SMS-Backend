using SpaManagementSystem.Domain.Entities;
using SpaManagementSystem.Application.Requests.Customer;

namespace SpaManagementSystem.Application.Extensions;

public static class CustomerExtensions
{
    public static bool HasChanges(this Customer existingEmployee, UpdateCustomerRequest request)
    {
        return existingEmployee.FirstName != request.FirstName ||
               existingEmployee.LastName != request.LastName ||
               existingEmployee.Gender != request.Gender||
               existingEmployee.PhoneNumber != request.PhoneNumber ||
               existingEmployee.Email != request.Email ||
               existingEmployee.Notes != request.Notes;
    }
}