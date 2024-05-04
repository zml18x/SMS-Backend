using Microsoft.Extensions.DependencyInjection;

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
        /// This method serves as a stub for future extensions where application-specific services such as domain services,
        /// application services, and handlers can be registered.
        /// </summary>
        /// <param name="services">The collection of service descriptors for registering application layer services.</param>
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            return services;
        }
    }
}