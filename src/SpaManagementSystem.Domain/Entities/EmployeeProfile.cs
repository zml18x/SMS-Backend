using SpaManagementSystem.Domain.Common.Helpers;
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
    
    
    
    protected EmployeeProfile(){}

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


    public bool UpdateEmployeeProfile(string email, string phoneNumber)
    {
        var propertyChanges = new Dictionary<Action, bool>
        {
            { () => Email = email, Email != email },
            { () => PhoneNumber = phoneNumber, PhoneNumber != phoneNumber },
        };

        var anyDataUpdated = EntityUpdater.ApplyChanges(propertyChanges);

        return anyDataUpdated;
    }
    
    public bool UpdateEmployeeProfile(string firstName, string lastName, GenderType gender, DateOnly dateOfBirth,
        string email, string phoneNumber)
    {
        var propertyChanges = new Dictionary<Action, bool>
        {
            { () => FirstName = firstName, FirstName != firstName },
            { () => LastName = lastName, LastName != lastName },
            { () => Gender = gender, Gender != gender },
            { () => DateOfBirth = dateOfBirth, DateOfBirth != dateOfBirth },
            { () => Email = email, Email != email },
            { () => PhoneNumber = phoneNumber, PhoneNumber != phoneNumber },
        };

        var anyDataUpdated = EntityUpdater.ApplyChanges(propertyChanges);

        return anyDataUpdated;
    }
}