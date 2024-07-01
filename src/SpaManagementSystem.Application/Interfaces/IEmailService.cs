namespace SpaManagementSystem.Application.Interfaces
{
    /// <summary>
    /// Defines the contract for a service that handles sending emails.
    /// This interface is used to abstract the details of sending emails, allowing for different implementations
    /// that can handle various email sending mechanisms.
    /// </summary>
    public interface IEmailService
    {
        /// <summary>
        /// Sends an email to the specified email address.
        /// </summary>
        /// <param name="email">The recipient's email address.</param>
        /// <param name="subject">The subject line of the email.</param>
        /// <param name="message">The body text of the email.</param>
        /// <returns>A Task representing the asynchronous operation of sending an email.</returns>
        public Task SendEmailAsync(string email, string subject, string message);
    }
}