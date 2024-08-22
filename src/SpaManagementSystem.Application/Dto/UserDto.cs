namespace SpaManagementSystem.Application.Dto;

public record UserDto(Guid Id, string Email, string PhoneNumber, IList<string> Roles);