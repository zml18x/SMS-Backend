using Microsoft.AspNetCore.Identity;
using SpaManagementSystem.Application.Interfaces;
using SpaManagementSystem.Infrastructure.Identity.Entities;

namespace SpaManagementSystem.Infrastructure.Services;

/// <summary>
/// Implementation of the IEmailSender&lt;User&gt; interface for sending various types of email notifications.
/// Utilizes an IEmailService to handle the actual email sending.
/// </summary>
public class EmailSender : IEmailSender<User>
{
    private readonly IEmailService _emailService;


        
    /// <summary>
    /// Initializes a new instance of the EmailSender class.
    /// </summary>
    /// <param name="emailService">An instance of IEmailService used to send emails.</param>
    public EmailSender(IEmailService emailService)
    {
        _emailService = emailService;
    }

        
        
    public async Task SendConfirmationLinkAsync(User user, string email, string confirmationLink)
    {
        var subject = "SMS - Account Confirmation Email";
        var message = $"Confirmation token: {confirmationLink}";

        await _emailService.SendEmailAsync(email, subject, message);
    }
        
    /// <summary>
    /// Sends a confirmation token to the specified email address for email change confirmation.
    /// </summary>
    /// <param name="user">The user for whom the confirmation token is being sent.</param>
    /// <param name="email">The email address to send the confirmation token to.</param>
    /// <param name="token">The confirmation token to include in the email.</param>
    public async Task SendConfirmationChangeEmailAsync(User user, string email, string token)
    {
        var subject = "SMS - Account Change Email";
        var message = $"Confirmation change email token: {token}";

        await _emailService.SendEmailAsync(email, subject, message);
    }

    public async Task SendPasswordResetCodeAsync(User user, string email, string resetCode)
    {
        var subject = "SMS - Password reset";
        var message = $"Password reset code: {resetCode}";

        await _emailService.SendEmailAsync(email, subject, message);
    }

    public async Task SendPasswordResetLinkAsync(User user, string email, string resetLink)
    {
        var subject = "SMS - Password reset";
        var message = $"Password reset link: {resetLink}";

        await _emailService.SendEmailAsync(email, subject, message);
    }
}