using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Authorization;
using SpaManagementSystem.Application.Interfaces;
using SpaManagementSystem.Application.Requests.Customer;
using SpaManagementSystem.WebApi.Models;
using SpaManagementSystem.WebApi.Extensions;

namespace SpaManagementSystem.WebApi.Controllers;

[ApiController]
[Route("api/customer")]
public class CustomerController(ICustomerService customerService) : BaseController
{
    [HttpPost]
    [Authorize(Roles = "Admin, Manager, Employee")]
    public async Task<IActionResult> CreateCustomerAsync([FromBody] CreateCustomerRequest request)
    {
        var customer = await customerService.CreateCustomerAsync(request);
        
        return CreatedAtAction(
            actionName: nameof(GetCustomerByIdAsync),
            controllerName: "Customer",
            routeValues: new { customerId = customer.Id },
            value: customer
        );
    }

    [HttpGet("{customerId:guid}")]
    [Authorize(Roles = "Admin, Manager, Employee")]
    public async Task<IActionResult> GetCustomerByIdAsync(Guid customerId)
    {
        var customer = await customerService.GetCustomerByIdAsync(customerId);

        return this.OkResponse(customer, "Successful retrieved customer.");
    }

    [HttpGet]
    [Authorize(Roles = "Admin, Manager, Employee")]
    public async Task<IActionResult> GetCustomersAsync(
        [FromQuery] Guid salonId,
        [FromQuery] string? firstName = null,
        [FromQuery] string? lastName = null,
        [FromQuery] string? phoneNumber = null,
        [FromQuery] string? email = null,
        [FromQuery] bool? isActive = null)
    {
        var customers =
            await customerService.GetCustomersAsync(salonId, firstName, lastName, phoneNumber, email, isActive);
        
        return this.OkResponse(customers, "Successful retrieved customers.");
    }

    [HttpPatch("{customerId:guid}")]
    [Authorize(Roles = "Admin, Manager, Employee")]
    public async Task<IActionResult> UpdateCustomerAsync(Guid customerId,
        [FromBody] JsonPatchDocument<UpdateCustomerRequest> patchDocument)
    {
        var result = await customerService.UpdateCustomerAsync(customerId, patchDocument);

        return result.IsSuccess
            ? NoContent()
            : BadRequest(new ValidationErrorResponse { Errors = result.Errors });
    }

    [HttpDelete("{customerId:guid}")]
    [Authorize(Roles = "Admin, Manager")]
    public async Task<IActionResult> DeleteCustomerAsync(Guid customerId)
    {
        await customerService.DeleteAsync(customerId);
        
        return NoContent();
    }
}