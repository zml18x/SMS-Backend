using SpaManagementSystem.Domain.Enums;

namespace SpaManagementSystem.Application.Requests.Employee;

public record UpdateEmployeeProfileRequest(
    string FirstName,
    string LastName,
    GenderType Gender,
    DateOnly DateOfBirth,
    string Email,
    string PhoneNumber);