using SpaManagementSystem.Domain.Entities;
using SpaManagementSystem.Domain.ValueObjects;

namespace SpaManagementSystem.Domain.Specifications;

public class EmployeeAvailabilitySpecification : ISpecification<EmployeeAvailability>
{
    private readonly ValidationResult _result = new(true);
    
    public ValidationResult IsSatisfiedBy(EmployeeAvailability entity)
    {
        SpecificationHelper.ValidateGuid(entity.EmployeeId, _result, "EmployeeId is required.");
        ValidateDate(entity.Date);
        ValidateAvailabilityHours(entity.AvailableHours);

        return _result;
    }

    private void ValidateDate(DateOnly date)
    {
        var currentDate = DateOnly.FromDateTime(DateTime.UtcNow);
        
        if(date < currentDate)
            _result.AddError("Date cannot be in the past.");
        else if (date > currentDate.AddYears(1))
            _result.AddError("Date must be within one year from now.");
    }

    private void ValidateAvailabilityHours(IEnumerable<AvailableHours> availableHours)
    {
        var hoursList = availableHours.ToList();
        
        var invalidHours = hoursList
            .Where(availableHour => availableHour.Start >= availableHour.End)
            .ToList();

        foreach (var invalidHour in invalidHours)
            _result.AddError($"Available hour '{invalidHour.Start} - {invalidHour.End}' is invalid: Start time must be earlier than end time.");

        var overlaps = from hour1 in hoursList
            from hour2 in hoursList
            where hour1 != hour2 && hour1.Overlaps(hour2)
            select new { hour1, hour2 };

        foreach (var overlap in overlaps)
            _result.AddError($"Available hours '{overlap.hour1.Start} - {overlap.hour1.End}' and '{overlap.hour2.Start} - {overlap.hour2.End}' overlap.");
    }
}