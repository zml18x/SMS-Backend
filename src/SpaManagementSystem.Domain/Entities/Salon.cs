using SpaManagementSystem.Domain.Common;
using SpaManagementSystem.Domain.ValueObjects;

namespace SpaManagementSystem.Domain.Entities;

public class Salon : BaseEntity
{
    private ISet<OpeningHours> _openingHours = new HashSet<OpeningHours>();
    private ISet<Employee> _employees = new HashSet<Employee>();
    public Guid UserId { get; protected set; }
    public string Name { get; protected set; } = String.Empty;
    public string Email { get; protected set; } = String.Empty;
    public string PhoneNumber { get; protected set; } = String.Empty;
    public string? Description { get; protected set; }
    public Address? Address { get; protected set; }
    public IEnumerable<OpeningHours> OpeningHours => _openingHours;
    public IEnumerable<Employee> Employees => _employees;
        

    
    public Salon(){}
    public Salon(Guid id, Guid userId, string name, string email, string phoneNumber, string? description)
    {
        Id = id;
        UserId = userId;
        Name = name;
        Email = email;
        PhoneNumber = phoneNumber;
        Description = description;
    }



    /// <summary>
    /// Updates the salon's details.
    /// </summary>
    /// <param name="name">The new name of the salon.</param>
    /// <param name="email">The new email address of the salon.</param>
    /// <param name="phoneNumber">The new phone number of the salon.</param>
    /// <param name="description">The new description of the salon.</param>
    /// <returns>True if any data was updated; otherwise, false.
    /// This can be used to determine if the entity needs to be saved to the database.</returns>
    public bool UpdateSalon(string name, string email, string phoneNumber, string? description)
    {
        var anyDataUpdated = false;
            
        if (!string.IsNullOrWhiteSpace(name))
        {
            Name = name;
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

        if (Description != description)
        {
            Description = description;
            anyDataUpdated = true;
        }
        
        if (anyDataUpdated)
            UpdateTimestamp();

        return anyDataUpdated;
    }
    
    public void SetAddress(Address address)
    {
        Address = address;
        UpdateTimestamp();
    }

    public void AddEmployee(Employee employee)
    {
        _employees.Add(employee);
        UpdateTimestamp();
    }
    
    /// <summary>
    /// Adds a new opening hours entry for a specific day of the week. 
    /// </summary>
    /// <param name="openingHours">The opening hours entry to be added.</param>
    /// <exception cref="InvalidOperationException">
    /// Thrown if opening hours for the specified day already exist in the collection.
    /// </exception>
    public void AddOpeningHours(OpeningHours openingHours)
    {
        if (_openingHours.Any(x => x.DayOfWeek == openingHours.DayOfWeek))
            throw new InvalidOperationException($"Opening hours for {openingHours.DayOfWeek} already exist.");

        _openingHours.Add(openingHours);
        UpdateTimestamp();
    }
    
    /// <summary>
    /// Updates the existing opening hours entry for a specific day of the week. 
    /// </summary>
    /// <param name="openingHours">The updated opening hours' entry.</param>
    /// <exception cref="InvalidOperationException">
    /// Thrown if no opening hours entry is found for the specified day of the week.
    /// </exception>
    public void UpdateOpeningHours(OpeningHours openingHours)
    {
        var existingOpeningHours = _openingHours.FirstOrDefault(oh => oh.DayOfWeek == openingHours.DayOfWeek);
        if (existingOpeningHours == null)
            throw new InvalidOperationException($"No opening hours found for {openingHours.DayOfWeek}.");

        _openingHours.Remove(existingOpeningHours);
        _openingHours.Add(openingHours);
        UpdateTimestamp();
    }
    
    /// <summary>
    /// Removes the opening hours entry for a specific day of the week. 
    /// </summary>
    /// <param name="dayOfWeek">The day of the week for which the opening hours entry should be removed.</param>
    /// <exception cref="InvalidOperationException">
    /// Thrown if no opening hours entry is found for the specified day of the week.
    /// </exception>
    public void RemoveOpeningHours(DayOfWeek dayOfWeek)
    {
        var openingHours = _openingHours.FirstOrDefault(oh => oh.DayOfWeek == dayOfWeek);

        if (openingHours == null)
            throw new InvalidOperationException($"No opening hours found for {dayOfWeek}.");

        _openingHours.Remove(openingHours);
        UpdateTimestamp();
    }
}