using SpaManagementSystem.Domain.Enums;

namespace SpaManagementSystem.Application.Dto;

public record EmployeeSummaryDto(
    Guid Id,
    Guid SalonId,
    string Position,
    EmploymentStatus EmploymentStatus,
    string Code,
    string Color,
    string FirstName,
    string LastName,
    GenderType Gender,
    string Email,
    string PhoneNumber);