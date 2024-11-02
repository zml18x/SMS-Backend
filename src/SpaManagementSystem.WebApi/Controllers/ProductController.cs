using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Authorization;
using SpaManagementSystem.Application.Interfaces;
using SpaManagementSystem.Application.Requests.Product;
using SpaManagementSystem.WebApi.Models;
using SpaManagementSystem.WebApi.Extensions;

namespace SpaManagementSystem.WebApi.Controllers;

[ApiController]
[Route("api/product")]
public class ProductController(IProductService productService) : BaseController
{
    [HttpPost("create")]
    [Authorize(Roles = "Admin, Manager")]
    public async Task<IActionResult> CreateProductAsync([FromBody] CreateProductRequest request)
    {
        var product = await productService.CreateProductAsync(request);
        
        return CreatedAtAction(
            actionName: nameof(GetByIdAsync),
            controllerName: "Product",
            routeValues: new { productId = product.Id },
            value: product
        );
    }
    
    [HttpGet("{productId}")]
    [Authorize(Roles = "Admin, Manager, Employee")]
    public async Task<IActionResult> GetByIdAsync(Guid productId)
    {
        var service = await productService.GetProductByIdAsync(productId);

        return this.OkResponse(service, "Successful retrieved product.");
    }
    
    [HttpGet]
    [Authorize(Roles = "Admin, Manager, Employee")]
    public async Task<IActionResult> GetProductsAsync([FromQuery] Guid salonId, [FromQuery] string? code = null,
        [FromQuery] string? name = null, [FromQuery] bool? active = null)
    {
        var services = await productService.GetProductsAsync(salonId, code, name, active);

        return this.OkResponse(services, "Successful retrieved products.");
    }
    
    [HttpPatch("{productId:guid}")]
    [Authorize(Roles = "Admin, Manager")]
    public async Task<IActionResult> UpdateServiceAsync(Guid productId, 
        [FromBody] JsonPatchDocument<UpdateProductRequest> patchDocument)
    {
        var result = await productService.UpdateProductAsync(productId, patchDocument);

        return result.IsSuccess
            ? NoContent()
            : BadRequest(new ValidationErrorResponse { Errors = result.Errors });
    }

    [HttpDelete("{productId:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteServiceAsync(Guid productId)
    {
        await productService.DeleteProductAsync(productId);

        return NoContent();
    }
}