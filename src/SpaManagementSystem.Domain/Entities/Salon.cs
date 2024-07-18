using System.Net.Mail;
using System.Text.RegularExpressions;
using SpaManagementSystem.Domain.Common;

namespace SpaManagementSystem.Domain.Entities
{
    public class Salon : BaseEntity
    {
        private ISet<OpeningHours> _openingHours = new HashSet<OpeningHours>();
        public Guid UserId { get; protected set; }
        public string Name { get; protected set; } = String.Empty;
        public string Email { get; protected set; } = String.Empty;
        public string PhoneNumber { get; protected set; } = String.Empty;
        public string? Description { get; protected set; }
        public Guid? AddressId { get; protected set; }
        public IEnumerable<OpeningHours> OpeningHours => _openingHours;


        
        public Salon(){}
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

        public void AddOrUpdateOpeningHours(DayOfWeek dayOfWeek, TimeSpan openingTime, TimeSpan closingTime, bool isClosed = false)
        {
            var existingOpeningHour = _openingHours.FirstOrDefault(x => x.DayOfWeek == dayOfWeek);

            if (existingOpeningHour == null)
                _openingHours.Add(new OpeningHours(Guid.NewGuid(), Id, dayOfWeek, openingTime, closingTime, isClosed));
            else
                existingOpeningHour.UpdateHours(openingTime, closingTime, isClosed);
        }

        public void SetDefaultOpeningHours()
        {
            foreach (var day in (DayOfWeek[])Enum.GetValues(typeof(DayOfWeek)))
                _openingHours.Add(new(Guid.NewGuid(), Id, day, new TimeSpan(9, 0, 0),
                    new TimeSpan(18, 0, 0)));
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