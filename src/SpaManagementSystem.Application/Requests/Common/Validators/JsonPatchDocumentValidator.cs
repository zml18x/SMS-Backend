using FluentValidation;
using Microsoft.AspNetCore.JsonPatch;

namespace SpaManagementSystem.Application.Requests.Common.Validators;

public class JsonPatchDocumentValidator<T> : AbstractValidator<JsonPatchDocument<T>> where T : class
{
    public JsonPatchDocumentValidator()
    {
        RuleForEach(doc => doc.Operations).ChildRules(ops =>
        {
            ops.RuleFor(op => op.path).NotEmpty().WithMessage("Path is required.");
            ops.RuleFor(op => op.op)
                .NotEmpty().WithMessage("Operation type is required.")
                .Must(op => op == "replace").WithMessage("Only replace operations are allowed.");
        });
    }
}