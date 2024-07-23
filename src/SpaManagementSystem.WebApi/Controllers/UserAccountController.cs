using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using SpaManagementSystem.Application.Dto;
using SpaManagementSystem.Application.Interfaces;
using SpaManagementSystem.Application.Requests.UserAccount;
using SpaManagementSystem.Infrastructure.Identity.Enums;
using SpaManagementSystem.Infrastructure.Identity.Entities;

namespace SpaManagementSystem.WebApi.Controllers
{
    /// <summary>
    /// Provides endpoints for user account management including registration, login, profile updates, and email verification.
    /// </summary>
    [Route("api/Account")]
    [ApiController]
    public class UserAccountController : BaseController
    {
        private readonly SignInManager<User> _signInManager;
        private readonly IUserService _userService;
        private readonly IJwtService _jwtService;
        private readonly IEmailSender<User> _emailSender;

        
        
        /// <summary>
        /// Initializes a new instance of the <see cref="UserAccountController"/> class.
        /// </summary>
        /// <param name="signInManager">The <see cref="SignInManager{User}"/> used for user sign-in operations.</param>
        /// <param name="userService">The <see cref="IUserService"/> used for user profile management.</param>
        /// <param name="jwtService">The <see cref="IJwtService"/> used for JWT token operations.</param>
        /// <param name="emailSender">The <see cref="IEmailSender{User}"/> used for sending emails.</param>
        public UserAccountController(SignInManager<User> signInManager, IUserService userService,
            IJwtService jwtService, IEmailSender<User> emailSender)
        {
            _signInManager = signInManager;
            _userService = userService;
            _jwtService = jwtService;
            _emailSender = emailSender;
        }


        
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

            var result = await _signInManager.UserManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {
                try
                {
                    await _signInManager.UserManager.AddToRoleAsync(user, RoleType.Admin.ToString());
                    await _userService.CreateProfileAsync(user.Id, request.FirstName, request.LastName, request.Gender,
                        request.DateOfBirth);
                    
                    var token = await _signInManager.UserManager.GenerateEmailConfirmationTokenAsync(user);

                    if (string.IsNullOrWhiteSpace(token))
                        return StatusCode(StatusCodes.Status500InternalServerError,
                            "Failed to generate email confirmation token.");

                    await _emailSender.SendConfirmationLinkAsync(user, user.Email, token);
                    
                    return Created("api/Account", null);
                }
                catch
                {
                    await _signInManager.UserManager.RemoveFromRoleAsync(user, RoleType.Admin.ToString());
                    await _signInManager.UserManager.DeleteAsync(user);

                    throw;
                }
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError(error.Code, error.Description);

            return BadRequest(ModelState);
        }
        
        /// <summary>
        /// Authenticates a user with the provided email and password, and returns a JWT token if successful.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST /Account/SignIn
        ///     {
        ///         "email": "example@mail.com",
        ///         "password": "pa$$w0rD!X"
        ///     }
        /// </remarks>
        /// <param name="request">The SignInRequest object containing user sign-in details.</param>
        /// <returns>An <see cref="IActionResult"/> containing the JWT token or an error message.</returns>
        /// <response code="200">Returns the JWT token if authentication is successful.</response>
        /// <response code="400">Returns an error message if the credentials are invalid or the email is not confirmed.</response>
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(JwtDto), StatusCodes.Status200OK)]
        [HttpPost("SignIn")]
        public async Task<IActionResult> SignInAsync([FromBody] SignInRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var user = await _signInManager.UserManager.FindByEmailAsync(request.Email);
            if (user == null)
                return BadRequest("Invalid Credentials");

            var signInResult =
                await _signInManager.PasswordSignInAsync(request.Email, request.Password, false,
                    lockoutOnFailure: false);
            
            if (signInResult.Succeeded)
            {
                var jwtDto = _jwtService.CreateToken(user.Id, user.Email!,
                    await _signInManager.UserManager.GetRolesAsync(user));

                return new OkObjectResult(jwtDto);
            }

            if (signInResult.IsNotAllowed)
            {
                var validPassword = await _signInManager.UserManager.CheckPasswordAsync(user, request.Password);
                if (!user.EmailConfirmed && validPassword)
                    return BadRequest("Please, confirm your email");
            }

            return BadRequest("Invalid Credentials");
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
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAccountAsync()
        {
            var user = await _signInManager.UserManager.FindByIdAsync(UserId.ToString());

            if (user == null)
                return NotFound($"User with id {UserId} not found");

            var userProfile = await _userService.GetProfileAsync(user.Id);

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
        [Authorize]
        [HttpPatch("UpdateProfile")]
        public async Task<IActionResult> UpdateProfileAsync( [FromBody] JsonPatchDocument<UpdateProfileRequest> patchDocument,
            [FromServices] IValidator<UpdateProfileRequest> requestValidator)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingUserProfile = await _userService.GetProfileAsync(UserId);

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

            var result = await _userService.UpdateProfileAsync(UserId, request);

            if (result)
                return Ok("The profile has been updated successfully.");

            return BadRequest("No changes were made to the profile.");
        }

        /// <summary>
        /// Confirms the user's email address with the provided token.
        /// </summary>
        /// <remarks>
        /// This endpoint allows users to confirm their email address using a confirmation token sent to their email.
        /// Sample request:
        /// 
        ///     POST /Account/Manage/ConfirmEmail
        ///     {
        ///         "email": "example@mail.com",
        ///         "token": "confirmationTokenHere"
        ///     }
        /// </remarks>
        /// <param name="request">The ConfirmEmailRequest object containing the email and token for confirmation.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the email confirmation process.</returns>
        /// <response code="200">Indicates that the email was confirmed successfully.</response>
        /// <response code="400">Indicates that the confirmation failed due to invalid token or email.</response>
        [Consumes("application/json")]
        [HttpPost("Manage/ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmailAsync([FromBody] ConfirmEmailRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _signInManager.UserManager.FindByEmailAsync(request.Email);
            
            var confirmationFailedMsg = "Email confirmation failed. Please verify your email confirmation token.";
            
            if (user == null)
                return BadRequest(confirmationFailedMsg);

            var result = await _signInManager.UserManager.ConfirmEmailAsync(user, request.Token);

            if (result.Succeeded)
                return Ok("Email confirmed successfully");

            return BadRequest(confirmationFailedMsg);
        }
        
        /// <summary>
        /// Sends a confirmation email to the user with the provided email address.
        /// </summary>
        /// <remarks>
        /// This endpoint sends a confirmation email to the specified address if it has not been confirmed yet.
        /// If the email is already confirmed or if the user does not exist, a corresponding message is returned.
        /// Sample request:
        /// 
        ///     POST /Account/Manage/SendConfirmationEmail
        ///     {
        ///         "email": "example@mail.com"
        ///     }
        /// </remarks>
        /// <param name="request">The SendConfirmationEmailRequest object containing the email address for sending the confirmation email.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the email sending process.</returns>
        /// <response code="200">Indicates that the confirmation email was sent successfully or that the email is already confirmed.</response>
        /// <response code="400">Indicates that the request is invalid.</response>
        /// <response code="500">Indicates an internal server error if the email confirmation token could not be generated.</response>
        [Consumes("application/json")]
        [HttpPost("Manage/SendConfirmationEmail")]
        public async Task<IActionResult> SendConfirmationEmailAsync([FromBody] SendConfirmationEmailRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _signInManager.UserManager.FindByEmailAsync(request.Email);

            var successMsg = "Confirmation email sent successfully. Please check your inbox, " +
                             "including the spam folder, for further instructions.";

            if (user == null)
                return Ok(successMsg);

            if (user.EmailConfirmed)
                return Ok("Email already confirmed.");
            

            var token = await _signInManager.UserManager.GenerateEmailConfirmationTokenAsync(user);

            if (string.IsNullOrWhiteSpace(token))
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Failed to generate email confirmation token.");

            await _emailSender.SendConfirmationLinkAsync(user, user.Email!, token);

            return Ok(successMsg);
        }
        
        /// <summary>
        /// Sends a confirmation email with a token to change the user's email address.
        /// </summary>
        /// <remarks>
        /// This endpoint sends a confirmation email with a token to the user's current email address to verify the change of their email address.
        /// If the user is not found or there is an error generating the token, a corresponding message is returned.
        /// Sample request:
        /// 
        ///     POST /Account/Manage/SendConfirmationChangeEmail
        ///     {
        ///         "newEmail": "newemail@example.com"
        ///     }
        /// </remarks>
        /// <param name="request">The ChangeEmailRequest object containing the new email address.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of sending the change email confirmation.</returns>
        /// <response code="200">Indicates that the confirmation email was sent successfully.</response>
        /// <response code="400">Indicates that the request is invalid.</response>
        /// <response code="401">Indicates that the user is not authenticated.</response>
        /// <response code="404">Indicates that the user profile was not found.</response>
        /// <response code="500">Indicates an internal server error if the change email token could not be generated.</response>
        [Consumes("application/json")]
        [Authorize]
        [HttpPost("Manage/SendConfirmationChangeEmail")]
        public async Task<IActionResult> ChangeEmailAsync([FromBody] ChangeEmailRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _signInManager.UserManager.FindByIdAsync(UserId.ToString());

            if (user == null)
                return NotFound("User account not found.");

            var token = await _signInManager.UserManager.GenerateChangeEmailTokenAsync(user, request.NewEmail);

            if (string.IsNullOrWhiteSpace(token))
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "An error occurred while generating the change email token.");

            await _emailSender.SendConfirmationLinkAsync(user, user.Email!, token);

            return Ok("Confirmation email sent successfully. Please check your email for further instructions.");
        }
        
        /// <summary>
        /// Confirms the user's email address change with the provided token.
        /// </summary>
        /// <remarks>
        /// This endpoint confirms the change of the user's email address using a confirmation token sent to the user's current email address.
        /// If the token is valid, the user's email address will be updated, and the confirmation status will be reset.
        /// Sample request:
        /// 
        ///     POST /Account/Manage/ConfirmChangedEmail
        ///     {
        ///         "newEmail": "newemail@example.com",
        ///         "token": "confirmationToken"
        ///     }
        /// </remarks>
        /// <param name="request">The ConfirmationChangeEmailRequest object containing the new email address and the confirmation token.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the email confirmation process.</returns>
        /// <response code="200">Indicates that the email address was successfully changed.</response>
        /// <response code="400">Indicates that the request is invalid.</response>
        /// <response code="401">Indicates that the user is not authenticated.</response>
        /// <response code="404">Indicates that the user profile was not found.</response>
        [Consumes("application/json")]
        [Authorize]
        [HttpPost("Manage/ConfirmChangedEmail")]
        public async Task<IActionResult> ConfirmChangedEmail([FromBody] ConfirmationChangeEmailRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _signInManager.UserManager.FindByIdAsync(UserId.ToString());

            if (user == null)
                return NotFound("User account not found.");

            var result = await _signInManager.UserManager.ChangeEmailAsync(user, request.NewEmail, request.Token);
            
            if (result.Succeeded)
            {
                await _signInManager.UserManager.SetUserNameAsync(user, request.NewEmail);
                user.EmailConfirmed = false;
                await _signInManager.UserManager.UpdateAsync(user);
                
                return Ok("Email address changed successfully.");
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError(error.Code, error.Description);

            return BadRequest(ModelState);
        }
        
        /// <summary>
        /// Changes the current user's password.
        /// </summary>
        /// <remarks>
        /// This endpoint allows the current user to change their password. It requires the current password and the new password to be provided.
        /// If the current password is incorrect or the new password does not meet the criteria, appropriate error messages will be returned.
        /// Sample request:
        /// 
        ///     POST /Account/Manage/ChangePassword
        ///     {
        ///         "currentPassword": "OldPa$$w0rD!",
        ///         "newPassword": "NewPa$$w0rD!"
        ///     }
        /// </remarks>
        /// <param name="request">The ChangePasswordRequest object containing the current and new passwords.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the success or failure of the password change operation.</returns>
        /// <response code="200">Indicates that the password was changed successfully.</response>
        /// <response code="400">Indicates that the request is invalid or the current password is incorrect.</response>
        /// <response code="401">Indicates that the user is not authenticated.</response>
        /// <response code="404">Indicates that the user account was not found.</response>
        [Consumes("application/json")]
        [Authorize]
        [HttpPost("Manage/ChangePassword")]
        public async Task<IActionResult> ChangePasswordAsync([FromBody] ChangePasswordRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _signInManager.UserManager.FindByIdAsync(UserId.ToString());

            if (user == null)
                return NotFound("User account not found");

            var result = await _signInManager.UserManager
                .ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);

            if (result.Succeeded)
                return Ok("Password changed successfully.");

            foreach (var error in result.Errors)
                ModelState.AddModelError(error.Code, error.Description);

            return BadRequest(ModelState);
        }
        
        /// <summary>
        /// Sends a password reset token to the specified email address.
        /// </summary>
        /// <remarks>
        /// This endpoint sends a password reset token to the provided email address. If the email is associated with an account, 
        /// a password reset email will be sent with instructions. If the email does not exist in the system, the response will 
        /// still be successful to avoid exposing whether an email is registered.
        /// 
        /// Sample request:
        /// 
        ///     POST /Account/Manage/SendResetPasswordToken
        ///     {
        ///         "email": "user@example.com"
        ///     }
        /// </remarks>
        /// <param name="request">The SendResetPasswordTokenRequest object containing the email address to send the reset token.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of sending the password reset token.</returns>
        /// <response code="200">Indicates that a password reset request has been sent successfully, and if the email exists, instructions have been sent.</response>
        /// <response code="400">Indicates that the request is invalid.</response>
        /// <response code="500">Indicates an internal server error occurred while generating the password reset token.</response>
        [Consumes("application/json")]
        [HttpPost("Manage/SendResetPasswordToken")]
        public async Task<IActionResult> SendResetPasswordTokenAsync([FromBody] SendResetPasswordTokenRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _signInManager.UserManager.FindByEmailAsync(request.Email);

            if (user != null)
            {
                var token = await _signInManager.UserManager.GeneratePasswordResetTokenAsync(user);

                if (string.IsNullOrWhiteSpace(token))
                    return StatusCode(StatusCodes.Status500InternalServerError,
                        "An error occurred while generating the password reset token.");

                await _emailSender.SendPasswordResetCodeAsync(user, user.Email!, token);
            }

            return Ok("A password reset request has been sent. If the provided email address exists in our system," +
                      " you will receive an email with instructions.");
        }
        
        /// <summary>
        /// Resets the user's password with the provided token and new password.
        /// </summary>
        /// <remarks>
        /// This endpoint allows users to reset their password using a reset token sent to their email address. The request should include
        /// the email address, reset token, and new password. If the provided token is valid and the new password meets the criteria,
        /// the password will be reset successfully. Otherwise, appropriate error messages will be returned.
        ///
        /// Sample request:
        /// 
        ///     POST /Account/Manage/ResetPassword
        ///     {
        ///         "email": "user@example.com",
        ///         "token": "resetPasswordToken",
        ///         "newPassword": "NewPa$$w0rD!"
        ///     }
        /// </remarks>
        /// <param name="request">The ResetPasswordRequest object containing the email address, reset token, and new password.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the success or failure of the password reset operation.</returns>
        /// <response code="200">Indicates that the password was reset successfully and the user can now log in with the new password.</response>
        /// <response code="400">Indicates that the request is invalid, the user account was not found, or the password reset failed.</response>
        [Consumes("application/json")]
        [HttpPost("Manage/ResetPassword")]
        public async Task<IActionResult> ResetPasswordAsync([FromBody] ResetPasswordRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _signInManager.UserManager.FindByEmailAsync(request.Email);

            if (user == null)
                return BadRequest("Password reset failed. Please check your email and try again.");

            var result = await _signInManager.UserManager.ResetPasswordAsync(user, request.Token, request.NewPassword);

            if (result.Succeeded)
                return Ok("Password reset successfully. Please login with your new password.");

            foreach (var error in result.Errors)
                ModelState.AddModelError(error.Code, error.Description);

            return BadRequest(ModelState);
        }
    }
}