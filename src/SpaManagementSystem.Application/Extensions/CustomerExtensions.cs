using SpaManagementSystem.Domain.Entities;
using SpaManagementSystem.Application.Requests.Customer;

namespace SpaManagementSystem.Application.Extensions;

public static class CustomerExtensions
{
    public static bool HasChanges(this Customer existingCustomer, UpdateCustomerRequest request)
    {
        return existingCustomer.FirstName != request.FirstName ||
               existingCustomer.LastName != request.LastName ||
               existingCustomer.Gender != request.Gender ||
               existingCustomer.PhoneNumber != request.PhoneNumber ||
               existingCustomer.Email != request.Email ||
               existingCustomer.Notes != request.Notes ||
               existingCustomer.IsActive != request.IsActive;
    }
}