using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.DependencyInjection;
using SpaManagementSystem.Domain.Builders;
using SpaManagementSystem.Domain.Entities;
using SpaManagementSystem.Domain.ValueObjects;
using SpaManagementSystem.Domain.Specifications;
using SpaManagementSystem.Application.Mappers;
using SpaManagementSystem.Application.Services;
using SpaManagementSystem.Application.Interfaces;
using SpaManagementSystem.Application.Requests.Salon;
using SpaManagementSystem.Application.Requests.Auth.Validators;
using SpaManagementSystem.Application.Requests.Common.Validators;
using SpaManagementSystem.Application.Requests.Service;

namespace SpaManagementSystem.Application.Container;

public static class ApplicationDependencies
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services
            .ConfigureServices()
            .ConfigureFluentValidation()
            .ConfigureAutoMapper();
            
        return services;
    }
    
    private static IServiceCollection ConfigureServices(this IServiceCollection services)
    {
        services.AddScoped<IRefreshTokenService, RefreshTokenService>();
        services.AddScoped<ISalonService, SalonService>();
        services.AddScoped<IEmployeeService, EmployeeService>();
        services.AddScoped<ISalonServiceService, SalonServiceService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IEmployeeAvailabilityService, EmployeeAvailabilityService>();
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<IAppointmentService, Services.AppointmentService>();
        
        services.AddScoped<ISpecification<Salon>, SalonSpecification>();
        services.AddScoped<ISpecification<Address>, AddressSpecification>();
        services.AddScoped<ISpecification<Employee>, EmployeeSpecification>();
        services.AddScoped<ISpecification<EmployeeProfile>, EmployeeProfileSpecification>();
        services.AddScoped<ISpecification<Service>, ServiceSpecification>();
        services.AddScoped<ISpecification<Product>, ProductSpecification>();
        services.AddScoped<ISpecification<EmployeeAvailability>, EmployeeAvailabilitySpecification>();
        services.AddScoped<ISpecification<Customer>, CustomerSpecification>();
        services.AddScoped<ISpecification<Appointment>, AppointmentSpecification>();
        services.AddScoped<ISpecification<Domain.Entities.AppointmentService>, AppointmentServiceSpecification>();
        services.AddScoped<ISpecification<Payment>, PaymentSpecification>();
        
        services.AddScoped<SalonBuilder>();
        services.AddScoped<AddressBuilder>();
        services.AddScoped<EmployeeBuilder>();
        services.AddScoped<ServiceBuilder>();
        services.AddScoped<ProductBuilder>();
        services.AddScoped<EmployeeAvailabilityBuilder>();
        services.AddScoped<CustomerBuilder>();
        services.AddScoped<AppointmentBuilder>();
        services.AddScoped<AppointmentServiceBuilder>();
        services.AddScoped<PaymentBuilder>();
        
        return services;
    }
    
    private static IServiceCollection ConfigureFluentValidation(this IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation()
            .AddFluentValidationClientsideAdapters()
            .AddValidatorsFromAssemblyContaining<RegisterRequestValidator>();

        services.AddScoped<IValidator<JsonPatchDocument<UpdateSalonRequest>>,
            JsonPatchDocumentValidator<UpdateSalonRequest>>();
        
        services.AddScoped<IValidator<JsonPatchDocument<UpdateServiceRequest>>,
            JsonPatchDocumentValidator<UpdateServiceRequest>>();

        return services;
    }
    
    private static IServiceCollection ConfigureAutoMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(AutoMapperProfile));
        
        return services;
    }
}