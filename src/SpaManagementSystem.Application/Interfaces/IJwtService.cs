using SpaManagementSystem.Application.Dto;

namespace SpaManagementSystem.Application.Interfaces
{
    /// <summary>
    /// Defines the contract for a service responsible for handling JWT operations within the Spa Management System.
    /// This service provides functionalities to generate JWTs for user authentication purposes.
    /// </summary>
    public interface IJwtService
    {
        /// <summary>
        /// Creates a JWT for a given user based on their ID, email, and roles.
        /// </summary>
        /// <param name="userId">The unique identifier of the user for whom the token is being created.</param>
        /// <param name="userEmail">The email address of the user, included in the JWT as a claim.</param>
        /// <param name="userRoles">A list of roles associated with the user, which will be included in the JWT as claims.</param>
        /// <returns>A <see cref="JwtDto"/> object containing the generated token and other relevant token information.</returns>
        /// <remarks>
        /// This method is typically used during the authentication process to provide users with a token that
        /// certifies their identity and authorizations for subsequent requests to the system.
        /// </remarks>
        JwtDto CreateToken(Guid userId, string userEmail, IList<string> userRoles);
    }
}