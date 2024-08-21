using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Authorization;
using SpaManagementSystem.Application.Dto;
using SpaManagementSystem.Application.Interfaces;
using SpaManagementSystem.Application.Requests.Salon;
using SpaManagementSystem.Application.Requests.Common;

namespace SpaManagementSystem.WebApi.Controllers;

/// <summary>
/// Controller for managing salons. Provides endpoints for creating, retrieving, updating, and deleting salons.
/// </summary>
[Route("api/Salon")]
[ApiController]
public class SalonController : BaseController
{
    private readonly ISalonService _salonService;
        
        
        
    /// <summary>
    /// Initializes a new instance of the <see cref="SalonController"/> class with the specified <see cref="ISalonService"/>.
    /// </summary>
    /// <param name="salonService">The <see cref="ISalonService"/> instance used to perform salon-related operations.</param>
    public SalonController(ISalonService salonService)
    {
        _salonService = salonService;
    }
        
        
        
    /// <summary>
    /// Creates a new salon with the provided details.
    /// </summary>
    /// <remarks>
    /// This endpoint allows an authenticated admin to create a new salon by providing the necessary details in the request body.
    /// The request should include details such as the salon's name, address, and other relevant information. Upon successful 
    /// creation of the salon, a `201 Created` response is returned.
    ///
    /// Sample request:
    /// 
    ///     POST /Salon/Create
    ///     {
    ///         "name": "ExampleName",
    ///         "phoneNumber": "321321123"
    ///         "email": "example@mail.com"
    ///     }
    /// </remarks>
    /// <param name="request">The CreateSalonRequest object containing the details for the new salon.</param>
    /// <returns>A <see cref="IActionResult"/> indicating the result of the creation operation.</returns>
    /// <response code="201">Indicates that the salon was created successfully.</response>
    /// <response code="400">Indicates that the request is invalid due to bad request or validation errors.</response>
    /// <response code="401">If the user is not authenticated (authorization failed).</response>
    /// <response code="403">Indicates that the request is forbidden due to insufficient permissions.</response>
    [Produces("application/json")]
    [Consumes("application/json")]
    [Authorize(Roles = "Admin")]
    [HttpPost("Create")]
    public async Task<IActionResult> CreateAsync([FromBody] CreateSalonRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        await _salonService.CreateAsync(UserId, request);

        return Created("api/Salon", null);
    }
        
    /// <summary>
    /// Retrieves a list of all salons associated with the current user.
    /// </summary>
    /// <remarks>
    /// This endpoint allows an authenticated admin to retrieve a list of all salons that are associated with the current user.
    /// The request does not require any parameters. The response will contain a list of salon objects in JSON format.
    /// 
    /// Sample request:
    /// 
    ///     GET /Salon/List
    /// 
    /// </remarks>
    /// <returns>A <see cref="IActionResult"/> containing a JSON result with the list of salons.</returns>
    /// <response code="200">Returns a list of salons associated with the current user.</response>
    /// <response code="401">If the user is not authenticated (authorization failed).</response>
    /// <response code="403">Indicates that the request is forbidden due to insufficient permissions.</response>
    [Produces("application/json")]
    [ProducesResponseType(typeof(IEnumerable<SalonDto>), StatusCodes.Status200OK)]
    [Authorize(Roles = "Admin")]
    [HttpGet("List")]
    public async Task<IActionResult> GetAllSalonsAsync()
    {
        var salons = await _salonService.GetAllSalonsByUserIdAsync(UserId);

        return new JsonResult(salons);
    }
        
    /// <summary>
    /// Retrieves details for a specific salon by its ID.
    /// </summary>
    /// <param name="salonId">The unique identifier of the salon.</param>
    /// <remarks>
    /// This endpoint retrieves detailed information about a specific salon using its unique identifier (ID). 
    /// If the salon with the specified ID is found, the details are returned in JSON format.
    /// If no salon is found, a `404 Not Found` response is returned.
    /// 
    /// Sample request:
    /// 
    ///     GET /Salon/{salonId}
    /// 
    /// </remarks>
    /// <returns>A <see cref="IActionResult"/> containing the salon details if found, otherwise a NotFound result.</returns>
    /// <response code="200">Returns the details of the salon if found.</response>
    /// <response code="404">If the salon with the specified ID is not found.</response>
    /// <response code="401">If the request is not authenticated.</response>
    [Produces("application/json")]
    [ProducesResponseType(typeof(SalonDetailsDto), StatusCodes.Status200OK)]
    [Authorize("Admin, Manager, Employee")]
    [HttpGet("{salonId}")]
    public async Task<IActionResult> GetSalonAsync(Guid salonId)
    {
        var salon = await _salonService.GetSalonDetailsByIdAsync(salonId);

        return new JsonResult(salon);
    }

        
    /// <summary>
    /// Updates the details of a specific salon based on the provided JsonPatchDocument.
    /// </summary>
    /// <param name="salonId">The unique identifier of the salon to update.</param>
    /// <param name="patchDocument">The JsonPatchDocument object containing the details to update.</param>
    /// <param name="requestValidator">An instance of <see cref="IValidator{UpdateSalonDetailsRequest}"/> used to validate the updated salon details.</param>
    /// <remarks>
    /// This endpoint allows you to update specific details of a salon using a JSON patch document.
    /// The patch document should contain the operations to apply to the salon details.
    ///
    /// Sample request:
    /// 
    ///     PATCH /Salon/{salonId}/Manage/UpdateDetails
    ///     [
    ///         { "op": "replace", "path": "/name", "value": "New Salon Name" },
    ///         { "op": "replace", "path": "/phoneNumber", "value": "321321321" },
    ///         { "op": "replace", "path": "/email", "value": "newExample@mail.com" },
    ///         { "op": "replace", "path": "/description", "value": "Example description" }
    ///     ]
    /// </remarks>
    /// <returns>A <see cref="IActionResult"/> indicating the result of the update operation.
    /// Returns <see cref="NotFoundResult"/> if the salon is not found,
    /// <see cref="BadRequestResult"/> if the request is invalid,
    /// or <see cref="NoContentResult"/> if the update is successful.</returns>
    /// <response code="204">If the update was successful and no content is returned.</response>
    /// <response code="400">If the request is invalid or no changes were made to the salon.</response>
    /// <response code="401">If the request is not authenticated.</response>
    /// <response code="403">Indicates that the request is forbidden due to insufficient permissions.</response>
    /// <response code="404">If the salon with the specified ID is not found.</response>
    [Produces("application/json-patch+json")]
    [Authorize(Roles = "Admin, Manager")]
    [HttpPatch("{salonId}/Manage/UpdateDetails")]
    public async Task<IActionResult> UpdateDetailsAsync(Guid salonId, [FromBody] JsonPatchDocument<UpdateSalonDetailsRequest> patchDocument,
        [FromServices] IValidator<UpdateSalonDetailsRequest> requestValidator)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
            
        var existingSalon = await _salonService.GetSalonDetailsByIdAsync(salonId);

        var request = new UpdateSalonDetailsRequest(existingSalon.Name, existingSalon.Email,
            existingSalon.PhoneNumber, existingSalon.Description);

        patchDocument.ApplyTo(request, ModelState);

        var requestValidationResult = await requestValidator.ValidateAsync(request);
        if (!requestValidationResult.IsValid)
        {
            foreach (var error in requestValidationResult.Errors)
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);

            return BadRequest(ModelState);
        }

        var isUpdated = await _salonService.UpdateSalonAsync(salonId, request);
        if (!isUpdated)
            return BadRequest("No changes were made to the salon.");

        return NoContent();
    }
    
    /// <summary>
    /// Adds opening hours for a specific salon.
    /// </summary>
    /// <param name="salonId">The unique identifier of the salon to add opening hours to.</param>
    /// <param name="request">The <see cref="OpeningHoursRequest"/> object containing the opening hours details.</param>
    /// <remarks>
    /// Sample request:
    /// 
    ///     POST /Salon/{salonId}/Manage/OpeningHours
    ///     {
    ///         "DayOfWeek": 0,
    ///         "OpeningTime": "09:00:00",
    ///         "ClosingTime": "18:00:00"
    ///     }
    /// 
    /// </remarks>
    /// <returns>A <see cref="IActionResult"/> indicating the result of the add operation.</returns>
    /// <response code="201">Indicates that the opening hours were added successfully.</response>
    /// <response code="400">Indicates that the request is invalid due to bad request or validation errors.</response>
    /// <response code="401">If the request is not authenticated.</response>
    /// <response code="403">Indicates that the request is forbidden due to insufficient permissions.</response>
    [Consumes("application/json")]
    [Authorize(Roles = "Admin, Manager")]
    [HttpPost("{salonId}/Manage/OpeningHours")]
    public async Task<IActionResult> AddOpeningHoursAsync(Guid salonId, [FromBody] OpeningHoursRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        await _salonService.AddOpeningHoursAsync(salonId, request);
            
        return Created();
    }
    
    /// <summary>
    /// Updates opening hours for a specific salon.
    /// </summary>
    /// <param name="salonId">The unique identifier of the salon to update opening hours for.</param>
    /// <param name="request">The <see cref="OpeningHoursRequest"/> object containing the opening hours details.</param>
    /// <remarks>
    /// Sample request:
    /// 
    ///     PUT /Salon/{salonId}/Manage/OpeningHours
    ///     {
    ///         "DayOfWeek": 0,
    ///         "OpeningTime": "09:00:00",
    ///         "ClosingTime": "18:00:00"
    ///     }
    ///  
    /// </remarks>
    /// <returns>A <see cref="IActionResult"/> indicating the result of the update operation.</returns>
    /// <response code="204">Indicates that the opening hours were updated successfully.</response>
    /// <response code="400">Indicates that the request is invalid due to bad request or validation errors.</response>
    /// <response code="401">If the request is not authenticated.</response>
    /// <response code="403">Indicates that the request is forbidden due to insufficient permissions.</response>
    [Consumes("application/json")]
    [Authorize(Roles = "Admin, Manager")]
    [HttpPut("{salonId}/Manage/OpeningHours")]
    public async Task<IActionResult> UpdateOpeningHoursAsync(Guid salonId, [FromBody] OpeningHoursRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        await _salonService.UpdateOpeningHoursAsync(salonId, request);
            
        return NoContent();
    }
    
    /// <summary>
    /// Deletes opening hours for a specific salon.
    /// </summary>
    /// <param name="salonId">The unique identifier of the salon whose opening hours will be deleted.</param>
    /// <param name="dayOfWeek">The day of the week whose opening hours will be deleted.</param>
    /// <remarks>
    /// Sample request:
    /// 
    ///     DELETE /Salon/{salonId}/Manage/OpeningHours/{dayOfWeek}
    /// 
    /// </remarks>
    /// <returns>A <see cref="IActionResult"/> indicating the result of the delete operation.</returns>
    /// <response code="204">Indicates that the opening hours were deleted successfully.</response>
    /// <response code="400">Indicates that the request is invalid due to bad request or validation errors.</response>
    /// <response code="401">If the request is not authenticated.</response>
    /// <response code="403">Indicates that the request is forbidden due to insufficient permissions.</response>
    [Consumes("application/json")]
    [Authorize(Roles = "Admin, Manager")]
    [HttpDelete("{salonId}/Manage/OpeningHours/{dayOfWeek}")]
    public async Task<IActionResult> RemoveOpeningHoursAsync(Guid salonId, DayOfWeek dayOfWeek)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        await _salonService.RemoveOpeningHoursAsync(salonId, dayOfWeek);

        return NoContent();
    }
    
    /// <summary>
    /// Updates the address for a specific salon.
    /// </summary>
    /// <param name="salonId">The unique identifier of the salon whose address will be updated.</param>
    /// <param name="request">The <see cref="UpdateAddressRequest"/> object containing the address details.</param>
    /// <remarks>
    /// Sample request:
    /// 
    ///     PUT /Salon/{salonId}/Manage/Address
    ///     {
    ///         "Country": "Poland",
    ///         "Region": "Małopolskie",
    ///         "City": "Warsaw",
    ///         "PostalCode": "00-001",
    ///         "Street": "Example street",
    ///         "BuildingNumber": "1"
    ///     }
    ///  
    /// </remarks>
    /// <returns>A <see cref="IActionResult"/> indicating the result of the update operation.</returns>
    /// <response code="200">Indicates that the address was updated successfully.</response>
    /// <response code="400">Indicates that the request is invalid due to bad request or validation errors.</response>
    /// <response code="401">If the request is not authenticated.</response>
    /// <response code="403">Indicates that the request is forbidden due to insufficient permissions.</response>
    [Consumes("application/json")]
    [Authorize(Roles = "Admin, Manager")]
    [HttpPut("{salonId}/Manage/Address")]
    public async Task<IActionResult> UpdateAddressAsync(Guid salonId, [FromBody] UpdateAddressRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        await _salonService.UpdateAddressAsync(salonId, request);
                
        return Ok();
    }
        
    /// <summary>
    /// Deletes a specific salon by its ID.
    /// </summary>
    /// <param name="salonId">The unique identifier of the salon to delete.</param>
    /// <remarks>
    /// This endpoint allows an admin to delete a specific salon using its unique identifier. 
    /// If the salon is successfully deleted, no content will be returned.
    ///
    /// Sample request:
    /// 
    ///     DELETE /Salon/{salonId}/Manage/Delete
    /// 
    /// </remarks>
    /// <returns>A <see cref="IActionResult"/> indicating the result of the delete operation.
    /// Returns <see cref="NoContentResult"/> if the deletion is successful.</returns>
    /// <response code="204">If the salon was deleted successfully.</response>
    /// <response code="401">If the request is not authenticated.</response>
    /// <response code="403">Indicates that the request is forbidden due to insufficient permissions.</response>
    /// <response code="404">If the salon with the specified ID is not found.</response>
    [Consumes("application/json")]
    [Authorize(Roles = "Admin")]
    [HttpDelete("{salonId}")]
    public async Task<IActionResult> DeleteAsync(Guid salonId)
    {
        await _salonService.DeleteAsync(salonId);

        return NoContent();
    }
}