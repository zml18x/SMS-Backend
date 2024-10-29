using SpaManagementSystem.Domain.Enums;

namespace SpaManagementSystem.Application.Requests.Employee;

public record UpdateEmployeeRequest(
    string Position,
    EmploymentStatus EmploymentStatus,
    string Code,
    string Color,
    DateOnly HireDate,
    string? Notes);