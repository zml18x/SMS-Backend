using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SpaManagementSystem.WebApi.Controllers;

[ApiController]
[Route("api/appointment")]
public class AppointmentController : BaseController
{
    [HttpPost]
    [Authorize(Roles = "Admin, Manager, Employee")]
    public async Task<IActionResult> CreateAppointmentAsync()
    {
        throw new NotImplementedException();
    }

    [HttpGet("{appointmentId:guid}")]
    [Authorize(Roles = "Admin, Manager, Employee")]
    public async Task<IActionResult> GetAppointmentAsync(Guid appointmentId)
    {
        throw new NotImplementedException();
    }
}