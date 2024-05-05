﻿using SpaManagementSystem.Domain.Enums;
using SpaManagementSystem.Domain.Entities;
using SpaManagementSystem.Domain.Interfaces;
using SpaManagementSystem.Application.Dto;
using SpaManagementSystem.Application.Exceptions;
using SpaManagementSystem.Application.Interfaces;


namespace SpaManagementSystem.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserProfileRepository _userProfileRepository;



        public UserService(IUserProfileRepository userProfileRepository)
        {
            _userProfileRepository = userProfileRepository;
        }



        public async Task CreateProfileAsync(Guid userId, string firstName, string lastName, string gender,
            DateOnly dateOfBirth)
        {
            var userProfile = await _userProfileRepository.GetByUserIdAsync(userId);

            if (userProfile != null)
                throw new InvalidOperationException("User profile already exists for the specified user ID.");

            userProfile = new UserProfile(Guid.NewGuid(), userId, firstName, lastName,
                GenderTypeHelper.ConvertToGenderType(gender), dateOfBirth);

            await _userProfileRepository.CreateAsync(userProfile);
            await _userProfileRepository.SaveChangesAsync();
        }

        public async Task<UserAccountDetailsDto> GetAccountDetailsAsync(Guid userId, string email, string phoneNumber)
        {
            var userProfile = await _userProfileRepository.GetByUserIdAsync(userId);

            if (userProfile == null)
                return new UserAccountDetailsDto(userId, email, phoneNumber);

            return new UserAccountDetailsDto(userId, email, phoneNumber, userProfile.FirstName, userProfile.LastName,
                userProfile.Gender.ToString(), userProfile.DateOfBirth);
        }

        public async Task<bool> UpdateProfileAsync(Guid userId, string? firstName, string? lastName, string? gender,
            DateOnly? dateOfBirth)
        {
            var userProfile = await _userProfileRepository.GetByUserIdAsync(userId);

            if (userProfile == null)
                throw new NotFoundException($"The user profile for user with id {userId} was not found.");
            
            GenderType? convertedGender = gender == null ? null : GenderTypeHelper.ConvertToGenderType(gender);
            
            var isUpdated = userProfile.UpdateProfile(firstName, lastName, convertedGender, dateOfBirth);

            if (isUpdated)
            {
                _userProfileRepository.Update(userProfile);
                await _userProfileRepository.SaveChangesAsync();
            }

            return isUpdated;
        }
    }
}