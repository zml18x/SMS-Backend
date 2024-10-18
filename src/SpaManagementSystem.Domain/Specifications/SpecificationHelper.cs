using System.Net.Mail;
using System.Text.RegularExpressions;
using SpaManagementSystem.Domain.Enums;

namespace SpaManagementSystem.Domain.Specifications;

public static class SpecificationHelper
{
    private static readonly Regex PhoneNumberRegex = new("^[0-9]+$", RegexOptions.Compiled);
    
    public static void ValidateGuid(Guid value, ValidationResult result, string errorMessage)
    {
        if (value == Guid.Empty)
            result.AddError(errorMessage);
    }
    
    public static void ValidateString(string value, ValidationResult result, string errorMessage)
    {
        if (string.IsNullOrWhiteSpace(value))
            result.AddError(errorMessage);
    }
    
    public static void ValidateOptionalStringLength(string? value, int maxLength, ValidationResult result, string errorMessage)
    {
        if (!string.IsNullOrWhiteSpace(value) && value.Length > maxLength)
            result.AddError(errorMessage);
    }
    
    public static void ValidatePrice(decimal value, ValidationResult result, string errorMessage, bool allowZero = true)
    {
        if (value < 0 || (!allowZero && value == 0))
            result.AddError(errorMessage);
    }
    
    public static void ValidateTaxRate(decimal value, ValidationResult result, string errorMessage)
    {
        if (value < 0 || value > 1)
            result.AddError(errorMessage);
    }
    
    public static void ValidateOptionalUrl(string? url, ValidationResult result, string errorMessage)
    {
        if (!string.IsNullOrWhiteSpace(url) && !Uri.IsWellFormedUriString(url, UriKind.Absolute))
            result.AddError(errorMessage);
    }
    
    public static void ValidateQuantity(int value, ValidationResult result, string errorMessage)
    {
        if (value < 0)
            result.AddError(errorMessage);
    }
    
    public static void ValidateQuantity(decimal value, ValidationResult result, string errorMessage)
    {
        if (value < 0)
            result.AddError(errorMessage);
    }
    
    public static void ValidatePhoneNumber(string phoneNumber, ValidationResult result)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
            result.AddError("Phone number is required.");
        else if (!PhoneNumberRegex.IsMatch(phoneNumber))
            result.AddError("Phone number can only consist of digits.");
    }
    
    public static void ValidateEmail(string email, ValidationResult result)
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
    
    public static void ValidateGender(GenderType gender, ValidationResult result)
    {
        if (!Enum.IsDefined(typeof(GenderType), gender))
            result.AddError($"Invalid gender : {gender}");
    }
}