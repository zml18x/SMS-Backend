using FluentValidation;
using Microsoft.AspNetCore.JsonPatch;
using SpaManagementSystem.Domain.Common;
using SpaManagementSystem.Domain.Exceptions;
using SpaManagementSystem.Domain.Interfaces;
using SpaManagementSystem.Domain.Specifications;

namespace SpaManagementSystem.Application.Common.Helpers;

public class PatchUpdateHelper
{
    public async Task<OperationResult> ApplyPatchAndUpdateAsync<TRequest, TEntity>(
        JsonPatchDocument<TRequest> patchDocument, 
        TEntity entity, 
        TRequest request, 
        IValidator<TRequest> validator, 
        Func<TEntity, TRequest, bool> updateAction, 
        Func<TEntity, ValidationResult> validateEntity, 
        Func<TEntity, TRequest, bool> hasChangesFunc,
        IRepository<TEntity> repository) where TRequest : class where TEntity : BaseEntity
    {
        patchDocument.ApplyTo(request);
        
        var requestValidationResult = await validator.ValidateAsync(request);
        if (!requestValidationResult.IsValid)
        {
            var errors = requestValidationResult.Errors
                .GroupBy(error => error.PropertyName)
                .ToDictionary(
                    group => group.Key, 
                    group => group.Select(error => error.ErrorMessage).ToArray()
                );

            return OperationResult.ValidationFailed(errors);
        }
        
        if (!hasChangesFunc(entity, request))
            return OperationResult.NoChanges();
        
        var isUpdated = updateAction(entity, request);
        if (!isUpdated)
            return OperationResult.NoChanges();
        
        var entityValidationResult = validateEntity(entity);
        if (!entityValidationResult.IsValid)
            throw new DomainValidationException($"Update failed: {string.Join(", ", entityValidationResult.Errors)}");
        
        await repository.SaveChangesAsync();

        return OperationResult.Success();
    }
}