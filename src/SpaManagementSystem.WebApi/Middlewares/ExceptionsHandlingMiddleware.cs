using System.Net;
using System.Text.Json;
using System.Security.Authentication;
using SpaManagementSystem.Application.Exceptions;
using SpaManagementSystem.Infrastructure.Exceptions;

namespace SpaManagementSystem.WebApi.Middlewares;

public class ExceptionsHandlingMiddleware(RequestDelegate next)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }
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

        var result = JsonSerializer.Serialize(new { Error = ex.Message, Code = code });

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)code;

        return context.Response.WriteAsync(result);
    }
}