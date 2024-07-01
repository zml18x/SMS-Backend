using SendGrid;
using SendGrid.Helpers.Mail;
using Microsoft.Extensions.Configuration;
using SpaManagementSystem.Application.Interfaces;

namespace SpaManagementSystem.Infrastructure.Services
{
    /// <summary>
    /// Implements the <see cref="IEmailService"/> using SendGrid to send emails.
    /// This service is configured via application settings and requires a valid SendGrid API key and sender email.
    /// </summary>
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;


        
        /// <summary>
        /// Initializes a new instance of the <see cref="EmailService"/> class with the specified configuration.
        /// </summary>
        /// <param name="configuration">The configuration repository that holds settings like SendGrid API keys and sender details.</param>
        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        
        /// <inheritdoc />
        /// <exception cref="ArgumentException">Thrown when the email address is null, empty, or whitespace.</exception>
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException(
                    "Failed to send email. Email address cannot be null, empty, or whitespace. ", nameof(email));

            var client = new SendGridClient(_configuration["SENDGRID_API_KEY"]);

            var msg = new SendGridMessage()
            {
                From = new EmailAddress(_configuration["SENDGRID_SENDER_EMAIL"]),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = $"<strong>{message}</strong>"
            };

            msg.AddTo(new EmailAddress($"{email}"));

            await client.SendEmailAsync(msg).ConfigureAwait(false);
        }
    }
}