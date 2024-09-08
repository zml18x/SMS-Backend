using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Authorization;
using SpaManagementSystem.Application.Dto;
using SpaManagementSystem.Application.Interfaces;
using SpaManagementSystem.Application.Requests.Salon;
using SpaManagementSystem.Application.Requests.Common;

namespace SpaManagementSystem.WebApi.Controllers;

[Route("api/salon")]
[ApiController]
public class SalonController(ISalonService salonService) : BaseController
{
    /// <summary>
    /// Creates a new salon based on the provided details.
    /// </summary>
    /// <remarks>
    /// This endpoint allows an admin user to create a new salon by providing the necessary details in the request body.
    /// The user must have the "Admin" role to access this endpoint.
    /// </remarks>
    /// <param name="request">The request object containing the salon details.</param>
    /// <returns>
    /// Returns an HTTP response indicating the result of the salon creation.
    /// </returns>
    /// <response code="201">Salon was successfully created.</response>
    /// <response code="400">Returned if the request contains invalid data or fails validation.</response>
    /// <response code="401">Returned if the user is not authenticated.</response>
    /// <response code="403">Returned if the user is not authorized to perform this action.</response>
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Authorize(Roles = "Admin")]
    [HttpPost("create")]
    public async Task<IActionResult> CreateAsync([FromBody] CreateSalonRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        await salonService.CreateAsync(UserId, request);

        return Created("api/salon", null);
    }
        
    
    /// <summary>
    /// Retrieves a list of all salons associated with the currently authenticated admin user.
    /// </summary>
    /// <remarks>
    /// This endpoint returns a list of salons that are linked to the admin user who is making the request.
    /// The user must have the "Admin" role to access this endpoint.
    /// </remarks>
    /// <returns>
    /// Returns an HTTP response containing a list of salons.
    /// </returns>
    /// <response code="200">Successfully retrieved the list of salons.</response>
    /// <response code="401">Returned if the user is not authenticated.</response>
    /// <response code="403">Returned if the user is not authorized to perform this action.</response>
    [Produces("application/json")]
    [ProducesResponseType(typeof(IEnumerable<SalonDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [Authorize(Roles = "Admin")]
    [HttpGet("list")]
    public async Task<IActionResult> GetAllSalonsAsync()
    {
        var salons = await salonService.GetAllSalonsByUserIdAsync(UserId);

        return Ok(salons);
    }
        
    /// <summary>
    /// Retrieves the details of a specific salon by its ID.
    /// </summary>
    /// <remarks>
    /// This endpoint returns the detailed information of a salon identified by the provided salon ID.
    /// The user must have one of the following roles: "Admin", "Manager", or "Employee" to access this endpoint.
    /// </remarks>
    /// <param name="salonId">The unique identifier of the salon.</param>
    /// <returns>
    /// Returns an HTTP response containing the salon details.
    /// </returns>
    /// <response code="200">Successfully retrieved the salon details.</response>
    /// <response code="401">Returned if the user is not authenticated.</response>
    /// <response code="403">Returned if the user is not authorized to access the salon details.</response>
    /// <response code="404">Returned if the salon with the specified ID is not found.</response>
    [Produces("application/json")]
    [ProducesResponseType(typeof(SalonDetailsDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Roles="Admin, Manager, Employee")]
    [HttpGet("{salonId}")]
    public async Task<IActionResult> GetSalonAsync(Guid salonId)
    {
        var salon = await salonService.GetSalonDetailsByIdAsync(salonId);

        return Ok(salon);
    }
    
    /// <summary>
    /// Partially updates the details of a specific salon using a JSON Patch document.
    /// </summary>
    /// <remarks>
    /// This endpoint allows an admin or manager to update specific fields of a salon's details by applying a JSON Patch document.
    /// The user must have one of the following roles: "Admin" or "Manager" to access this endpoint.
    /// </remarks>
    /// <param name="salonId">The unique identifier of the salon to be updated.</param>
    /// <param name="patchDocument">The JSON Patch document containing the changes to be applied to the salon details.</param>
    /// <param name="requestValidator">The validator service for validating the updated salon details.</param>
    /// <returns>
    /// Returns an HTTP response indicating the result of the update operation.
    /// </returns>
    /// <response code="204">Successfully updated the salon details.</response>
    /// <response code="400">Returned if the request contains invalid data or if no changes were made.</response>
    /// <response code="401">Returned if the user is not authenticated.</response>
    /// <response code="403">Returned if the user is not authorized to update the salon details.</response>
    /// <response code="404">Returned if the salon with the specified ID is not found.</response>
    [Produces("application/json-patch+json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Roles = "Admin, Manager")]
    [HttpPatch("{salonId}/manage/update-details")]
    public async Task<IActionResult> UpdateDetailsAsync(Guid salonId, [FromBody] JsonPatchDocument<UpdateSalonDetailsRequest> patchDocument,
        [FromServices] IValidator<UpdateSalonDetailsRequest> requestValidator)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
            
        var existingSalon = await salonService.GetSalonDetailsByIdAsync(salonId);

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

        var isUpdated = await salonService.UpdateSalonAsync(salonId, request);
        if (!isUpdated)
            return BadRequest("No changes were made to the salon.");

        return NoContent();
    }
    
    /// <summary>
    /// Adds opening hours to a specific salon.
    /// </summary>
    /// <remarks>
    ///  This endpoint allows an admin or manager to define the opening hours for a salon by providing the details in the request body.
    /// The user must have one of the following roles: "Admin" or "Manager" to access this endpoint.
    /// </remarks>
    /// <param name="salonId">The unique identifier of the salon to which the opening hours are being added.</param>
    /// <param name="request">The request object containing the opening hours details.</param>
    /// <returns>
    /// Returns an HTTP response indicating the result of the operation.
    /// </returns>
    /// <response code="201">Successfully added the opening hours to the salon.</response>
    /// <response code="400">Returned if the request contains invalid data.</response>
    /// <response code="401">Returned if the user is not authenticated.</response>
    /// <response code="403">Returned if the user is not authorized to add opening hours.</response>
    /// <response code="404">Returned if the salon with the specified ID is not found.</response>
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Roles = "Admin, Manager")]
    [HttpPost("{salonId}/manage/opening-hours")]
    public async Task<IActionResult> AddOpeningHoursAsync(Guid salonId, [FromBody] OpeningHoursRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        await salonService.AddOpeningHoursAsync(salonId, request);
            
        return Created();
    }
    
    /// <summary>
    /// Updates the opening hours of a specific salon.
    /// </summary>
    /// <remarks>
    /// This endpoint allows an admin or manager to update the opening hours of a salon by providing the updated details in the request body.
    /// The user must have one of the following roles: "Admin" or "Manager" to access this endpoint.
    /// </remarks>
    /// <param name="salonId">The unique identifier of the salon whose opening hours are being updated.</param>
    /// <param name="request">The request object containing the updated opening hours details.</param>
    /// <returns>
    /// Returns an HTTP response indicating the result of the update operation.
    /// </returns>
    /// <response code="204">Successfully updated the opening hours of the salon.</response>
    /// <response code="400">Returned if the request contains invalid data.</response>
    /// <response code="401">Returned if the user is not authenticated.</response>
    /// <response code="403">Returned if the user is not authorized to update opening hours.</response>
    /// <response code="404">Returned if the salon with the specified ID is not found.</response>
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Roles = "Admin, Manager")]
    [HttpPut("{salonId}/manage/opening-hours")]
    public async Task<IActionResult> UpdateOpeningHoursAsync(Guid salonId, [FromBody] OpeningHoursRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        await salonService.UpdateOpeningHoursAsync(salonId, request);
            
        return NoContent();
    }
    
    /// <summary>
    /// Removes the opening hours for a specific day of the week from a salon.
    /// </summary>
    /// <remarks>
    /// This endpoint allows an admin or manager to remove the opening hours for a specific day of the week from a salon.
    /// The user must have one of the following roles: "Admin" or "Manager" to access this endpoint.
    /// </remarks>
    /// <param name="salonId">The unique identifier of the salon from which the opening hours are being removed.</param>
    /// <param name="dayOfWeek">The day of the week for which the opening hours are to be removed.</param>
    /// <returns>
    /// Returns an HTTP response indicating the result of the operation.
    /// </returns>
    /// <response code="204">Successfully removed the opening hours for the specified day of the week.</response>
    /// <response code="400">Returned if the request contains invalid data.</response>
    /// <response code="401">Returned if the user is not authenticated.</response>
    /// <response code="403">Returned if the user is not authorized to remove opening hours.</response>
    /// <response code="404">Returned if the salon with the specified ID is not found.</response>
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Roles = "Admin, Manager")]
    [HttpDelete("{salonId}/manage/opening-hours/{dayOfWeek}")]
    public async Task<IActionResult> RemoveOpeningHoursAsync(Guid salonId, DayOfWeek dayOfWeek)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        await salonService.RemoveOpeningHoursAsync(salonId, dayOfWeek);

        return NoContent();
    }
    
    /// <summary>
    /// Updates the address of a specific salon.
    /// </summary>
    /// <remarks>
    /// This endpoint allows an admin or manager to update the address details of a salon by providing the new address information in the request body.
    /// The user must have one of the following roles: "Admin" or "Manager" to access this endpoint.
    /// </remarks>
    /// <param name="salonId">The unique identifier of the salon whose address is being updated.</param>
    /// <param name="request">The request object containing the updated address details.</param>
    /// <returns>
    /// Returns an HTTP response indicating the result of the update operation.
    /// </returns>
    /// <response code="204">Successfully updated the salon's address.</response>
    /// <response code="400">Returned if the request contains invalid data.</response>
    /// <response code="401">Returned if the user is not authenticated.</response>
    /// <response code="403">Returned if the user is not authorized to update the address.</response>
    /// <response code="404">Returned if the salon with the specified ID is not found.</response>
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Roles = "Admin, Manager")]
    [HttpPut("{salonId}/manage/address")]
    public async Task<IActionResult> UpdateAddressAsync(Guid salonId, [FromBody] UpdateAddressRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        await salonService.UpdateAddressAsync(salonId, request);
                
        return NoContent();
    }
        
    /// <summary>
    /// Deletes a specific salon by its ID.
    /// </summary>
    /// <remarks>
    /// This endpoint allows an admin to delete a salon from the system. The user must have the "Admin" role to access this endpoint.
    /// </remarks>
    /// <param name="salonId">The unique identifier of the salon to be deleted.</param>
    /// <returns>
    /// Returns an HTTP response indicating the result of the deletion operation.
    /// </returns>
    /// <response code="204">Successfully deleted the salon.</response>
    /// <response code="400">Returned if the request contains invalid data.</response>
    /// <response code="401">Returned if the user is not authenticated.</response>
    /// <response code="403">Returned if the user is not authorized to delete the salon.</response>
    /// <response code="404">Returned if the salon with the specified ID is not found.</response>
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Roles = "Admin")]
    [HttpDelete("{salonId}")]
    public async Task<IActionResult> DeleteAsync(Guid salonId)
    {
        await salonService.DeleteAsync(salonId);

        return NoContent();
    }
}