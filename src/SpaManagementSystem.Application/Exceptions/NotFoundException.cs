namespace SpaManagementSystem.Application.Exceptions;

/// <summary>
/// Exception that should be thrown when a requested entity or resource is not found.
/// This exception is typically used within the context of handling requests for data that does not exist.
/// </summary>
public class NotFoundException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="NotFoundException"/> class with a specific error message.
    /// </summary>
    /// <param name="message">The message that describes the error. This message is intended to be understood by the developer who reads it.</param>
    public NotFoundException(string? message) : base(message) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="NotFoundException"/> class with a specific error message and a reference
    /// to the inner exception that is the cause of this exception.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    /// <param name="innerException">The exception that is the cause of the current exception, or a null reference
    /// if no inner exception is specified. This allows for exceptions to be chained and for more detailed error handling.</param>
    public NotFoundException(string message, Exception innerException) : base(message, innerException) { }
}