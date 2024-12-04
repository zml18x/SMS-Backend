using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SpaManagementSystem.Domain.Enums;
using SpaManagementSystem.Application.Interfaces;
using SpaManagementSystem.Application.Requests.Appointment;
using SpaManagementSystem.WebApi.Extensions;

namespace SpaManagementSystem.WebApi.Controllers;

[ApiController]
[Route("api/appointment")]
public class AppointmentController(IAppointmentService appointmentService) : BaseController
{
    [HttpPost]
    [Authorize(Roles = "Admin, Manager, Employee")]
    public async Task<IActionResult> CreateAppointmentAsync([FromBody] CreateAppointmentRequest request)
    {
        var appointment = await appointmentService.CreateAppointmentAsync(request);
        
        return CreatedAtAction(
            actionName: nameof(GetAppointmentAsync),
            controllerName: "Appointment",
            routeValues: new { appointmentId = appointment.Id },
            value: appointment
        );
    }

    [HttpGet("{appointmentId:guid}")]
    [Authorize(Roles = "Admin, Manager, Employee")]
    public async Task<IActionResult> GetAppointmentAsync(Guid appointmentId)
    {
        var appointment = await appointmentService.GetAppointmentByIdAsync(appointmentId);

        return this.OkResponse(appointment, "Appointment retrieved successfully");
    }
    
    [HttpGet("salon/{salonId:guid}")]
    [Authorize(Roles = "Admin, Manager, Employee")]
    public async Task<IActionResult> GetAppointmentsForSalonAsync(
        Guid salonId,
        [FromQuery] DateOnly? startDate = null,
        [FromQuery] DateOnly? endDate = null,
        [FromQuery] AppointmentStatus? status = null)
    {
        var appointments =
            await appointmentService.GetAppointmentsBySalonIdAsync(salonId, startDate, endDate, status);
        
        return this.OkResponse(appointments, "Appointments retrieved successfully");
    }
    
    [HttpGet("employee/{employeeId:guid}")]
    [Authorize(Roles = "Admin, Manager, Employee")]
    public async Task<IActionResult> GetAppointmentsForEmployeeAsync(
        Guid employeeId,
        [FromQuery] DateOnly? startDate = null,
        [FromQuery] DateOnly? endDate = null,
        [FromQuery] AppointmentStatus? status = null)
    {
        var appointments =
            await appointmentService.GetAppointmentsByEmployeeIdAsync(employeeId, startDate, endDate, status);
        
        return this.OkResponse(appointments, "Appointments retrieved successfully");
    }
    
    [HttpGet("customer/{customerId:guid}")]
    [Authorize(Roles = "Admin, Manager, Employee")]
    public async Task<IActionResult> GetAppointmentsForCustomerAsync(
        Guid customerId,
        [FromQuery] DateOnly? startDate = null,
        [FromQuery] DateOnly? endDate = null,
        [FromQuery] AppointmentStatus? status = null)
    {
        var appointments =
            await appointmentService.GetAppointmentsByCustomerIdAsync(customerId, startDate, endDate, status);
        
        return this.OkResponse(appointments, "Appointments retrieved successfully");
    }
    
    [HttpPatch("{appointmentId:guid}/status")]
    [Authorize(Roles = "Admin, Manager, Employee")]
    public async Task<IActionResult> UpdateAppointmentStatusAsync(
        Guid appointmentId,
        [FromBody] ChangeAppointmentStatusRequest request)
    {
        await appointmentService.UpdateStatusAsync(appointmentId, request.Status);
        
        return NoContent();
    }

    [HttpPatch("{appointmentId:guid}")]
    [Authorize(Roles = "Admin, Manager, Employee")]
    public async Task<IActionResult> UpdateAppointmentAsync(Guid appointmentId, [FromBody] UpdateAppointmentRequest request)
    {
        await appointmentService.UpdateAppointmentAsync(appointmentId, request);
        
        return NoContent();
    }

    [HttpDelete("{appointmentId:guid}")]
    [Authorize(Roles = "Admin, Manager, Employee")]
    public async Task<IActionResult> DeleteAppointmentAsync(Guid appointmentId)
    {
        await appointmentService.DeleteAppointmentAsync(appointmentId);
        
        return NoContent();
    }
}