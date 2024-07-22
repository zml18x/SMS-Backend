using System.Net.Mail;
using System.Text.RegularExpressions;
using SpaManagementSystem.Domain.Common;

namespace SpaManagementSystem.Domain.Entities
{
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
        /// Gets the unique identifier for the address of the salon.
        /// </summary>
        public Guid? AddressId { get; protected set; }

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
        /// <param name="addressId">The unique identifier for the address of the salon.</param>
        public Salon(Guid id, Guid userId, string name, string email, string phoneNumber, string? description = null,
            Guid? addressId = null) : base(id)
        {
            SetUserId(userId);
            SetName(name);
            SetEmail(email);
            SetPhoneNumber(phoneNumber);
            SetDescription(description);
            SetAddressId(addressId);
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
        /// Adds or updates the opening hours for a specific day of the week.
        /// </summary>
        /// <param name="dayOfWeek">The day of the week to set the opening hours for.</param>
        /// <param name="openingTime">The opening time for the specified day.</param>
        /// <param name="closingTime">The closing time for the specified day.</param>
        /// <param name="isClosed">Indicates if the salon is closed on the specified day.</param>
        public void AddOrUpdateOpeningHours(DayOfWeek dayOfWeek, TimeSpan openingTime, TimeSpan closingTime, bool isClosed = false)
        {
            var existingOpeningHour = _openingHours.FirstOrDefault(x => x.DayOfWeek == dayOfWeek);

            if (existingOpeningHour == null)
                _openingHours.Add(new OpeningHours(Guid.NewGuid(), Id, dayOfWeek, openingTime, closingTime, isClosed));
            else
                existingOpeningHour.UpdateHours(openingTime, closingTime, isClosed);
        }
        
        /// <summary>
        /// Sets the default opening hours for the salon (9 AM to 6 PM) for all days of the week.
        /// </summary>
        public void SetDefaultOpeningHours()
        {
            foreach (var day in (DayOfWeek[])Enum.GetValues(typeof(DayOfWeek)))
                _openingHours.Add(new(Guid.NewGuid(), Id, day, new TimeSpan(9, 0, 0),
                    new TimeSpan(18, 0, 0)));
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
        
        /// <summary>
        /// Validates and sets the AddressId.
        /// </summary>
        /// <param name="addressId">The unique identifier for the address to set.</param>
        /// <exception cref="ArgumentException">Thrown when the AddressId is empty and not null.</exception>
        private void SetAddressId(Guid? addressId)
        {
            if(addressId != null)
                AddressId = (addressId != Guid.Empty)
                    ? addressId
                    : throw new ArgumentException("The address id cannot be empty", nameof(addressId));

            AddressId = addressId;
        }
    }
}