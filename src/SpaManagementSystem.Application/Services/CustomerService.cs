using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using SpaManagementSystem.Domain.Builders;
using SpaManagementSystem.Domain.Interfaces;
using SpaManagementSystem.Domain.Specifications;
using SpaManagementSystem.Application.Common;
using SpaManagementSystem.Application.Common.Helpers;
using SpaManagementSystem.Application.Dto;
using SpaManagementSystem.Application.Extensions;
using SpaManagementSystem.Application.Interfaces;
using SpaManagementSystem.Application.Requests.Customer;
using SpaManagementSystem.Application.Requests.Customer.Validators;

namespace SpaManagementSystem.Application.Services;

public class CustomerService(ISalonRepository salonRepository, ICustomerRepository customerRepository,
    CustomerBuilder customerBuilder, IMapper mapper) : ICustomerService
{
    public async Task<CustomerDto> CreateCustomerAsync(CreateCustomerRequest request)
    {
        var customer = await customerRepository.GetByPhoneNumberAsync(request.SalonId, request.PhoneNumber);
        if (customer != null)
            throw new InvalidOperationException($"Customer with phone number {request.PhoneNumber} already exists.");

        var salon = await salonRepository.GetOrThrowAsync(() => salonRepository.GetByIdAsync(request.SalonId));
        
        customer = customerBuilder
            .WithSalonId(request.SalonId)
            .WithFirstName(request.FirstName)
            .WithLastName(request.LastName)
            .WithGender(request.Gender)
            .WithPhoneNumber(request.PhoneNumber)
            .WithEmail(request.Email)
            .WithNotes(request.Notes)
            .Build();
        
        
        salon.AddCustomer(customer);
        await salonRepository.SaveChangesAsync();
        
        return mapper.Map<CustomerDto>(customer);
    }

    public async Task<CustomerDto> GetCustomerByIdAsync(Guid customerId)
    {
        var customer = await customerRepository.GetOrThrowAsync(() => customerRepository.GetByIdAsync(customerId));

        return mapper.Map<CustomerDto>(customer);
    }

    public async Task<IEnumerable<CustomerDto>> GetCustomersAsync(Guid salonId, string? fistName = null,
        string? lastName = null, string? phoneNumber = null, string? email = null, bool? isActive = null)
    { 
        await salonRepository.GetOrThrowAsync(() => salonRepository.GetByIdAsync(salonId));
        
        var customers =
            await customerRepository.GetCustomersAsync(salonId, fistName, lastName, phoneNumber, email, isActive);
        
        return mapper.Map<IEnumerable<CustomerDto>>(customers);
    }

    public async Task<OperationResult> UpdateCustomerAsync(Guid customerId,
        JsonPatchDocument<UpdateCustomerRequest> patchDocument)
    {
        var existingCustomer = await customerRepository.GetOrThrowAsync(() => customerRepository.GetByIdAsync(customerId));
        
        var request = mapper.Map<UpdateCustomerRequest>(existingCustomer);
        
        await ValidateUniqueFieldAsync(
            existingCustomer.SalonId,
            "email",
            patchDocument,
            customerRepository.GetByEmailAsync,
            "Customer with this email already exists.");
        
        await ValidateUniqueFieldAsync(
            existingCustomer.SalonId,
            "phoneNumber",
            patchDocument,
            customerRepository.GetByPhoneNumberAsync,
            "Customer with this phone number already exists.");
        
        return await new PatchUpdateHelper().ApplyPatchAndUpdateAsync(
            patchDocument,
            existingCustomer,
            request,
            new UpdateCustomerRequestValidator(),
            (s, r) => s.UpdateCustomer(r.FirstName, r.LastName, r.Gender, r.PhoneNumber, r.Email, r.Notes, r.IsActive),
            s => new CustomerSpecification().IsSatisfiedBy(s),
            (s, r) => s.HasChanges(r),
            customerRepository
        );
    }
    
    public async Task DeleteAsync(Guid customerId)
    {
        var customer = await customerRepository.GetOrThrowAsync(() => customerRepository.GetByIdAsync(customerId));
        
        customerRepository.Delete(customer);
        await customerRepository.SaveChangesAsync();
    }
    
    private async Task ValidateUniqueFieldAsync<TRequest, TEntity>(
        Guid salonId,
        string fieldName,
        JsonPatchDocument<TRequest> patchDocument,
        Func<Guid, string, Task<TEntity?>> getByFieldFunc,
        string errorMessage) where TRequest : class where TEntity : class
    {
        var operation = patchDocument.Operations
            .FirstOrDefault(op => op.path.TrimStart('/').Equals(fieldName, StringComparison.OrdinalIgnoreCase));

        if (operation?.value is string value && !string.IsNullOrWhiteSpace(value))
        {
            value = value.ToLower().Trim();
            var existingEntity = await getByFieldFunc(salonId, value);
            if (existingEntity != null)
                throw new InvalidOperationException(errorMessage);
        }
    }
}