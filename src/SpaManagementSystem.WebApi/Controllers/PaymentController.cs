using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SpaManagementSystem.Application.Interfaces;
using SpaManagementSystem.Application.Requests.Payment;
using SpaManagementSystem.WebApi.Extensions;

namespace SpaManagementSystem.WebApi.Controllers;

[ApiController]
[Route("api/payment")]
public class PaymentController(IPaymentService paymentService) : BaseController
{
    [HttpPost("appointment/{appointmentId}")]
    [Authorize(Roles = "Admin, Manager, Employee")]
    public async Task<IActionResult> CreateAppointmentPaymentAsync(Guid appointmentId, [FromBody] CreateAppointmentPaymentRequest request)
    {
        var payment = await paymentService.CreateAppointmentPaymentAsync(appointmentId, request);
        
        return CreatedAtAction(
            actionName: nameof(GetPaymentDetailsAsync),
            controllerName: "Payment",
            routeValues: new { paymentId = payment.Id },
            value: payment
        );
    }

    [HttpGet("{paymentId:guid}")]
    [Authorize(Roles = "Admin, Manager, Employee")]
    public async Task<IActionResult> GetPaymentDetailsAsync(Guid paymentId)
    {
        var payment = await paymentService.GetByIdAsync(paymentId);

        return this.OkResponse(payment, "Payment retrieved successfully");
    }

    [HttpGet("appointment/{appointmentId:guid}")]
    [Authorize(Roles = "Admin, Manager, Employee")]
    public async Task<IActionResult> GetPaymentsForAppointmentAsync(Guid appointmentId)
    {
        var payments = await paymentService.GetPaymentsForAppointmentAsync(appointmentId);
        
        return this.OkResponse(payments, "Payments retrieved successfully");
    }

    [HttpGet("customer/{customerId:guid}")]
    [Authorize(Roles = "Admin, Manager, Employee")]
    public async Task<IActionResult> GetPaymentsForCustomerAsync(
        Guid customerId,
        [FromQuery] DateOnly? startDate = null,
        [FromQuery] DateOnly? endDate = null)
    {
        var payments = await paymentService.GetPaymentsForCustomerAsync(customerId, startDate, endDate);
        
        return this.OkResponse(payments, "Payments retrieved successfully");
    }
    
    [HttpPatch("{paymentId:guid}")]
    [Authorize(Roles = "Admin, Manager, Employee")]
    public async Task<IActionResult> UpdatePaymentStatus(Guid paymentId, [FromBody] UpdatePaymentStatusRequest request)
    {
        await paymentService.UpdatePaymentStatusAsync(paymentId, request);

        return NoContent();
    }
}