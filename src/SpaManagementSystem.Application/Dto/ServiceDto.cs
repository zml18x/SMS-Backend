namespace SpaManagementSystem.Application.Dto;

public record ServiceDto(
    Guid Id,
    string Name,
    string Code,
    string? Description,
    decimal Price,
    decimal TaxRate,
    TimeSpan Duration,
    bool IsActive,
    string? ImgUrl);