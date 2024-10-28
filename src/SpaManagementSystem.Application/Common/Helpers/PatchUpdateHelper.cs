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
        var salonIdProperty = typeof(TEntity).GetProperty("SalonId");
        var codeProperty = typeof(TEntity).GetProperty("Code");
        
        if (salonIdProperty != null && codeProperty != null)
        {
            var salonId = (Guid)salonIdProperty.GetValue(entity)!;
            var code = (string)codeProperty.GetValue(entity)!;

            await ValidateCodeUniquenessAsync(patchDocument, (IUniqueCodeRepository)repository, typeof(TEntity).Name,
                salonId, code);
        }
        
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
    
    private async Task ValidateCodeUniquenessAsync<TRequest>(
        JsonPatchDocument<TRequest> patchDocument,
        IUniqueCodeRepository repository,
        string entityName,
        Guid salonId,
        string code) where TRequest : class
    {
        var codeOperation = patchDocument.Operations
            .FirstOrDefault(op => op.path.TrimStart('/').Equals("code", StringComparison.OrdinalIgnoreCase));

        if (codeOperation?.value is string value && !string.IsNullOrWhiteSpace(value) && code != value)
            if (await repository.IsExistsAsync(salonId, value))
                throw new InvalidOperationException($"{entityName} with code '{value}' already exists.");
    }
}