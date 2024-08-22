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
    /// Retrieves the current user's account.
    /// </summary>
    /// <remarks>
    /// This endpoint retrieves the details of the currently authenticated user.
    /// 
    /// Sample request:
    /// 
    ///     GET /Account
    /// 
    /// </remarks>
    /// <returns>An <see cref="IActionResult"/> containing the user's account or a 404 if the user is not found.</returns>
    /// <response code="200">Returns the user's account dto.</response>
    /// <response code="404">If the user with the specified ID is not found.</response>
    /// <response code="401">If the user is not authenticated (authorization failed).</response>
    [Produces("application/json")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAccountAsync()
    {
        var user = await signInManager.UserManager.FindByIdAsync(UserId.ToString());
        if (user == null)
            return NotFound($"User with id {UserId} not found");

        return new OkObjectResult(new UserDto(user.Id, user.Email!, user.PhoneNumber!));
    }
}