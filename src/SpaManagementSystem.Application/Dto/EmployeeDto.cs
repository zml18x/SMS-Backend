using SpaManagementSystem.Domain.Enums;

namespace SpaManagementSystem.Application.Dto;

public record EmployeeDto(
    Guid Id,
    Guid SalonId,
    string Position,
    EmploymentStatus EmploymentStatus,
    string Code,
    string Color,
    DateOnly HireDate,
    string? Notes);