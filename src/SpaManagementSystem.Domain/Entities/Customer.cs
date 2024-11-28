using SpaManagementSystem.Domain.Enums;
using SpaManagementSystem.Domain.Common;
using SpaManagementSystem.Domain.Common.Helpers;

namespace SpaManagementSystem.Domain.Entities;

public class Customer : BaseEntity
{
    private ISet<Appointment> _appointments = new HashSet<Appointment>();
    
    public Guid SalonId { get; protected set; }
    public Salon Salon { get; protected set; }  = null!;
    
    public string FirstName { get; protected set; }
    public string LastName { get; protected set; }
    public GenderType Gender {get; protected set; }
    public string PhoneNumber { get; protected set; }
    public string? Email { get; protected set; }
    public string? Notes { get; protected set; }
    public bool IsActive { get; protected set; }
    public IEnumerable<Appointment> Appointments => _appointments;


    
    protected Customer(){}
    public Customer(Guid id, Guid salonId, string firstName, string lastName, GenderType gender, string phoneNumber,
        string? email = null, string? notes = null)
    {
        Id = id;
        SalonId = salonId;
        FirstName = firstName;
        LastName = lastName;
        Gender = gender;
        PhoneNumber = phoneNumber;
        Email = email;
        Notes = notes;
        IsActive = true;
    }
    
    
    
    public bool UpdateCustomer(string firstName, string lastName, GenderType gender, string phoneNumber, string? email,
        string? notes, bool isActive)
    {
        var propertyChanges = new Dictionary<Action, bool>
        {
            { () => FirstName = firstName, FirstName != firstName },
            { () => LastName = lastName, LastName != lastName },
            { () => Gender = gender, Gender != gender },
            { () => PhoneNumber = phoneNumber, PhoneNumber != phoneNumber },
            { () => Email = email, Email != email },
            { () => Notes = notes, Notes != notes },
            { () => IsActive = isActive, IsActive != isActive }
        };

        var anyDataUpdated = EntityUpdater.ApplyChanges(propertyChanges);

        if (anyDataUpdated)
            UpdateTimestamp();

        return anyDataUpdated;
    }
}