using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using SpaManagementSystem.Infrastructure.Identity.Enums;
using SpaManagementSystem.Infrastructure.Identity.Entities;
using SpaManagementSystem.Application.Dto;
using SpaManagementSystem.Application.Interfaces;
using SpaManagementSystem.Application.Requests.Auth;

namespace SpaManagementSystem.WebApi.Controllers;

[ApiController]
[Route("api/Auth")]
public class AuthController(SignInManager<User> signInManager, ITokenService tokenService, IEmailSender<User> emailSender,
    IRefreshTokenService refreshTokenService) : BaseController
{
    /// <summary>
    /// Registers a new user with the provided email, password, and phone number.
    /// </summary>
    /// <remarks>
    /// This endpoint allows a client to create a new user account by providing an email, password, and phone number.
    /// Upon successful registration, the user is assigned the "Admin" role.
    /// </remarks>
    /// <param name="request">The request object containing the user's email, password, and phone number.</param>
    /// <returns>
    /// Returns an HTTP response indicating the result of the registration.
    /// </returns>
    /// <response code="201">User was successfully registered and assigned to the "Admin" role.</response>
    /// <response code="400">Returned if the registration failed due to validation errors or other issues.</response>
    /// <response code="500">Returned if an unexpected error occurs during the processing of the request.</response>
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPost("Register")]
    public async Task<IActionResult> RegisterAsync([FromBody] UserRegisterRequest request)
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
    /// Authenticates a user with their email and password, and returns a JWT access token and a refresh token.
    /// </summary>
    /// <remarks>
    /// This endpoint allows a client to sign in by providing valid credentials (email and password).
    /// If the authentication is successful, the user receives a JWT access token and a refresh token for further authenticated requests.
    /// </remarks>
    /// <param name="request">The request object containing the user's email and password.</param>
    /// <returns>
    /// Returns an HTTP response indicating the result of the sign-in.
    /// </returns>
    /// <response code="200">Returns the JWT access token, its expiration time, and a refresh token with its expiration time.</response>
    /// <response code="400">Returned if the credentials are invalid, the email is not confirmed or if the request data is incorrect.</response>
    /// <response code="500">Returned if an unexpected error occurs during the processing of the request.</response>
    [Consumes("application/json")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPost("SignIn")]
    public async Task<IActionResult> SignInAsync([FromBody] SignInRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
            
        var user = await signInManager.UserManager.FindByEmailAsync(request.Email);
        if (user == null)
            return BadRequest("Invalid Credentials");

        var signInResult =
            await signInManager.PasswordSignInAsync(request.Email, request.Password, false,
                lockoutOnFailure: false);
        
        if (signInResult.Succeeded)
        {
            var userRoles = await signInManager.UserManager.GetRolesAsync(user);
            var accessToken = tokenService.CreateJwtToken(new UserDto(user.Id, user.Email!, user.PhoneNumber!, userRoles));
            var refreshToken = tokenService.CreateRefreshToken(user.Id);
            
            await refreshTokenService.SaveRefreshToken(refreshToken);

            var response = new AuthResponseDto(accessToken.Token, accessToken.Expire, refreshToken.Token,
                refreshToken.ExpirationTime);

            return Ok(response);
        }

        if (signInResult.IsNotAllowed)
        {
            var validPassword = await signInManager.UserManager.CheckPasswordAsync(user, request.Password);
            if (!user.EmailConfirmed && validPassword)
                return BadRequest("Email not confirmed.");
        }

        return BadRequest("Invalid Credentials");
    }
    
    /// <summary>
    /// Handles the refreshing of JWT access tokens using a valid refresh token.
    /// </summary>
    /// <remarks>
    /// This endpoint allows a client to refresh an expired access token by providing a valid refresh token.
    /// </remarks>
    /// <param name="request">The request object containing the current access token and refresh token.</param>
    /// <returns>
    /// Returns an HTTP response indicating the result of the token refresh operation.
    /// </returns>
    /// <response code="200">Returns the new access token and refresh token along with their expiration times.</response>
    /// <response code="400">Returned if the provided tokens are invalid, expired, or if the request data is incorrect.</response>
    /// <response code="404">Returned if the user associated with the provided token could not be found.</response>
    /// <response code="500">Returned if an unexpected error occurs during the processing of the request.</response>
    [Consumes("application/json")]
    [Produces("application/json")]
    [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPost("Refresh")]
    public async Task<IActionResult> RefreshAsync([FromBody] RefreshRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        ClaimsPrincipal principal;

        try
        {
            principal = tokenService.GetPrincipalFromExpiredToken(request.AccessToken);
        }
        catch(SecurityTokenException e)
        {
            return BadRequest($"Access token error: {e.Message}");
        }

        if (!Guid.TryParse(principal.Identity?.Name, out var userId))
            return BadRequest("Invalid user ID in token.");

        var isValidRefreshToken = await refreshTokenService.IsValidToken(userId, request.RefreshToken);
        if (!isValidRefreshToken)
            return BadRequest("Refresh token is invalid or has expired.");

        var user = await signInManager.UserManager.FindByIdAsync(userId.ToString());
        if (user == null)
            return NotFound("User not found.");
        
        var userRoles = await signInManager.UserManager.GetRolesAsync(user);
        if (!userRoles.Any())
            return BadRequest("User roles could not be retrieved.");
        
        var newJwt = tokenService.CreateJwtToken(new UserDto(user.Id, user.Email!, user.PhoneNumber!, userRoles));
        var newRefreshToken = tokenService.CreateRefreshToken(userId);

        await refreshTokenService.SaveRefreshToken(newRefreshToken);

        var response = new AuthResponseDto(newJwt.Token, newJwt.Expire, newRefreshToken.Token,
            newRefreshToken.ExpirationTime);

        return Ok(response);
    }
    
    /// <summary>
    /// Confirms a user's email address using a confirmation token.
    /// </summary>
    /// <remarks>
    /// This endpoint allows a client to confirm a user's email address by providing the user's email and a confirmation token.
    /// The token is typically sent to the user's email address as part of the registration or email change process.
    /// </remarks>
    /// <param name="request">The request object containing the user's email and confirmation token.</param>
    /// <returns>
    /// Returns an HTTP response indicating the result of the email confirmation process.
    /// </returns>
    /// <response code="200">Email was confirmed successfully.</response>
    /// <response code="400">Returned if the confirmation token is invalid or the email could not be confirmed.</response>
    /// <response code="500">Returned if an unexpected error occurs during the processing of the request.</response>
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPost("ConfirmEmail")]
    public async Task<IActionResult> ConfirmEmailAsync([FromBody] ConfirmEmailRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = await signInManager.UserManager.FindByEmailAsync(request.Email);
            
        var confirmationFailedMsg = "Email confirmation failed. Please verify your email confirmation token.";
            
        if (user == null)
            return BadRequest(confirmationFailedMsg);

        var result = await signInManager.UserManager.ConfirmEmailAsync(user, request.Token);

        if (result.Succeeded)
            return Ok("Email confirmed successfully");

        return BadRequest(confirmationFailedMsg);
    }
    
    /// <summary>
    /// Sends an email confirmation link to the specified user's email address.
    /// </summary>
    /// <remarks>
    /// This endpoint allows a client to request a confirmation email to be sent to a user's email address. 
    /// The email contains a link that the user can use to confirm their email address.
    /// </remarks>
    /// <param name="request">The request object containing the user's email.</param>
    /// <returns>
    /// Returns an HTTP response indicating the result of the email sending process.
    /// </returns>
    /// <response code="200">The confirmation email was sent successfully, or the email is already confirmed.</response>
    /// <response code="400">Returned if the request data is invalid.</response>
    /// <response code="500">Returned if the token generation fails and the email could not be sent.</response>
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPost("SendConfirmationEmail")]
    public async Task<IActionResult> SendConfirmationEmailAsync([FromBody] SendConfirmationEmailRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = await signInManager.UserManager.FindByEmailAsync(request.Email);

        var successMsg = "Confirmation email sent successfully. Please check your inbox, " +
                         "including the spam folder, for further instructions.";

        if (user == null)
            return Ok(successMsg);

        if (user.EmailConfirmed)
            return Ok("Email already confirmed.");
            

        var token = await signInManager.UserManager.GenerateEmailConfirmationTokenAsync(user);

        if (string.IsNullOrWhiteSpace(token))
            return StatusCode(StatusCodes.Status500InternalServerError,
                "Failed to generate email confirmation token.");

        await emailSender.SendConfirmationLinkAsync(user, user.Email!, token);

        return Ok(successMsg);
    }
    
    /// <summary>
    /// Sends a confirmation email to change the user's email address.
    /// </summary>
    /// <remarks>
    /// This endpoint allows a logged-in user to request a confirmation email to change their email address.
    /// The email contains a link that the user can use to confirm the change of their email address.
    /// </remarks>
    /// <param name="request">The request object containing the new email address.</param>
    /// <returns>
    /// Returns an HTTP response indicating the result of the email sending process.
    /// </returns>
    /// <response code="200">The confirmation email was sent successfully.</response>
    /// <response code="400">Returned if the request data is invalid.</response>
    /// <response code="401">Returned if the user is not authenticated.</response>
    /// <response code="404">Returned if the user account could not be found.</response>
    /// <response code="500">Returned if the token generation fails.</response>
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize(Roles = "Admin, Manager, Employee")]
    [HttpPost("SendConfirmationChangeEmail")]
    public async Task<IActionResult> ChangeEmailAsync([FromBody] ChangeEmailRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = await signInManager.UserManager.FindByIdAsync(UserId.ToString());

        if (user == null)
            return NotFound("User account not found.");

        var token = await signInManager.UserManager.GenerateChangeEmailTokenAsync(user, request.NewEmail);

        if (string.IsNullOrWhiteSpace(token))
            return StatusCode(StatusCodes.Status500InternalServerError,
                "An error occurred while generating the change email token.");

        await emailSender.SendConfirmationLinkAsync(user, user.Email!, token);

        return Ok("Confirmation email sent successfully. Please check your email for further instructions.");
    }
    
    /// <summary>
    /// Confirms the change of the user's email address using a confirmation token.
    /// </summary>
    /// <remarks>
    /// This endpoint allows a logged-in user to confirm the change of their email address by providing the new email address and the confirmation token.
    /// </remarks>
    /// <param name="request">The request object containing the new email address and confirmation token.</param>
    /// <returns>
    /// Returns an HTTP response indicating the result of the email change process.
    /// </returns>
    /// <response code="200">The email address was changed successfully.</response>
    /// <response code="400">Returned if the request data is invalid.</response>
    /// <response code="401">Returned if the user is not authenticated.</response>
    /// <response code="404">Returned if the user account could not be found.</response>
    /// <response code="500">Returned if an unexpected error occurs during the processing of the request.</response>
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize(Roles = "Admin, Manager, Employee")]
    [HttpPost("ConfirmChangedEmail")]
    public async Task<IActionResult> ConfirmChangedEmail([FromBody] ConfirmationChangeEmailRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = await signInManager.UserManager.FindByIdAsync(UserId.ToString());

        if (user == null)
            return NotFound("User account not found.");

        var result = await signInManager.UserManager.ChangeEmailAsync(user, request.NewEmail, request.Token);
            
        if (result.Succeeded)
        {
            await signInManager.UserManager.SetUserNameAsync(user, request.NewEmail);
            user.EmailConfirmed = false;
            await signInManager.UserManager.UpdateAsync(user);
                
            return Ok("Email address changed successfully.");
        }

        foreach (var error in result.Errors)
            ModelState.AddModelError(error.Code, error.Description);

        return BadRequest(ModelState);
    }
    
    /// <summary>
    /// Changes the user's password.
    /// </summary>
    /// <remarks>
    /// This endpoint allows a logged-in user to change their password by providing the current password and a new password.
    /// </remarks>
    /// <param name="request">The request object containing the current and new passwords.</param>
    /// <returns>
    /// Returns an HTTP response indicating the result of the password change process.
    /// </returns>
    /// <response code="200">The password was changed successfully.</response>
    /// <response code="400">Returned if the request data is invalid.</response>
    /// <response code="401">Returned if the user is not authenticated.</response>
    /// <response code="404">Returned if the user account could not be found.</response>
    /// <response code="500">Returned if an unexpected error occurs during the processing of the request.</response>
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize(Roles = "Admin, Manager, Employee")]
    [HttpPost("ChangePassword")]
    public async Task<IActionResult> ChangePasswordAsync([FromBody] ChangePasswordRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = await signInManager.UserManager.FindByIdAsync(UserId.ToString());

        if (user == null)
            return NotFound("User account not found");

        var result = await signInManager.UserManager
            .ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);

        if (result.Succeeded)
            return Ok("Password changed successfully.");

        foreach (var error in result.Errors)
            ModelState.AddModelError(error.Code, error.Description);

        return BadRequest(ModelState);
    }
        
    /// <summary>
    /// Sends a password reset token to the user's email address.
    /// </summary>
    /// <remarks>
    /// This endpoint allows a user to request a password reset token by providing their email address. 
    /// The token will be sent to the user's email address if it exists in the system.
    /// </remarks>
    /// <param name="request">The request object containing the user's email address.</param>
    /// <returns>
    /// Returns an HTTP response indicating the result of the password reset token sending process.
    /// </returns>
    /// <response code="200">The password reset token was sent successfully.</response>
    /// <response code="400">Returned if the request data is invalid.</response>
    /// <response code="500">Returned if the token generation fails.</response>
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPost("SendResetPasswordToken")]
    public async Task<IActionResult> SendResetPasswordTokenAsync([FromBody] SendResetPasswordTokenRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = await signInManager.UserManager.FindByEmailAsync(request.Email);

        if (user != null)
        {
            var token = await signInManager.UserManager.GeneratePasswordResetTokenAsync(user);

            if (string.IsNullOrWhiteSpace(token))
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "An error occurred while generating the password reset token.");

            await emailSender.SendPasswordResetCodeAsync(user, user.Email!, token);
        }

        return Ok("A password reset request has been sent. If the provided email address exists in our system," +
                  " you will receive an email with instructions.");
    }
        
    /// <summary>
    /// Resets the user's password using a password reset token.
    /// </summary>
    /// <remarks>
    /// This endpoint allows a user to reset their password by providing their email address, a password reset token, and a new password.
    /// </remarks>
    /// <param name="request">The request object containing the user's email, password reset token, and new password.</param>
    /// <returns>
    /// Returns an HTTP response indicating the result of the password reset process.
    /// </returns>
    /// <response code="200">The password was reset successfully.</response>
    /// <response code="400">Returned if the request data is invalid or the user account could not be found.</response>
    /// <response code="500">Returned if the password reset failed due to an internal error.</response>
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [HttpPost("ResetPassword")]
    public async Task<IActionResult> ResetPasswordAsync([FromBody] ResetPasswordRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = await signInManager.UserManager.FindByEmailAsync(request.Email);

        if (user == null)
            return BadRequest("Password reset failed. Please check your email and try again.");

        var result = await signInManager.UserManager.ResetPasswordAsync(user, request.Token, request.NewPassword);

        if (result.Succeeded)
            return Ok("Password reset successfully. Please login with your new password.");

        foreach (var error in result.Errors)
            ModelState.AddModelError(error.Code, error.Description);

        return BadRequest(ModelState);
    }
}