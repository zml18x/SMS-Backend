using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.DependencyInjection;
using SpaManagementSystem.Application.Mappers;
using SpaManagementSystem.Application.Services;
using SpaManagementSystem.Application.Interfaces;
using SpaManagementSystem.Application.Requests.Salon;
using SpaManagementSystem.Application.Requests.Auth.Validators;
using SpaManagementSystem.Application.Requests.Common.Validators;

namespace SpaManagementSystem.Application.Container;

/// <summary>
/// Provides a centralized class for configuring essential application layer services required by the application.
/// This class is intended for setting up services specific to the application layer, such as application logic services,
/// validation, or business process handling components.
/// </summary>
public static class ApplicationDependencies
{
    /// <summary>
    /// Configures core services and dependencies for the application layer.
    /// This method is the entry point for adding application-specific services such as domain services,
    /// application services, and validation mechanisms to the IServiceCollection.
    /// </summary>
    /// <param name="services">The collection of service descriptors for registering application layer services.</param>
    /// <returns>The IServiceCollection with registered services, supporting fluent configuration.</returns>
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.ConfigureServices();
        services.ConfigureFluentValidation();
        services.ConfigureAutoMapper();
            
        return services;
    }
    
    private static IServiceCollection ConfigureServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ISalonService, SalonService>();

        return services;
    }
    
    private static IServiceCollection ConfigureFluentValidation(this IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation()
            .AddFluentValidationClientsideAdapters()
            .AddValidatorsFromAssemblyContaining<RegisterRequestValidator>();
        
        services.AddScoped<IValidator<JsonPatchDocument<UpdateSalonDetailsRequest>>,
            JsonPatchDocumentValidator<UpdateSalonDetailsRequest>>();

        return services;
    }
    
    private static IServiceCollection ConfigureAutoMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(AutoMapperProfile));
        
        return services;
    }
}