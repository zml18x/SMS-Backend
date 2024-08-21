using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Authorization;
using SpaManagementSystem.Infrastructure.Identity.Enums;
using SpaManagementSystem.Infrastructure.Identity.Entities;
using SpaManagementSystem.Application.Dto;
using SpaManagementSystem.Application.Interfaces;
using SpaManagementSystem.Application.Requests.UserAccount;

namespace SpaManagementSystem.WebApi.Controllers;

[Route("api/User/Account")]
[ApiController]
public class UserAccountController(SignInManager<User> signInManager, IUserService userService, 
    IEmailSender<User> emailSender) : BaseController
{
    /// <summary>
    /// Registers a new user and sends confirmation email.
    /// </summary>
    /// <remarks>
    /// Sample request:
    /// 
    ///     POST /Account/Register
    ///     {
    ///         "email": "example@mail.com",
    ///         "password": "pa$$w0rD!X",
    ///         "phoneNumber": "999555111",
    ///         "firstName": "John",
    ///         "lastName": "Doe",
    ///         "gender": "male",
    ///         "dateOfBirth": "2000-01-01"
    ///     }
    /// </remarks>
    /// <param name="request">The registration request containing user details.</param>
    /// <response code="201">Returns the newly created user.</response>
    /// <response code="400">If the request is invalid (e.g. incorrect data was provided) or any error occurs during registration.</response>
    /// <response code="500">If there is a server error. (e.g. while generating the email confirmation token)</response>
    [HttpPost("Register")]
    [Consumes("application/json")]
    public async Task<IActionResult> RegisterAsync([FromBody] RegisterRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
            
        var user = new User { UserName = request.Email, Email = request.Email, PhoneNumber = request.PhoneNumber };

        var result = await signInManager.UserManager.CreateAsync(user, request.Password);

        if (result.Succeeded)
        {
            try
            {
                await signInManager.UserManager.AddToRoleAsync(user, RoleType.Admin.ToString());
                await userService.CreateProfileAsync(user.Id, request.FirstName, request.LastName, request.Gender,
                    request.DateOfBirth);
                    
                var token = await signInManager.UserManager.GenerateEmailConfirmationTokenAsync(user);

                if (string.IsNullOrWhiteSpace(token))
                    return StatusCode(StatusCodes.Status500InternalServerError,
                        "Failed to generate email confirmation token.");

                await emailSender.SendConfirmationLinkAsync(user, user.Email, token);
                    
                return Created("api/Account", null);
            }
            catch
            {
                await signInManager.UserManager.RemoveFromRoleAsync(user, RoleType.Admin.ToString());
                await signInManager.UserManager.DeleteAsync(user);

                throw;
            }
        }

        foreach (var error in result.Errors)
            ModelState.AddModelError(error.Code, error.Description);

        return BadRequest(ModelState);
    }
        
    
        
    /// <summary>
    /// Retrieves the current user's account details.
    /// </summary>
    /// <remarks>
    /// This endpoint retrieves the details of the currently authenticated user.
    /// 
    /// Sample request:
    /// 
    ///     GET /Account
    /// 
    /// </remarks>
    /// <returns>An <see cref="IActionResult"/> containing the user's account details, or a 404 if the user is not found.</returns>
    /// <response code="200">Returns the user's account details.</response>
    /// <response code="404">If the user with the specified ID is not found.</response>
    /// <response code="401">If the user is not authenticated (authorization failed).</response>
    [Produces("application/json")]
    [ProducesResponseType(typeof(UserDetailsDto), StatusCodes.Status200OK)]
    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> GetAccountAsync()
    {
        var user = await signInManager.UserManager.FindByIdAsync(UserId.ToString());

        if (user == null)
            return NotFound($"User with id {UserId} not found");

        var userProfile = await userService.GetProfileAsync(user.Id);

        return new OkObjectResult(new UserDetailsDto(user.Id, user.Email!, user.PhoneNumber!, userProfile.FirstName,
            userProfile.LastName, userProfile.Gender.ToString(), userProfile.DateOfBirth));
    }

    /// <summary>
    /// Updates the current user's profile with the provided details using JSON Patch operations.
    /// </summary>
    /// <remarks>
    /// This endpoint allows users to update their profile information using a JSON Patch document.
    /// Sample request:
    /// 
    ///     PATCH /Account/UpdateProfile
    ///     [
    ///         { "op": "replace", "path": "/firstName", "value": "John" },
    ///         { "op": "replace", "path": "/lastName", "value": "Doe" },
    ///         { "op": "replace", "path": "/gender", "value": "male" },
    ///         { "op": "replace", "path": "/dateOfBirth", "value": "2000-01-01" }
    ///     ]
    /// </remarks>
    /// <param name="patchDocument">The JsonPatchDocument object containing the patch operations to apply to the profile.</param>
    /// <param name="requestValidator">The <see cref="IValidator{T}"/> instance used to validate the updated profile request.</param>
    /// <returns>An <see cref="IActionResult"/> indicating the success or failure of the update operation.</returns>
    /// <response code="200">Indicates that the profile has been successfully updated.</response>
    /// <response code="400">Indicates that the request is invalid or no changes were made.</response>
    /// <response code="401">Indicates that the user is not authenticated.</response>
    /// <response code="404">Indicates that the user profile was not found.</response>
    [Consumes("application/json")]
    [Authorize(Roles = "Admin")]
    [HttpPatch("UpdateProfile")]
    public async Task<IActionResult> UpdateProfileAsync( [FromBody] JsonPatchDocument<UpdateProfileRequest> patchDocument,
        [FromServices] IValidator<UpdateProfileRequest> requestValidator)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var existingUserProfile = await userService.GetProfileAsync(UserId);

        var request = new UpdateProfileRequest(existingUserProfile.FirstName, existingUserProfile.LastName,
            existingUserProfile.Gender.ToString(), existingUserProfile.DateOfBirth);

        patchDocument.ApplyTo(request, ModelState);

        var requestValidationResult = await requestValidator.ValidateAsync(request);
        if (!requestValidationResult.IsValid)
        {
            foreach (var error in requestValidationResult.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

            return BadRequest(ModelState);
        }

        var result = await userService.UpdateProfileAsync(UserId, request);

        if (result)
            return Ok("The profile has been updated successfully.");

        return BadRequest("No changes were made to the profile.");
    }
}