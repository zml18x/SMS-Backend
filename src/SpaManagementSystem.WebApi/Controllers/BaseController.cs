using Microsoft.AspNetCore.Mvc;

namespace SpaManagementSystem.WebApi.Controllers
{
    /// <summary>
    /// A base class for api controller in SpaManagementSystem.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class BaseController : ControllerBase
    {
        /// <summary>
        /// Gets the User ID as a GUID if the user is authenticated; otherwise, returns Guid.Empty.
        /// </summary>
        protected Guid UserId
        {
            get
            {
                if (User?.Identity?.IsAuthenticated == true)
                    if (Guid.TryParse(User.Identity.Name, out var userId))
                        return userId;

                return Guid.Empty;
            }
        }
    }
}