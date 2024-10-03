using System.Net;

namespace SpaManagementSystem.Infrastructure.Exceptions;

public class EmailSendException : Exception
{
    public HttpStatusCode StatusCode { get; }
    
    public EmailSendException(string message, HttpStatusCode statusCode) : base(message)
    {
        StatusCode = statusCode;
    }

    public EmailSendException(string message, HttpStatusCode statusCode, Exception innerException) : base(message, innerException)
    {
        StatusCode = statusCode;
    }
}