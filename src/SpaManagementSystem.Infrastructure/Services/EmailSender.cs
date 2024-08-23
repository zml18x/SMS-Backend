using Microsoft.AspNetCore.Identity;
using SpaManagementSystem.Application.Interfaces;
using SpaManagementSystem.Infrastructure.Identity.Entities;

namespace SpaManagementSystem.Infrastructure.Services;

public class EmailSender(IEmailService emailService) : IEmailSender<User>
{
    public async Task SendConfirmationLinkAsync(User user, string email, string confirmationLink)
    {
        var subject = "SMS - Account Confirmation Email";
        var message = $"Confirmation token: {confirmationLink}";

        await emailService.SendEmailAsync(email, subject, message);
    }
    
    public async Task SendConfirmationChangeEmailAsync(User user, string email, string token)
    {
        var subject = "SMS - Account Change Email";
        var message = $"Confirmation change email token: {token}";

        await emailService.SendEmailAsync(email, subject, message);
    }

    public async Task SendPasswordResetCodeAsync(User user, string email, string resetCode)
    {
        var subject = "SMS - Password reset";
        var message = $"Password reset code: {resetCode}";

        await emailService.SendEmailAsync(email, subject, message);
    }

    public async Task SendPasswordResetLinkAsync(User user, string email, string resetLink)
    {
        var subject = "SMS - Password reset";
        var message = $"Password reset link: {resetLink}";

        await emailService.SendEmailAsync(email, subject, message);
    }
}