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
        /// Registers a new user with the provided details and sends a confirmation email.
        /// </summary>
        /// <param name="request">The <see cref="RegisterRequest"/> object containing user registration details.</param>
        /// <returns>An <see cref="IActionResult"/> representing the result of the registration process.</returns>
        [HttpPost("Register")]
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
        /// <param name="request">The <see cref="SignInRequest"/> object containing user sign-in details.</param>
        /// <returns>An <see cref="IActionResult"/> containing the JWT token or an error message.</returns>
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
        /// <returns>An <see cref="IActionResult"/> containing the user's account details.</returns>
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
        /// Updates the current user's profile with the provided details.
        /// </summary>
        /// <param name="patchDocument">The <see cref="JsonPatchDocument{T}"/> object containing the patch operations to apply to the profile.</param>
        /// <param name="requestValidator">The <see cref="IValidator{T}"/> instance used to validate the updated profile request.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the success or failure of the update operation.</returns>
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
        /// <param name="request">The <see cref="ConfirmEmailRequest"/> object containing the email and token for confirmation.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the email confirmation process.</returns>
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
        /// Sends a confirmation email to the specified email address if it is not already confirmed.
        /// </summary>
        /// <param name="request">The <see cref="SendConfirmationEmailRequest"/> object containing the email address.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the confirmation email sending process.</returns>
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
        /// <param name="request">The <see cref="ChangeEmailRequest"/> object containing the new email address.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of sending the change email confirmation.</returns>
        [Authorize]
        [HttpPost("Manage/SendConfirmationChangeEmail")]
        public async Task<IActionResult> ChangeEmailAsync([FromBody] ChangeEmailRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _signInManager.UserManager.FindByIdAsync(UserId.ToString());

            if (user == null)
                return BadRequest("User account not found.");

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
        /// <param name="request">The <see cref="ConfirmationChangeEmailRequest"/> object containing the new email and confirmation token.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the email confirmation process.</returns>
        [Authorize]
        [HttpPost("Manage/ConfirmChangedEmail")]
        public async Task<IActionResult> ConfirmChangedEmail([FromBody] ConfirmationChangeEmailRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _signInManager.UserManager.FindByIdAsync(UserId.ToString());

            if (user == null)
                return BadRequest("User account not found.");

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
        /// <param name="request">The <see cref="ChangePasswordRequest"/> object containing the current and new passwords.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the success or failure of the password change operation.</returns>
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
        /// <param name="request">The <see cref="SendResetPasswordTokenRequest"/> object containing the email address to send the reset token.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the result of sending the password reset token.</returns>
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
        /// <param name="request">The <see cref="ResetPasswordRequest"/>
        /// object containing the email address, reset token, and new password.</param>
        /// <returns>An <see cref="IActionResult"/> indicating the success or failure of the password reset operation.</returns>
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