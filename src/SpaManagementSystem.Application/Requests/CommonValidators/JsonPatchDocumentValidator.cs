using FluentValidation;
using Microsoft.AspNetCore.JsonPatch;

namespace SpaManagementSystem.Application.Requests.CommonValidators
{
    /// <summary>
    /// Validator for JSON Patch documents to ensure that each operation is valid.
    /// This validator only allows "replace" operations and requires that the path and operation type are not empty.
    /// </summary>
    /// <typeparam name="T">The type of the class that the JSON Patch document applies to.</typeparam>
    public class JsonPatchDocumentValidator<T> : AbstractValidator<JsonPatchDocument<T>> where T : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JsonPatchDocumentValidator{T}"/> class.
        /// Sets up validation rules for each operation in the JSON Patch document.
        /// </summary>
        public JsonPatchDocumentValidator()
        {
            // Validates each operation in the JSON Patch document
            RuleForEach(doc => doc.Operations).ChildRules(ops =>
            {
                ops.RuleFor(op => op.path).NotEmpty().WithMessage("Path is required.");
                ops.RuleFor(op => op.op)
                    .NotEmpty().WithMessage("Operation type is required.")
                    .Must(op => op == "replace").WithMessage("Only replace operations are allowed.");
            });
        }
    }
}