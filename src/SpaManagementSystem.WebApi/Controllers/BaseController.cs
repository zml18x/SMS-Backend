using Microsoft.AspNetCore.Mvc;

namespace SpaManagementSystem.WebApi.Controllers;

/// <summary>
/// A base class for api controller in SpaManagementSystem.
/// </summary>
[ApiController]
public class BaseController : ControllerBase
{
    /// <summary>
    /// Gets the User ID as a GUID if the user is authenticated; otherwise, returns Guid.Empty.
    /// </summary>
    protected Guid UserId => User?.Identity?.IsAuthenticated == true
        ? Guid.TryParse(User.Identity.Name, out var userId) ? userId : Guid.Empty
        : Guid.Empty;
}