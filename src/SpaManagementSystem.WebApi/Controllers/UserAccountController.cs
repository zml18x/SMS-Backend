using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using SpaManagementSystem.Infrastructure.Identity.Entities;
using SpaManagementSystem.Application.Dto;

namespace SpaManagementSystem.WebApi.Controllers;

[Route("api/User/Account")]
[ApiController]
public class UserAccountController(SignInManager<User> signInManager) : BaseController
{
    /// <summary>
    /// Retrieves the account information for the currently authenticated user.
    /// </summary>
    /// <remarks>
    /// This endpoint returns the user's account details, including their email, phone number, and assigned roles.
    /// The user must be authenticated to access this endpoint.
    /// </remarks>
    /// <returns>
    /// Returns an HTTP response containing the user's account details.
    /// </returns>
    /// <response code="200">Successfully retrieved the user's account details.</response>
    /// <response code="401">Returned if the user is not authenticated.</response>
    /// <response code="404">Returned if the user with the specified ID is not found.</response>
    [Produces("application/json")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAccountAsync()
    {
        var user = await signInManager.UserManager.FindByIdAsync(UserId.ToString());
        if (user == null)
            return NotFound($"User with id {UserId} not found");
        
        var userRoles = await signInManager.UserManager.GetRolesAsync(user);
        
        return Ok(new UserDto(user.Id, user.Email!, user.PhoneNumber!,userRoles));
    }
}