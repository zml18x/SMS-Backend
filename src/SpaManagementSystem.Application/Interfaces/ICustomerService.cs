using Microsoft.AspNetCore.JsonPatch;
using SpaManagementSystem.Application.Common;
using SpaManagementSystem.Application.Dto;
using SpaManagementSystem.Application.Requests.Customer;

namespace SpaManagementSystem.Application.Interfaces;

public interface ICustomerService
{
    public Task<CustomerDto> CreateCustomerAsync(CreateCustomerRequest request);
    public Task<CustomerDto> GetCustomerByIdAsync(Guid customerId);
    public Task<IEnumerable<CustomerDto>> GetCustomersAsync(Guid salonId, string? fistName = null,
        string? lastName = null, string? phoneNumber = null, string? email = null, bool? isActive = null);
    public Task<OperationResult> UpdateCustomerAsync(Guid customerId,
        JsonPatchDocument<UpdateCustomerRequest> patchDocument);
    public Task DeleteAsync(Guid customerId);
}