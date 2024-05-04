namespace SpaManagementSystem.Infrastructure.Identity.Enums
{
    /// <summary>
    /// Defines the different roles available within the Spa Management System.
    /// Each role corresponds to a specific set of permissions and responsibilities within the application.
    /// </summary>
    public enum RoleType
    {
        /// <summary>
        /// Represents an administrator role that typically has the highest level of access rights,
        /// allowing for system-wide configuration and management.
        /// </summary>
        Admin,

        /// <summary>
        /// Represents a manager role with permissions to oversee and manage specific areas or functions
        /// within the spa, typically including supervision of employees and operational management.
        /// </summary>
        Manager,

        /// <summary>
        /// Represents an employee role with access rights limited to tasks and functions directly related
        /// to their job responsibilities.
        /// </summary>
        Employee,
    }
}