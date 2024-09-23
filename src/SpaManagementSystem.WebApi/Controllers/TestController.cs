using Microsoft.AspNetCore.Mvc;

namespace SpaManagementSystem.WebApi.Controllers;

[ApiController]
[Route("api/test")]
public class TestController : BaseController
{
    /// <summary>
    /// Gets a response indicating that the API is functioning properly.
    /// </summary>
    /// <remarks>
    /// This endpoint can be used to verify if the API is up and running. It simply returns a message confirming that the API is working.
    /// </remarks>
    /// <returns>
    /// A string message indicating the API status.
    /// </returns>
    /// <response code="200">API is working as expected.</response>
    /// <response code="400">Bad request - possibly due to invalid parameters.</response>
    /// <response code="500">Returned if an unexpected error occurs during the processing of the request.</response>
    [HttpGet]
    public IActionResult Get()
        => Ok("API is working.");
}