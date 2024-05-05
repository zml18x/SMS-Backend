using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using SpaManagementSystem.Application.Exceptions;
using SpaManagementSystem.Application.Interfaces;
using SpaManagementSystem.Application.Requests.UserAccount;
using SpaManagementSystem.Infrastructure.Identity.Enums;
using SpaManagementSystem.Infrastructure.Identity.Entities;

namespace SpaManagementSystem.WebApi.Controllers
{
    [Route("api/Account")]
    [ApiController]
    public class UserAccountController : ControllerBase
    {
        private Guid UserId => User.Identity!.IsAuthenticated ? Guid.Parse(User.Identity.Name!) : Guid.Empty;
        private readonly SignInManager<User> _signInManager;
        private readonly IUserService _userService;
        private readonly IJwtService _jwtService;
        private readonly IEmailSender<User> _emailSender;


        public UserAccountController(SignInManager<User> signInManager, IUserService userService,
            IJwtService jwtService, IEmailSender<User> emailSender)
        {
            _signInManager = signInManager;
            _userService = userService;
            _jwtService = jwtService;
            _emailSender = emailSender;
        }



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

        [HttpPost("SignIn")]
        public async Task<IActionResult> SignInAsync([FromBody] SignInRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result =
                await _signInManager.PasswordSignInAsync(request.Email, request.Password, false,
                    lockoutOnFailure: false);

            if (result.Succeeded)
            {
                var user = await _signInManager.UserManager.FindByEmailAsync(request.Email);

                if (user == null)
                    throw new NotFoundException("User account not found");

                var jwtDto = _jwtService.CreateToken(user.Id, user.Email!,
                    await _signInManager.UserManager.GetRolesAsync(user));

                return new OkObjectResult(jwtDto);
            }
            
            return BadRequest("Invalid Credentials");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAccountAsync()
        {
            var user = await _signInManager.UserManager.FindByIdAsync(UserId.ToString());

            if (user == null)
                return NotFound($"User with id {UserId} not found");

            var userProfileDto = await _userService.GetAccountDetailsAsync(user.Id, user.Email!, user.PhoneNumber!);

            return new OkObjectResult(userProfileDto);
        }
        
        [Authorize]
        [HttpPatch("UpdateProfile")]
        public async Task<IActionResult> UpdateProfileAsync([FromBody] UpdateProfileRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.UpdateProfileAsync(UserId, request.FirstName, request.LastName,
                request.Gender, request.DateOfBirth);

            if (result)
                return Ok("The profile has been updated successfully.");

            return BadRequest("No changes were made to the profile.");
        }
        
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
        
        [Authorize]
        [HttpPatch("Manage/ConfirmChangedEmail")]
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

                return Ok("Email address changed successfully.");
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError(error.Code, error.Description);

            return BadRequest(ModelState);
        }
        
        [Authorize]
        [HttpPatch("Manage/ChangePassword")]
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
        
        [HttpPatch("Manage/ResetPassword")]
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