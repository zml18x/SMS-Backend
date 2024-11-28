using SpaManagementSystem.Domain.Enums;

namespace SpaManagementSystem.Application.Requests.Customer;

public record UpdateCustomerRequest(
    string FirstName,
    string LastName,
    GenderType Gender,
    string PhoneNumber,
    string? Email,
    string? Notes,
    bool IsActive);