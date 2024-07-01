namespace SpaManagementSystem.Infrastructure.Exceptions
{
    /// <summary>
    /// Represents errors that occur during application execution when a required configuration setting is missing.
    /// This exception is used to signify issues where configuration data necessary for the proper functioning
    /// of the application is not found, potentially due to issues in setup or deployment environments.
    /// </summary>
    public class MissingConfigurationException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MissingConfigurationException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public MissingConfigurationException(string? message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MissingConfigurationException"/> class with a specified error message and a reference
        /// to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference
        /// if no inner exception is specified.</param>
        public MissingConfigurationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}