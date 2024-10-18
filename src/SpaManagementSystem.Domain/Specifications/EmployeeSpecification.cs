using SpaManagementSystem.Domain.Entities;
using SpaManagementSystem.Domain.Enums;

namespace SpaManagementSystem.Domain.Specifications;

public class EmployeeSpecification : ISpecification<Employee>
{
    public ValidationResult IsSatisfiedBy(Employee entity)
    {
        var result = new ValidationResult(true);
        
        ValidateSalonId(entity.SalonId, result);
        ValidateUserId(entity.UserId, result);
        ValidatePosition(entity.Position, result);
        ValidateStatus(entity.EmploymentStatus, result);
        ValidateCode(entity.Code, result);
        ValidateHireDate(entity.HireDate, result);
        ValidateNotes(entity.Notes, result);

        return result;
    }
    
    private void ValidateSalonId(Guid salonId, ValidationResult result)
    {
        if (salonId == Guid.Empty)
            result.AddError("SalonId is required (Cannot be Guid.Empty).");
    }
    
    private void ValidateUserId(Guid userId, ValidationResult result)
    {
        if (userId == Guid.Empty)
            result.AddError("UserId is required (Cannot be Guid.Empty).");
    }
    
    private void ValidatePosition(string position, ValidationResult result)
    {
        if(string.IsNullOrWhiteSpace(position))
            result.AddError("Position is required.");
    }

    private void ValidateStatus(EmploymentStatus employmentStatus, ValidationResult result)
    {
        if (!Enum.IsDefined(typeof(EmploymentStatus), employmentStatus))
            result.AddError($"Invalid employment status: {employmentStatus}");
    }

    private void ValidateCode(string code, ValidationResult result)
    {
        if(string.IsNullOrWhiteSpace(code))
            result.AddError("Code is required.");
    }

    private void ValidateHireDate(DateOnly hireDate, ValidationResult result)
    {
        var currentDate = DateOnly.FromDateTime(DateTime.Now);

        if (hireDate > currentDate)
            result.AddError($"Hire date {hireDate} cannot be in the future.");
        
        var earliestAllowedHireDate = currentDate.AddYears(-50);
        if (hireDate < earliestAllowedHireDate)
            result.AddError($"Hire date {hireDate} is too far in the past.");
    }
    
    private void ValidateNotes(string? notes, ValidationResult result)
    {
        if (!string.IsNullOrWhiteSpace(notes) && notes.Length > 500)
            result.AddError($"Notes cannot exceed 500 characters. Current length: {notes.Length}");
    }
}