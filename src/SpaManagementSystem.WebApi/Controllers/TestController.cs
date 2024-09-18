using Microsoft.AspNetCore.Mvc;

namespace SpaManagementSystem.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestController : BaseController
{
    [HttpGet]
    public IActionResult Get()
        => Ok("API is working.");
}