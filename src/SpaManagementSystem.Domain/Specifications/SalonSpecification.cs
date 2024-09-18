using System.Net.Mail;
using System.Text.RegularExpressions;
using SpaManagementSystem.Domain.Entities;

namespace SpaManagementSystem.Domain.Specifications;

public class SalonSpecification : ISpecification<Salon>
{
    private static readonly Regex PhoneNumberRegex = new("^[0-9]+$", RegexOptions.Compiled);
    
    public ValidationResult IsSatisfiedBy(Salon entity)
    {
        var result = new ValidationResult(true);

        ValidateUserId(entity.UserId, result);
        ValidateName(entity.Name, result);
        ValidatePhoneNumber(entity.PhoneNumber, result);
        ValidateEmail(entity.Email, result);
        ValidateDescription(entity.Description, result);
        
        return result;
    }
    
    private void ValidateUserId(Guid userId, ValidationResult result)
    {
        if (userId == Guid.Empty)
            result.AddError("UserId is required (Cannot be Guid.Empty).");
    }

    private void ValidateName(string name, ValidationResult result)
    {
        if (string.IsNullOrWhiteSpace(name))
            result.AddError("Salon name is required.");
    }

    private void ValidatePhoneNumber(string phoneNumber, ValidationResult result)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
            result.AddError("Salon phone number is required.");
        else if (!PhoneNumberRegex.IsMatch(phoneNumber))
            result.AddError("Salon phone number can only consist of digits.");
    }

    private void ValidateEmail(string email, ValidationResult result)
    {
        try
        {
            var mailAddress = new MailAddress(email);
        }
        catch
        {
            result.AddError("Invalid email address format.");
        }
    }

    private void ValidateDescription(string? description, ValidationResult result)
    {
        if (!string.IsNullOrWhiteSpace(description) && description.Length > 1000)
            result.AddError("Salon description cannot be longer than 1000 characters.");
    }
}