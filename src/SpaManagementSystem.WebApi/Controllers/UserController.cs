using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using SpaManagementSystem.Domain.Enums;
using SpaManagementSystem.Infrastructure.Identity.Entities;
using SpaManagementSystem.Application.Dto;

namespace SpaManagementSystem.WebApi.Controllers;

[Route("api/user")]
[ApiController]
public class UserController(UserManager<User> userManager) : BaseController
{
    /// <summary>
    /// Retrieves the details for the currently authenticated user.
    /// </summary>
    /// <remarks>
    /// This endpoint returns the user's details, including their email, phone number, and assigned roles.
    /// The user must be authenticated to access this endpoint.
    /// </remarks>
    /// <returns>
    /// Returns an HTTP response containing the user's details.
    /// </returns>
    /// <response code="200">Successfully retrieved the user's details.</response>
    /// <response code="401">Returned if the user is not authenticated.</response>
    /// <response code="404">Returned if the user with the specified ID is not found.</response>
    [Produces("application/json")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetUserAsync()
    {
        var user = await userManager.FindByIdAsync(UserId.ToString());
        if (user == null)
            return NotFound($"User with id {UserId} not found");
        
        var userRoles = await userManager.GetRolesAsync(user);
        
        return Ok(new UserDto(user.Id, user.Email!, user.PhoneNumber!,userRoles));
    }
    
    /// <summary>
    /// Retrieves the details of a specific user by their unique identifier (ID).
    /// </summary>
    /// <remarks>
    /// This endpoint allows users with the "Admin" or "Manager" roles to retrieve detailed information about a specific user.
    /// The details include the user's ID, email, phone number, and roles.
    /// </remarks>
    /// <param name="userId">The unique identifier (GUID) of the user whose details are being retrieved.</param>
    /// <returns>
    /// Returns an HTTP response containing the user's details.
    /// </returns>
    /// <response code="200">Successfully retrieved the user's details.</response>
    /// <response code="401">Returned if the user is not authenticated.</response>
    /// <response code="403">Returned if the user is not authorized to view the details.</response>
    /// <response code="404">Returned if the user with the specified ID is not found.</response>
    [Produces("application/json")]
    [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Roles = "Admin, Manager")]
    [HttpGet("{userId:guid}")]
    public async Task<IActionResult> GetUserByIdAsync(Guid userId)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());
        if (user == null)
            return NotFound($"User with id {UserId} not found");
        
        var userRoles = await userManager.GetRolesAsync(user);
        
        return Ok(new UserDto(user.Id, user.Email!, user.PhoneNumber!,userRoles));
    }
    
    /// <summary>
    /// Assigns the "Manager" role to a specific user by their unique identifier (ID).
    /// </summary>
    /// <remarks>
    /// This endpoint allows an admin to assign the "Manager" role to a user.
    /// If the user is not found or there is an error during role assignment, an appropriate error response is returned.
    /// </remarks>
    /// <param name="userId">The unique identifier (GUID) of the user to whom the "Manager" role will be assigned.</param>
    /// <returns>
    /// Returns an HTTP response indicating the result of the role assignment.
    /// </returns>
    /// <response code="200">Successfully assigned the "Manager" role to the user.</response>
    /// <response code="400">Returned if there was an error during role assignment.</response>
    /// <response code="401">Returned if the user is not authenticated.</response>
    /// <response code="403">Returned if the user is not authorized to assign roles.</response>
    /// <response code="404">Returned if the user with the specified ID is not found.</response>
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Roles = "Admin")]
    [HttpPost("assign-role/{userId:guid}")]
    public async Task<IActionResult> AssignManagerRoleAsync(Guid userId)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());
        if (user == null)
            return NotFound($"User with id {UserId} not found");

        var result = await userManager.AddToRoleAsync(user, RoleTypes.Manager.ToString());
        if (!result.Succeeded)
            return BadRequest("Error while assigning the role.");
        
        return Ok("Manager role has been assigned.");
    }
    
    /// <summary>
    /// Removes the "Manager" role from a specific user by their unique identifier (ID).
    /// </summary>
    /// <remarks>
    /// This endpoint allows users with the "Admin" or "Manager" roles to remove the "Manager" role from a user.
    /// If the user is not found or there is an error during role removal, an appropriate error response is returned.
    /// </remarks>
    /// <param name="userId">The unique identifier (GUID) of the user from whom the "Manager" role will be removed.</param>
    /// <returns>
    /// Returns an HTTP response indicating the result of the role removal.
    /// </returns>
    /// <response code="200">Successfully removed the "Manager" role from the user.</response>
    /// <response code="400">Returned if there was an error during role removal.</response>
    /// <response code="401">Returned if the user is not authenticated.</response>
    /// <response code="403">Returned if the user is not authorized to remove roles.</response>
    /// <response code="404">Returned if the user with the specified ID is not found.</response>
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Roles = "Admin, Manager")]
    [HttpPost("remove-role/{userId:guid}")]
    public async Task<IActionResult> RemoveManagerRoleAsync(Guid userId)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());
        if (user == null)
            return NotFound($"User with id {UserId} not found");

        var result = await userManager.RemoveFromRoleAsync(user, RoleTypes.Manager.ToString());
        if (!result.Succeeded)
            return BadRequest("Error while removing the role.");

        return Ok("Manager role has been removed.");
    }
}