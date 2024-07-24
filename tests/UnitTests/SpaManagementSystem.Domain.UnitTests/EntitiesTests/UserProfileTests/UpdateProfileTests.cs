using FluentAssertions;
using SpaManagementSystem.Domain.Enums;
using SpaManagementSystem.Domain.Entities;

namespace SpaManagementSystem.Domain.UnitTests.EntitiesTests.UserProfileTests;

public class UpdateProfileTests
{
    public class UserProfileTests
    {
        [Fact]
        public void UpdateProfile_ShouldUpdateAllFields_WhenAllFieldsAreDifferent()
        {
            // Arrange
            var id = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var firstName = "John";
            var lastName = "Doe";
            var gender = GenderType.Male;
            var dateOfBirth = DateOnly.FromDateTime(DateTime.Today.AddYears(-20));
            var user = new UserProfile(id, userId, firstName, lastName, gender, dateOfBirth);
            var updatedAtBeforeUpdate = user.UpdatedAt;
            
            var newFirstName = "Jane";
            var newLastName = "Smith";
            var newGender = GenderType.Female;
            var newDateOfBirth = DateOnly.FromDateTime(DateTime.Today.AddYears(-10));

            // Act
            var result = user.UpdateProfile(newFirstName, newLastName, newGender, newDateOfBirth);

            // Assert
            result.Should().BeTrue();
            user.FirstName.Should().Be(newFirstName);
            user.LastName.Should().Be(newLastName);
            user.Gender.Should().Be(newGender);
            user.DateOfBirth.Should().Be(newDateOfBirth);
            user.UpdatedAt.Should().NotBe(updatedAtBeforeUpdate);
        }

        [Fact]
        public void UpdateProfile_ShouldNotUpdateAnyFields_WhenAllFieldsAreSame()
        {
            // Arrange
            var id = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var firstName = "John";
            var lastName = "Doe";
            var gender = GenderType.Male;
            var dateOfBirth = DateOnly.FromDateTime(DateTime.Today.AddYears(-20));
            var user = new UserProfile(id, userId, firstName, lastName, gender, dateOfBirth);
            var updatedAtBeforeUpdate = user.UpdatedAt;

            // Act
            var result = user.UpdateProfile(firstName, lastName, gender, dateOfBirth);

            // Assert
            result.Should().BeFalse();
            user.UpdatedAt.Should().Be(updatedAtBeforeUpdate);
        }

        [Fact]
        public void UpdateProfile_ShouldUpdateFirstName_WhenFirstNameIsDifferent()
        {
            // Arrange
            var id = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var firstName = "John";
            var lastName = "Doe";
            var gender = GenderType.Male;
            var dateOfBirth = DateOnly.FromDateTime(DateTime.Today.AddYears(-20));
            var user = new UserProfile(id, userId, firstName, lastName, gender, dateOfBirth);
            var updatedAtBeforeUpdate = user.UpdatedAt;
            
            var newFirstName = "Jane";

            // Act
            var result = user.UpdateProfile(newFirstName, lastName, gender, dateOfBirth);

            // Assert
            result.Should().BeTrue();
            user.FirstName.Should().Be(newFirstName);
            user.LastName.Should().Be(lastName);
            user.Gender.Should().Be(gender);
            user.DateOfBirth.Should().Be(dateOfBirth);
            user.UpdatedAt.Should().NotBe(updatedAtBeforeUpdate);
        }

        [Fact]
        public void UpdateProfile_ShouldUpdateLastName_WhenLastNameIsDifferent()
        {
            // Arrange
            var id = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var firstName = "John";
            var lastName = "Doe";
            var gender = GenderType.Male;
            var dateOfBirth = DateOnly.FromDateTime(DateTime.Today.AddYears(-20));
            var user = new UserProfile(id, userId, firstName, lastName, gender, dateOfBirth);
            var updatedAtBeforeUpdate = user.UpdatedAt;
            
            var newLastName = "Smith";

            // Act
            var result = user.UpdateProfile(firstName, newLastName, gender, dateOfBirth);

            // Assert
            result.Should().BeTrue();
            user.FirstName.Should().Be(firstName);
            user.LastName.Should().Be(newLastName);
            user.Gender.Should().Be(gender);
            user.DateOfBirth.Should().Be(dateOfBirth);
            user.UpdatedAt.Should().NotBe(updatedAtBeforeUpdate);
        }

        [Fact]
        public void UpdateProfile_ShouldUpdateGender_WhenGenderIsDifferent()
        {
            // Arrange
            var id = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var firstName = "John";
            var lastName = "Doe";
            var gender = GenderType.Male;
            var dateOfBirth = DateOnly.FromDateTime(DateTime.Today.AddYears(-20));
            var user = new UserProfile(id, userId, firstName, lastName, gender, dateOfBirth);
            var updatedAtBeforeUpdate = user.UpdatedAt;
            
            var newGender = GenderType.Female;

            // Act
            var result = user.UpdateProfile(firstName, lastName, newGender, dateOfBirth);

            // Assert
            result.Should().BeTrue();
            user.FirstName.Should().Be(firstName);
            user.LastName.Should().Be(lastName);
            user.Gender.Should().Be(newGender);
            user.DateOfBirth.Should().Be(dateOfBirth);
            user.UpdatedAt.Should().NotBe(updatedAtBeforeUpdate);
        }

        [Fact]
        public void UpdateProfile_ShouldUpdateDateOfBirth_WhenDateOfBirthIsDifferent()
        {
            // Arrange
            var id = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var firstName = "John";
            var lastName = "Doe";
            var gender = GenderType.Male;
            var dateOfBirth = DateOnly.FromDateTime(DateTime.Today.AddYears(-20));
            var user = new UserProfile(id, userId, firstName, lastName, gender, dateOfBirth);
            var updatedAtBeforeUpdate = user.UpdatedAt;
            
            var newDateOfBirth = DateOnly.FromDateTime(DateTime.Today.AddYears(-10));

            // Act
            var result = user.UpdateProfile(firstName, lastName, gender, newDateOfBirth);

            // Assert
            result.Should().BeTrue();
            user.FirstName.Should().Be(firstName);
            user.LastName.Should().Be(lastName);
            user.Gender.Should().Be(gender);
            user.DateOfBirth.Should().Be(newDateOfBirth);
            user.UpdatedAt.Should().NotBe(updatedAtBeforeUpdate);
        }
        
        [Fact]
        public void UpdateProfile_ShouldNotUpdateUpdatedAt_WhenNoFieldIsUpdated()
        {
            // Arrange
            var id = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var firstName = "John";
            var lastName = "Doe";
            var gender = GenderType.Male;
            var dateOfBirth = DateOnly.FromDateTime(DateTime.Today.AddYears(-20));
            var user = new UserProfile(id, userId, firstName, lastName, gender, dateOfBirth);
            var updatedAtBeforeUpdate = user.UpdatedAt;
            
            // Act
            var result = user.UpdateProfile(firstName, lastName, gender, dateOfBirth);

            // Assert
            result.Should().BeFalse();
            user.UpdatedAt.Should().Be(updatedAtBeforeUpdate);
        }
    }
}