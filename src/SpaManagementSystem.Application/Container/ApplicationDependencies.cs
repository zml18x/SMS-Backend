using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.DependencyInjection;
using SpaManagementSystem.Application.Services;
using SpaManagementSystem.Application.Interfaces;
using SpaManagementSystem.Application.Requests.Salon;
using SpaManagementSystem.Application.Requests.CommonValidators;
using SpaManagementSystem.Application.Requests.UserAccount.Validators;

namespace SpaManagementSystem.Application.Container
{
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
            
            return services;
        }
        

        /// <summary>
        /// Registers services specific to the application's core functionalities.
        /// This method is used internally to add scoped services which are essential
        /// for the application's operation and are utilized across various components of the application.
        /// </summary>
        /// <param name="services">The collection of service descriptors where services are registered.</param>
        /// <returns>The IServiceCollection with added scoped services, enabling chained configurations.</returns>
        private static IServiceCollection ConfigureServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ISalonService, SalonService>();
            services.AddScoped<IValidator<JsonPatchDocument<UpdateSalonDetailsRequest>>,
                    JsonPatchDocumentValidator<UpdateSalonDetailsRequest>>();


            return services;
        }

        /// <summary>
        /// Configures FluentValidation integration with the application.
        /// This method sets up automatic validation and client-side adapters for models based on FluentValidation.
        /// It also registers all validators within the assembly that contains the RegisterRequestValidator,
        /// ensuring that they are available to validate incoming requests.
        /// </summary>
        /// <param name="services">The collection of service descriptors for registering FluentValidation services.</param>
        /// <returns>The IServiceCollection with FluentValidation services configured, supporting fluent configuration.</returns>
        private static IServiceCollection ConfigureFluentValidation(this IServiceCollection services)
        {
            services.AddFluentValidationAutoValidation()
                .AddFluentValidationClientsideAdapters()
                .AddValidatorsFromAssemblyContaining<RegisterRequestValidator>();

            return services;
        }
    }
}