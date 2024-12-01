using SpaManagementSystem.Domain.Enums;
using SpaManagementSystem.Domain.Entities;

namespace SpaManagementSystem.Domain.Specifications;

public class AppointmentSpecification : ISpecification<Appointment>
{
    private readonly ValidationResult _result = new(true);
    
    public ValidationResult IsSatisfiedBy(Appointment entity)
    {
        SpecificationHelper.ValidateGuid(entity.SalonId, _result, "SalonId is required (Cannot be Guid.Empty");
        SpecificationHelper.ValidateGuid(entity.EmployeeId, _result, "EmployeeId is required (Cannot be Guid.Empty");
        SpecificationHelper.ValidateGuid(entity.CustomerId, _result, "CustomerId is required (Cannot be Guid.Empty");
        ValidateDate(entity.Date);
        ValidateTimes(entity.StartTime, entity.EndTime);
        ValidateStatus(entity.Status);  
        SpecificationHelper.ValidateOptionalStringLength(entity.Notes, 1000,
            _result, "Notes cannot be longer than 1000 characters.");
        
        return _result;
    }
    
    private void ValidateDate(DateOnly date)
    {
        var currentDate = DateOnly.FromDateTime(DateTime.UtcNow);

        if (date < currentDate)
            _result.AddError($"The date cannot be in the past.");
    }

    private void ValidateTimes(DateTime startTime, DateTime endTime)
    {
        if (startTime >= endTime)
            _result.AddError("Start time must be earlier than end time.");
        
        var now = DateTime.UtcNow;
        if (startTime < now)
            _result.AddError("Start time cannot be in the past.");
    }

    private void ValidateStatus(AppointmentStatus status)
    {
        if (!Enum.IsDefined(typeof(AppointmentStatus), status))
            _result.AddError($"Invalid appointment status : {status}");
    }
}