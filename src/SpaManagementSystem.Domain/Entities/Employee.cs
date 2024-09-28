using SpaManagementSystem.Domain.Common;
using SpaManagementSystem.Domain.Enums;

namespace SpaManagementSystem.Domain.Entities;

public class Employee : BaseEntity
{
    public Guid SalonId { get; protected set; }
    public Guid UserId { get; protected set; }
    public string Position { get; protected set; }
    public EmploymentStatus EmploymentStatus { get; protected set; }
    public string Code { get; protected set; }
    public string Color { get; protected set; }
    public DateOnly HireDate { get; protected set; }
    public string? Notes { get; protected set; }
    public Salon Salon { get; protected set; }
    public EmployeeProfile Profile { get; protected set; }
    
    
    
    public Employee(){}

    public Employee(Guid id, Guid salonId, Guid userId, string position, EmploymentStatus employmentStatus, string code,
        DateOnly hireDate, string color, string? notes)
    {
        Id = id;
        SalonId = salonId;
        UserId = userId;
        Position = position;
        EmploymentStatus = employmentStatus;
        Code = code;
        HireDate = hireDate;
        Color = color;
        Notes = notes;
    }


    
    public void AddEmployeeProfile(EmployeeProfile employeeProfile)
    {
        Profile = employeeProfile;
    }

    public bool UpdateEmployee(string position, EmploymentStatus employmentStatus, string code, string color,
        DateOnly hireDate, string? notes)
    {
        var anyDataUpdated = false;
            
        if (!string.IsNullOrWhiteSpace(position))
        {
            Position = position;
            anyDataUpdated = true;
        }
            
        if (EmploymentStatus != employmentStatus)
        {
            EmploymentStatus = employmentStatus;
            anyDataUpdated = true;
        }
            
        if (!string.IsNullOrWhiteSpace(code))
        {
            Code = code;
            anyDataUpdated = true;
        }
        
        if (!string.IsNullOrWhiteSpace(color))
        {
            Color = color;
            anyDataUpdated = true;
        }

        if (HireDate != hireDate)
        {
            HireDate = hireDate;
            anyDataUpdated = true;
        }

        if (Notes != notes)
        {
            Notes = notes;
            anyDataUpdated = true;
        }
        
        if (anyDataUpdated)
            UpdateTimestamp();

        return anyDataUpdated;
    }
}