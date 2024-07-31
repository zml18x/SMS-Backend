using System.Net.Mail;
using System.Text.RegularExpressions;
using SpaManagementSystem.Domain.Common;
using SpaManagementSystem.Domain.ValueObjects;

namespace SpaManagementSystem.Domain.Entities;

/// <summary>
/// Represents a salon within the Spa Management System.
/// Inherits from BaseEntity and includes additional fields specific to a salon such as name,
/// email, phone number, description, address ID, and opening hours.
/// </summary>
public class Salon : BaseEntity
{
    private ISet<OpeningHours> _openingHours = new HashSet<OpeningHours>();
        
    /// <summary>
    /// Gets the unique identifier for the associated user.
    /// </summary>
    public Guid UserId { get; protected set; }

    /// <summary>
    /// Gets the name of the salon.
    /// </summary>
    public string Name { get; protected set; } = String.Empty;

    /// <summary>
    /// Gets the email address of the salon.
    /// </summary>
    public string Email { get; protected set; } = String.Empty;

    /// <summary>
    /// Gets the phone number of the salon.
    /// </summary>
    public string PhoneNumber { get; protected set; } = String.Empty;

    /// <summary>
    /// Gets the description of the salon.
    /// </summary>
    public string? Description { get; protected set; }
        
    /// <summary>
    /// Gets the address of the salon. Can be null.
    /// </summary>
    public Address? Address { get; protected set; }
        
    /// <summary>
    /// Gets the collection of opening hours for the salon.
    /// </summary>
    public IEnumerable<OpeningHours> OpeningHours => _openingHours;
        

        
    /// <summary>
    /// Initializes a new instance of the Salon class.
    /// </summary>
    public Salon(){}

    /// <summary>
    /// Initializes a new instance of the Salon class with specific details.
    /// </summary>
    /// <param name="id">The unique identifier for the salon.</param>
    /// <param name="userId">The user's unique identifier.</param>
    /// <param name="name">The name of the salon.</param>
    /// <param name="email">The email address of the salon.</param>
    /// <param name="phoneNumber">The phone number of the salon.</param>
    /// <param name="description">The description of the salon.</param>
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

    /// <summary>
    /// Updates the salon's address
    /// </summary>
    /// <param name="address">The new address</param>
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
    /// <param name="openingHours">The updated opening hours entry.</param>
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
        
    /// <summary>
    /// Validates and sets the name of the salon.
    /// </summary>
    /// <param name="name">The name to set.</param>
    /// <exception cref="ArgumentException">Thrown when the name is null, whitespace, or exceeds 30 characters.</exception>
    private void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("The name of the salon cannot be empty or whitespace.", nameof(name));

        if (name.Length > 30)
            throw new ArgumentException("The name of the salon cannot be longer than 30 characters.", nameof(name));

        Name = name;
    }
        
    /// <summary>
    /// Validates and sets the description of the salon.
    /// </summary>
    /// <param name="description">The description to set.</param>
    /// <exception cref="ArgumentException">Thrown when the description exceeds 1000 characters.</exception>
    private void SetDescription(string? description)
    {
        if (!string.IsNullOrWhiteSpace(description))
            if (description.Length > 1000)
                throw new ArgumentException("The description of the salon cannot be longer than 1000 characters",
                    nameof(description));
            
        Description = description;
    }
        
    /// <summary>
    /// Validates and sets the phone number of the salon.
    /// </summary>
    /// <param name="phoneNumber">The phone number to set.</param>
    /// <exception cref="ArgumentException">Thrown when the phone number is null, whitespace, or contains non-digit characters.</exception>
    private void SetPhoneNumber(string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
            throw new ArgumentException("The phone number cannot be empty.", nameof(phoneNumber));
            
        var regex = new Regex("^[0-9]+$");

        if (!regex.IsMatch(phoneNumber))
            throw new ArgumentException("The phone number can only consist of digits.");

        PhoneNumber = phoneNumber;
    }
        
    /// <summary>
    /// Validates and sets the email address of the salon.
    /// </summary>
    /// <param name="email">The email address to set.</param>
    /// <exception cref="ArgumentException">Thrown when the email is not a valid email address.</exception>
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
        
    /// <summary>
    /// Validates and sets the UserId.
    /// </summary>
    /// <param name="userId">The unique identifier to set.</param>
    /// <exception cref="ArgumentException">Thrown when the UserId is empty.</exception>
    private void SetUserId(Guid userId)
    {
        UserId = (userId != Guid.Empty)
            ? userId
            : throw new ArgumentException("The user id cannot be empty", nameof(userId));
    }
}