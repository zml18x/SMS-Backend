using SpaManagementSystem.Domain.Enums;

namespace SpaManagementSystem.Application.Dto;

public record EmployeeProfileDto(
    string FirstName,
    string LastName,
    GenderType Gender,
    DateOnly DateOfBirth,
    string Email,
    string PhoneNumber);