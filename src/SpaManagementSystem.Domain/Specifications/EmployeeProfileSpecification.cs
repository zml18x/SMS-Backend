using System.Net.Mail;
using System.Text.RegularExpressions;
using SpaManagementSystem.Domain.Entities;
using SpaManagementSystem.Domain.Enums;

namespace SpaManagementSystem.Domain.Specifications;

public class EmployeeProfileSpecification : ISpecification<EmployeeProfile>
{
    private static readonly Regex PhoneNumberRegex = new("^[0-9]+$", RegexOptions.Compiled);
    
    public ValidationResult IsSatisfiedBy(EmployeeProfile entity)
    {
        var result = new ValidationResult(true);

        ValidateFirstName(entity.FirstName, result);
        ValidateLastName(entity.LastName, result);
        ValidateGender(entity.Gender, result);
        ValidateDateOfBirth(entity.DateOfBirth, result);
        ValidateEmail(entity.Email, result);
        ValidatePhoneNumber(entity.PhoneNumber, result);

        return result;
    }
    
    private void ValidateFirstName(string firstName, ValidationResult result)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            result.AddError("First name is required");
    }
    
    private void ValidateLastName(string lastName, ValidationResult result)
    {
        if (string.IsNullOrWhiteSpace(lastName))
            result.AddError("Last name is required");
    }

    private void ValidateGender(GenderType gender, ValidationResult result)
    {
        if (!Enum.IsDefined(typeof(GenderType), gender))
            result.AddError($"Invalid gender : {gender}");
    }

    private void ValidateDateOfBirth(DateOnly dateOfBirth, ValidationResult result)
    {
        var currentDate = DateOnly.FromDateTime(DateTime.Now);

        if (dateOfBirth > currentDate)
            result.AddError($"Date of birth {dateOfBirth} cannot be in the future.");
        
        var minimumAllowedDateOfBirth = currentDate.AddYears(-16);
        if (dateOfBirth > minimumAllowedDateOfBirth)
            result.AddError($"Date of birth {dateOfBirth} indicates the person is too young. Minimum age is {minimumAllowedDateOfBirth}.");
        
        var maximumAllowedDateOfBirth = currentDate.AddYears(-100);
        if (dateOfBirth < maximumAllowedDateOfBirth)
            result.AddError($"Date of birth {dateOfBirth} indicates the person is too old. Maximum allowed age is {maximumAllowedDateOfBirth}.");
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
    
    private void ValidatePhoneNumber(string phoneNumber, ValidationResult result)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
            result.AddError("Phone number is required.");
        else if (!PhoneNumberRegex.IsMatch(phoneNumber))
            result.AddError("Phone number can only consist of digits.");
    }
}