using SpaManagementSystem.Domain.Enums;

namespace SpaManagementSystem.Application.Requests.Employee;

public record CreateEmployeeRequest(
    Guid SalonId,
    Guid UserId,
    string Position,
    EmploymentStatus EmploymentStatus,
    string Code,
    string Color,
    DateOnly HireDate,
    string FirstName,
    string LastName,
    GenderType Gender,
    DateOnly DateOfBirth,
    string Email,
    string PhoneNumber);