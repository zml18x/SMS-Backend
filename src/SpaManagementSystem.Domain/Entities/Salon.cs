using System.Net.Mail;
using System.Text.RegularExpressions;
using SpaManagementSystem.Domain.Common;
using SpaManagementSystem.Domain.ValueObjects;

namespace SpaManagementSystem.Domain.Entities;

public class Salon : BaseEntity
{
    private ISet<OpeningHours> _openingHours = new HashSet<OpeningHours>();
    public Guid UserId { get; protected set; }
    public string Name { get; protected set; } = String.Empty;
    public string Email { get; protected set; } = String.Empty;
    public string PhoneNumber { get; protected set; } = String.Empty;
    public string? Description { get; protected set; }
    public Address? Address { get; protected set; }
    public IEnumerable<OpeningHours> OpeningHours => _openingHours;
        

    
    public Salon(){}
    public Salon(Guid id, Guid userId, string name, string email, string phoneNumber,
        string? description = null) : base(id)
    {
        SetUserId(userId);
        SetName(name);
        SetEmail(email);
        SetPhoneNumber(phoneNumber);
        SetDescription(description);
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
            SetName(name);
            anyDataUpdated = true;
        }
            
        if (!string.IsNullOrWhiteSpace(email))
        {
            SetEmail(email);
            anyDataUpdated = true;
        }
            
        if (!string.IsNullOrWhiteSpace(phoneNumber))
        {
            SetPhoneNumber(phoneNumber);
            anyDataUpdated = true;
        }

        if (Description != description)
        {
            SetDescription(description);
            anyDataUpdated = true;
        }

        return anyDataUpdated;
    }
    
    public void UpdateAddress(Address address)
    {
        Address = address;
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
    }
    
    private void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("The name of the salon cannot be empty or whitespace.", nameof(name));

        if (name.Length > 30)
            throw new ArgumentException("The name of the salon cannot be longer than 30 characters.", nameof(name));

        Name = name;
    }
    
    private void SetDescription(string? description)
    {
        if (!string.IsNullOrWhiteSpace(description))
            if (description.Length > 1000)
                throw new ArgumentException("The description of the salon cannot be longer than 1000 characters",
                    nameof(description));
            
        Description = description;
    }
    
    private void SetPhoneNumber(string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
            throw new ArgumentException("The phone number cannot be empty.", nameof(phoneNumber));
            
        var regex = new Regex("^[0-9]+$");

        if (!regex.IsMatch(phoneNumber))
            throw new ArgumentException("The phone number can only consist of digits.");

        PhoneNumber = phoneNumber;
    }
    
    private void SetEmail(string email)
    {
        try
        {
            var mailAddress = new MailAddress(email);
        }
        catch(Exception e)
        {
            throw new ArgumentException(e.Message, nameof(email));
        }

        Email = email;
    }
    
    private void SetUserId(Guid userId)
    {
        UserId = (userId != Guid.Empty)
            ? userId
            : throw new ArgumentException("The user id cannot be empty", nameof(userId));
    }
}