namespace SpaManagementSystem.Application.Requests.Service;

public record CreateServiceRequest(
    Guid SalonId,
    string Name,
    string Code,
    string? Description,
    decimal Price,
    decimal TaxRate,
    TimeSpan Duration,
    string? ImgUrl);