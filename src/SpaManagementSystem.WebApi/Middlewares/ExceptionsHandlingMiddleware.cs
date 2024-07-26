using System.Net;
using System.Security.Authentication;
using Newtonsoft.Json;
using SpaManagementSystem.Application.Exceptions;
using SpaManagementSystem.Infrastructure.Exceptions;

namespace SpaManagementSystem.WebApi.Middlewares;

/// <summary>
/// Middleware to handle exceptions globally across the application.
/// This middleware catches exceptions thrown from the next delegates
/// in the pipeline and returns appropriate HTTP responses.
/// </summary>
public class ExceptionsHandlingMiddleware
{
    private readonly RequestDelegate _next;


        
    /// <summary>
    /// Constructor for the middleware.
    /// </summary>
    /// <param name="next">Next delegate in the HTTP request processing pipeline.</param>
    public ExceptionsHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }


        
    /// <summary>
    /// Invokes the next middleware in the pipeline and handles any exceptions that occur.
    /// </summary>
    /// <param name="context">HTTP context for the current request.</param>
    /// <returns>A task representing the completion of exception handling.</returns>
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }
        
    /// <summary>
    /// Handles exceptions by mapping them to appropriate HTTP status codes and creating a JSON response.
    /// </summary>
    /// <param name="context">HTTP context for the current request.</param>
    /// <param name="ex">The exception that occurred.</param>
    /// <returns>A task that completes when the exception response has been sent.</returns>
    private Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        var code = HttpStatusCode.InternalServerError;

        code = ex switch
        {
            ArgumentNullException => HttpStatusCode.BadRequest,
            ArgumentException => HttpStatusCode.BadRequest,
            UnauthorizedAccessException => HttpStatusCode.Unauthorized,
            NotFoundException => HttpStatusCode.NotFound,
            InvalidOperationException => HttpStatusCode.Conflict,
            InvalidCredentialException => HttpStatusCode.BadRequest,
            MissingConfigurationException => HttpStatusCode.BadRequest,
            _ => code
        };

        var result = JsonConvert.SerializeObject(new { Error = ex.Message, Code = code });

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)code;

        return context.Response.WriteAsync(result);
    }
}