using SpaManagementSystem.Domain.Enums;

namespace SpaManagementSystem.Domain.Entities;

public class EmployeeProfile
{
    public string FirstName { get; protected set;}
    public string LastName { get; protected set; }
    public GenderType Gender { get; protected set; }
    public DateOnly DateOfBirth { get; protected set; }
    public string Email { get; protected set; }
    public string PhoneNumber { get; protected set; }
    
    
    
    public EmployeeProfile(){}

    public EmployeeProfile(string firstName, string lastName, GenderType gender, DateOnly dateOfBirth, string email,
        string phoneNumber)
    {
        FirstName = firstName;
        LastName = lastName;
        Gender = gender;
        DateOfBirth = dateOfBirth;
        Email = email;
        PhoneNumber = phoneNumber;
    }
    
    
    
    public bool UpdateEmployeeProfile(string firstName, string lastName, GenderType gender, DateOnly dateOfBirth,
        string email, string phoneNumber)
    {
        var anyDataUpdated = false;
            
        if (!string.IsNullOrWhiteSpace(firstName))
        {
            FirstName = firstName;
            anyDataUpdated = true;
        }
        
        if (!string.IsNullOrWhiteSpace(lastName))
        {
            LastName = lastName;
            anyDataUpdated = true;
        }
            
        if (Gender != gender)
        {
            Gender = gender;
            anyDataUpdated = true;
        }
        
        if (DateOfBirth != dateOfBirth)
        {
            DateOfBirth = dateOfBirth;
            anyDataUpdated = true;
        }
            
        if (!string.IsNullOrWhiteSpace(email))
        {
            Email = email;
            anyDataUpdated = true;
        }
        
        if (!string.IsNullOrWhiteSpace(phoneNumber))
        {
            PhoneNumber = phoneNumber;
            anyDataUpdated = true;
        }

        return anyDataUpdated;
    }
}