using SpaManagementSystem.Domain.Enums;
using SpaManagementSystem.Domain.Entities;

namespace SpaManagementSystem.Domain.Specifications;

public class EmployeeSpecification : ISpecification<Employee>
{
    private readonly ValidationResult _result = new(true);

    public ValidationResult IsSatisfiedBy(Employee entity)
    {
        SpecificationHelper.ValidateGuid(entity.SalonId, _result, "SalonId is required (Cannot be Guid.Empty).");
        SpecificationHelper.ValidateGuid(entity.UserId, _result, "UserId is required (Cannot be Guid.Empty).");
        SpecificationHelper.ValidateString(entity.Position, _result, "Position is required.");
        ValidateStatus(entity.EmploymentStatus);
        SpecificationHelper.ValidateString(entity.Code, _result, "Code is required.");
        ValidateHireDate(entity.HireDate);
        SpecificationHelper.ValidateOptionalStringLength(entity.Notes, 500,
            _result, "Notes cannot be longer than 500 characters.");

        return _result;
    }

    private void ValidateStatus(EmploymentStatus employmentStatus)
    {
        if (!Enum.IsDefined(typeof(EmploymentStatus), employmentStatus))
            _result.AddError($"Invalid employment status: {employmentStatus}");
    }

    private void ValidateHireDate(DateOnly hireDate)
    {
        var currentDate = DateOnly.FromDateTime(DateTime.Now);

        if (hireDate > currentDate)
            _result.AddError($"Hire date {hireDate} cannot be in the future.");
        
        var earliestAllowedHireDate = currentDate.AddYears(-50);
        if (hireDate < earliestAllowedHireDate)
            _result.AddError($"Hire date {hireDate} is too far in the past.");
    }
}