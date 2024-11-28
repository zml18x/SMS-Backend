using SpaManagementSystem.Domain.Enums;

namespace SpaManagementSystem.Application.Requests.Customer;

public record CreateCustomerRequest(
    Guid SalonId,
    string FirstName,
    string LastName,
    GenderType Gender,
    string PhoneNumber,
    string? Email,
    string? Notes);