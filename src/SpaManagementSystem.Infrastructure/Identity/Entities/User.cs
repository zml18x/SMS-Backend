using Microsoft.AspNetCore.Identity;

namespace SpaManagementSystem.Infrastructure.Identity.Entities
{
    /// <summary>
    /// Represents a user in the Spa Management System.
    /// This class extends the IdentityUser class provided by ASP.NET Core Identity,
    /// incorporating a globally unique identifier (GUID) as the primary key.
    /// </summary>
    /// <remarks>
    /// The User class can be extended with additional properties specific to the Spa Management System,
    /// such as profile details, preferences, and other relevant user-specific information that is not
    /// covered by the default IdentityUser properties.
    /// </remarks>
    public class User : IdentityUser<Guid>
    {
        
    }
}