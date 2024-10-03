using SendGrid;
using SendGrid.Helpers.Mail;
using Microsoft.Extensions.Configuration;
using SpaManagementSystem.Application.Interfaces;
using SpaManagementSystem.Infrastructure.Exceptions;

namespace SpaManagementSystem.Infrastructure.Services;

/// <summary>
/// Implements the <see cref="IEmailService"/> using SendGrid to send emails.
/// This service is configured via application settings and requires a valid SendGrid API key and sender email.
/// </summary>
public class EmailService(IConfiguration configuration) : IEmailService
{
    public async Task SendEmailAsync(string email, string subject, string message)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException(
                "Failed to send email. Email address cannot be null, empty, or whitespace. ", nameof(email));

        var client = new SendGridClient(configuration["SENDGRID_API_KEY"]);

        var msg = new SendGridMessage()
        {
            From = new EmailAddress(configuration["SENDGRID_SENDER_EMAIL"]),
            Subject = subject,
            PlainTextContent = message,
            HtmlContent = $"<strong>{message}</strong>"
        };

        msg.AddTo(new EmailAddress($"{email}"));
        
        var response = await client.SendEmailAsync(msg).ConfigureAwait(false);

        if (!response.IsSuccessStatusCode)
        {
            var errorMessage = await response.Body.ReadAsStringAsync();
            
            throw new EmailSendException($"Message: {errorMessage}", response.StatusCode);
        }
    }
}