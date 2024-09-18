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
}