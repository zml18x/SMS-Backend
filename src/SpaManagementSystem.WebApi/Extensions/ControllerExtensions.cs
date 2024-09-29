using Microsoft.AspNetCore.Mvc;
using SpaManagementSystem.WebApi.Controllers;
using SpaManagementSystem.WebApi.Models;

namespace SpaManagementSystem.WebApi.Extensions;

public static class ControllerExtensions
{
    public static IActionResult BadRequestResponse(this BaseController controller, string message)
        => CreateResponse<object>(StatusCodes.Status400BadRequest, message, false);

    public static IActionResult UnauthorizedResponse(this BaseController controller, string message)
        => CreateResponse<object>(StatusCodes.Status401Unauthorized, message, false);

    public static IActionResult ForbiddenResponse(this BaseController controller, string message)
        => CreateResponse<object>(StatusCodes.Status403Forbidden, message, false);

    public static IActionResult OkResponse(this BaseController controller, string message)
        => CreateResponse<object>(StatusCodes.Status200OK, message, true);
    
    public static IActionResult OkResponse<T>(this BaseController controller,T data, string message)
        => CreateResponse<object>(StatusCodes.Status200OK, message, true, data);

    public static IActionResult InternalServerErrorResponse(this BaseController controller, string message)
        => CreateResponse<object>(StatusCodes.Status500InternalServerError, message, false);

    private static IActionResult CreateResponse<T>(int statusCode, string message, bool success, T? data = null) where T : class
    {
        var response = new ApiResponse<T>()
        {
            Success = success,
            Status = statusCode,
            Message = message,
            Data = data
        };

        return new ObjectResult(response) { StatusCode = statusCode };
    }
}