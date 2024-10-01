using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using PasswordGenerator;
using SpaManagementSystem.Domain.Enums;
using SpaManagementSystem.Infrastructure.Identity.Entities;
using SpaManagementSystem.Application.Dto;
using SpaManagementSystem.Application.Interfaces;
using SpaManagementSystem.Application.Requests.Auth;
using SpaManagementSystem.WebApi.Models;
using SpaManagementSystem.WebApi.Extensions;

namespace SpaManagementSystem.WebApi.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(SignInManager<User> signInManager, ITokenService tokenService, IEmailSender<User> emailSender,
    IRefreshTokenService refreshTokenService) : BaseController
{
    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync([FromBody] UserRegisterRequest request)
    {
        var user = new User 
        { 
            UserName = request.Email,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber 
        };

        var result = await signInManager.UserManager.CreateAsync(user, request.Password);
        if (result.Succeeded)
        {
            try
            {
                await signInManager.UserManager.AddToRoleAsync(user, RoleTypes.Admin.ToString());
                
                return CreatedAtAction(
                    actionName: nameof(UserController.GetUserByIdAsync),
                    controllerName: "User",
                    routeValues: new { userId = user.Id },
                    value: user
                );
            }
            catch
            {
                await signInManager.UserManager.RemoveFromRoleAsync(user, RoleTypes.Admin.ToString());
                await signInManager.UserManager.DeleteAsync(user);

                throw;
            }
        }

        var errors = result.Errors.ToDictionary(error => error.Code, error => new[] { error.Description });
        
        return BadRequest(new ValidationErrorResponse { Errors = errors });
    }
    
    [Authorize(Roles = "Admin, Manager")]
    [HttpPost("register-employee")]
    public async Task<IActionResult> RegisterEmployeeAsync([FromBody] RegisterEmployeeRequest request)
    {
        var user = new User
        {
            UserName = request.Email,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            EmailConfirmed = true
        };

        var userRandomPassword = new Password(15)
            .IncludeLowercase().IncludeUppercase().IncludeSpecial().IncludeNumeric().Next();

        var result = await signInManager.UserManager.CreateAsync(user, userRandomPassword);
        if (result.Succeeded)
        {
            try
            {
                await signInManager.UserManager.AddToRoleAsync(user, RoleTypes.Employee.ToString());
                
                return CreatedAtAction(
                    actionName: nameof(UserController.GetUserByIdAsync),
                    controllerName: "User",
                    routeValues: new { userId = user.Id },
                    value: user
                );
            }
            catch
            {
                await signInManager.UserManager.RemoveFromRoleAsync(user, RoleTypes.Employee.ToString());
                await signInManager.UserManager.DeleteAsync(user);

                throw;
            }
        }
        
        var errors = result.Errors.ToDictionary(error => error.Code, error => new[] { error.Description });
        
        return BadRequest(new ValidationErrorResponse { Errors = errors });
    }
    
    [HttpPost("sign-in")]
    public async Task<IActionResult> SignInAsync([FromBody] SignInRequest request)
    {
        var user = await signInManager.UserManager.FindByEmailAsync(request.Email);
        if (user == null)
            return this.UnauthorizedResponse("Invalid Credentials");
            

        var signInResult =
            await signInManager.PasswordSignInAsync(request.Email, request.Password, false,
                lockoutOnFailure: false);
        
        if (signInResult.Succeeded)
        {
            var userRoles = await signInManager.UserManager.GetRolesAsync(user);
            var accessToken = tokenService.CreateJwtToken(new UserDto(user.Id, user.Email!, user.PhoneNumber!, userRoles));
            var refreshToken = tokenService.CreateRefreshToken(user.Id);
            
            await refreshTokenService.SaveRefreshToken(refreshToken);

            var authResponse = new AuthResponseDto(accessToken.Token, accessToken.ExpirationTime, refreshToken.Token,
                refreshToken.ExpirationTime);

            return this.OkResponse<object>(authResponse, "Login successful");
        }

        if (signInResult.IsNotAllowed)
        {
            var validPassword = await signInManager.UserManager.CheckPasswordAsync(user, request.Password);
            if (!user.EmailConfirmed && validPassword)
                return this.ForbiddenResponse("Email not confirmed.");
        }

        return this.UnauthorizedResponse("Invalid Credentials");
    }
    
    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshAsync([FromBody] RefreshRequest request)
    {
        ClaimsPrincipal principal;

        try
        {
            principal = tokenService.GetPrincipalFromExpiredToken(request.AccessToken);
        }
        catch(SecurityTokenException e)
        {
            return this.BadRequestResponse($"Access Token Error: {e.Message}");
        }

        if (!Guid.TryParse(principal.Identity?.Name, out var userId))
            return this.BadRequestResponse("Access Token error: Invalid user ID in the Access Token.");

        var isValidRefreshToken = await refreshTokenService.IsValidToken(userId, request.RefreshToken);
        if (!isValidRefreshToken)
            return this.BadRequestResponse("Refresh Token error: The refresh token is either invalid or has expired.");

        var user = await signInManager.UserManager.FindByIdAsync(userId.ToString());
        if (user == null)
            return this.BadRequestResponse("Access Token error: he user associated with the ID in the token does not exist.");
        
        var userRoles = await signInManager.UserManager.GetRolesAsync(user);
        if (!userRoles.Any())
            return this.BadRequestResponse("The user roles could not be retrieved.");
        
        var newJwt = tokenService.CreateJwtToken(new UserDto(user.Id, user.Email!, user.PhoneNumber!, userRoles));
        var newRefreshToken = tokenService.CreateRefreshToken(userId);

        await refreshTokenService.SaveRefreshToken(newRefreshToken);

        var authResponse = new AuthResponseDto(newJwt.Token, newJwt.ExpirationTime, newRefreshToken.Token,
            newRefreshToken.ExpirationTime);

        return this.OkResponse(authResponse, "The refresh token operation was successful.");
    }
    
    [HttpPost("confirm-email")]
    public async Task<IActionResult> ConfirmEmailAsync([FromBody] ConfirmEmailRequest request)
    {
        var user = await signInManager.UserManager.FindByEmailAsync(request.Email);
            
        var confirmationFailedMsg = "Email confirmation failed. Please verify your email confirmation token.";

        if (user == null)
            return this.BadRequestResponse(confirmationFailedMsg);

        var result = await signInManager.UserManager.ConfirmEmailAsync(user, request.Token);
        if (result.Succeeded)
            return this.OkResponse("Email has been confirmed successfully.");

        return this.BadRequestResponse(confirmationFailedMsg);
    }
    
    [HttpPost("send-confirmation-email")]
    public async Task<IActionResult> SendConfirmationEmailAsync([FromBody] SendConfirmationEmailRequest request)
    {
        var user = await signInManager.UserManager.FindByEmailAsync(request.Email);
        if (user == null || user.EmailConfirmed)
            return this.OkResponse("An email with a confirmation token has been sent to the address provided.");
        
        var token = await signInManager.UserManager.GenerateEmailConfirmationTokenAsync(user);

        if (string.IsNullOrWhiteSpace(token))
            return this.InternalServerErrorResponse("Failed to generate email confirmation token.");
        
        await emailSender.SendConfirmationLinkAsync(user, user.Email!, token);

        return this.OkResponse("An email with a confirmation token has been sent to the address provided.");
    }
    
    [Authorize]
    [HttpPost("send-confirmation-change-email")]
    public async Task<IActionResult> ChangeEmailAsync([FromBody] ChangeEmailRequest request)
    {
        var user = await signInManager.UserManager.FindByIdAsync(UserId.ToString());
        if (user == null)
            return this.BadRequestResponse("An error occurred while changing email. Please try again.");

        var token = await signInManager.UserManager.GenerateChangeEmailTokenAsync(user, request.NewEmail);

        if (string.IsNullOrWhiteSpace(token))
            return this.InternalServerErrorResponse("Failed to generate email confirmation token.");

        await emailSender.SendConfirmationLinkAsync(user, user.Email!, token);

        return this.OkResponse("A confirmation token for the email change has been sent.");
    }
    
    [Authorize]
    [HttpPost("confirm-changed-email")]
    public async Task<IActionResult> ConfirmChangedEmail([FromBody] ConfirmationChangeEmailRequest request)
    {
        var user = await signInManager.UserManager.FindByIdAsync(UserId.ToString());
        if (user == null)
            return this.BadRequestResponse("An error occurred while changing  email. Please try again.");

        var result = await signInManager.UserManager.ChangeEmailAsync(user, request.NewEmail, request.Token);
            
        if (result.Succeeded)
        {
            await signInManager.UserManager.SetUserNameAsync(user, request.NewEmail);
            user.EmailConfirmed = false;
            await signInManager.UserManager.UpdateAsync(user);

            return this.OkResponse("Email address has been successfully changed. Confirm new address");
        }

        var errors = result.Errors.ToDictionary(error => error.Code, error => new[] { error.Description });
        
        return BadRequest(new ValidationErrorResponse { Errors = errors });
    }
    
    [Authorize]
    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePasswordAsync([FromBody] ChangePasswordRequest request)
    {
        var user = await signInManager.UserManager.FindByIdAsync(UserId.ToString());
        if (user == null)
            return this.BadRequestResponse("An error occured while changing password. Please try again.");

        var result = await signInManager.UserManager
            .ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);

        if (result.Succeeded)
            return this.OkResponse("Password changed successfully.");

        var errors = result.Errors.ToDictionary(error => error.Code, error => new[] { error.Description });
        
        return BadRequest(new ValidationErrorResponse { Errors = errors });
    }
    
    [HttpPost("send-reset-password-token")]
    public async Task<IActionResult> SendResetPasswordTokenAsync([FromBody] SendResetPasswordTokenRequest request)
    {
        var user = await signInManager.UserManager.FindByEmailAsync(request.Email);
        if (user != null)
        {
            var token = await signInManager.UserManager.GeneratePasswordResetTokenAsync(user);

            if (string.IsNullOrWhiteSpace(token))
                return this.InternalServerErrorResponse("An error occurred while generating the password reset token.");

            await emailSender.SendPasswordResetCodeAsync(user, user.Email!, token);
        }

        return this.OkResponse("If an account with that email exists, a password reset token has been sent.");
    }
    
    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPasswordAsync([FromBody] ResetPasswordRequest request)
    {
        var user = await signInManager.UserManager.FindByEmailAsync(request.Email);
        if (user == null)
            return this.BadRequestResponse("An error occured while changing password. Please try again.");

        var result = await signInManager.UserManager.ResetPasswordAsync(user, request.Token, request.NewPassword);
        if (result.Succeeded)
            return this.OkResponse("Password has been reset successfully.");

        var errors = result.Errors.ToDictionary(error => error.Code, error => new[] { error.Description });
        
        return BadRequest(new ValidationErrorResponse { Errors = errors });
    }
}