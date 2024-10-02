using Microsoft.AspNetCore.Mvc;
using SpaManagementSystem.WebApi.Extensions;

namespace SpaManagementSystem.WebApi.Controllers;

[ApiController]
[Route("api/test")]
public class TestController : BaseController
{
    [HttpGet]
    public IActionResult Get()
        => this.OkResponse("API is working.");
}