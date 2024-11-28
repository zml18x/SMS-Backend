using SpaManagementSystem.Domain.Enums;

namespace SpaManagementSystem.Application.Dto;

public record CustomerDto(Guid Id, string FirstName, string LastName, GenderType Gender, string PhoneNumber,
    string? Email);