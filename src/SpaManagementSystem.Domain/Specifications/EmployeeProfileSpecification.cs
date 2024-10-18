using SpaManagementSystem.Domain.Entities;

namespace SpaManagementSystem.Domain.Specifications;

public class EmployeeProfileSpecification : ISpecification<EmployeeProfile>
{
    private readonly ValidationResult _result = new(true);
    
    public ValidationResult IsSatisfiedBy(EmployeeProfile entity)
    {
        SpecificationHelper.ValidateString(entity.FirstName, _result, "FirstName is required.");
        SpecificationHelper.ValidateString(entity.LastName, _result, "LastName is required.");
        SpecificationHelper.ValidateGender(entity.Gender, _result);
        ValidateDateOfBirth(entity.DateOfBirth);
        SpecificationHelper.ValidateEmail(entity.Email, _result);
        SpecificationHelper.ValidatePhoneNumber(entity.PhoneNumber, _result);
        
        return _result;
    }
    
    private void ValidateDateOfBirth(DateOnly dateOfBirth)
    {
        var currentDate = DateOnly.FromDateTime(DateTime.Now);

        if (dateOfBirth > currentDate)
            _result.AddError($"Date of birth {dateOfBirth} cannot be in the future.");
        
        var minimumAllowedDateOfBirth = currentDate.AddYears(-16);
        if (dateOfBirth > minimumAllowedDateOfBirth)
            _result.AddError($"Date of birth {dateOfBirth} indicates the person is too young. Minimum age is {minimumAllowedDateOfBirth}.");
        
        var maximumAllowedDateOfBirth = currentDate.AddYears(-100);
        if (dateOfBirth < maximumAllowedDateOfBirth)
            _result.AddError($"Date of birth {dateOfBirth} indicates the person is too old. Maximum allowed age is {maximumAllowedDateOfBirth}.");
    }
}