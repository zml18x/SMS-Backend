using FluentAssertions;
using SpaManagementSystem.Domain.Enums;
using SpaManagementSystem.Domain.Entities;

namespace SpaManagementSystem.Domain.UnitTests.EntitiesTests.UserProfileTests
{
    public class UserProfileTests
    {
        [Fact]
        public void UserProfileConstructorSetsPropertiesCorrectly()
        {
            // Arrange
            var id = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var firstName = "John";
            var lastName = "Doe";
            var gender = GenderType.Male;
            var dateOfBirth = DateOnly.FromDateTime(DateTime.Today.AddYears(-20));

            // Act
            var user = new UserProfile(id, userId, firstName, lastName, gender, dateOfBirth);

            // Assert
            user.Should().NotBeNull();
            user.Id.Should().Be(id);
            user.UserId.Should().Be(userId);
            user.FirstName.Should().Be(firstName);
            user.LastName.Should().Be(lastName);
            user.Gender.Should().Be(gender);
            user.DateOfBirth.Should().Be(dateOfBirth);
        }
        
        [Fact]
        public void UserProfileConstructorShouldThrowsArgumentExceptionWhenIdIsIncorrect()
        {
            // Arrange
            var id = Guid.Empty;
            var userId = Guid.NewGuid();
            var firstName = "John";
            var lastName = "Doe";
            var gender = GenderType.Male;
            var dateOfBirth = DateOnly.FromDateTime(DateTime.Today.AddYears(-20));
            

            // Act & Assert
            new Action(() => new UserProfile(id, userId, firstName, lastName, gender, dateOfBirth))
                .Should().Throw<ArgumentException>();
        }
        
        [Fact]
        public void UserProfileConstructorShouldThrowsArgumentExceptionWhenUserIdIsIncorrect()
        {
            // Arrange
            var id = Guid.NewGuid();
            var userId = Guid.Empty;
            var firstName = "John";
            var lastName = "Doe";
            var gender = GenderType.Male;
            var dateOfBirth = DateOnly.FromDateTime(DateTime.Today.AddYears(-20));
            

            // Act & Assert
            new Action(() => new UserProfile(id, userId, firstName, lastName, gender, dateOfBirth))
                .Should().Throw<ArgumentException>();
        }
        
        [Fact]
        public void UserProfileConstructorShouldThrowsArgumentExceptionWhenFirstNameIsNull()
        {
            // Arrange
            var id = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var lastName = "Doe";
            var gender = GenderType.Male;
            var dateOfBirth = DateOnly.FromDateTime(DateTime.Today.AddYears(-20));
            

            // Act & Assert
            new Action(() => new UserProfile(id, userId, null!, lastName, gender, dateOfBirth))
                .Should().Throw<ArgumentException>();
        }
        
        [Fact]
        public void UserProfileConstructorShouldThrowsArgumentExceptionWhenLastNameIsNull()
        {
            // Arrange
            var id = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var firstName = "John";
            var gender = GenderType.Male;
            var dateOfBirth = DateOnly.FromDateTime(DateTime.Today.AddYears(-20));
            

            // Act & Assert
            new Action(() => new UserProfile(id, userId, firstName, null!, gender, dateOfBirth))
                .Should().Throw<ArgumentException>();
        }
        
        [Fact]
        public void UserProfileConstructorShouldThrowsArgumentExceptionWhenGenderIsNotDefinedGenderTypeEnumValue()
        {
            // Arrange
            var id = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var firstName = "John";
            var lastName = "Doe";
            var gender = (GenderType)200;
            var dateOfBirth = DateOnly.FromDateTime(DateTime.Today.AddYears(-20));
            

            // Act & Assert
            new Action(() => new UserProfile(id, userId, firstName, lastName, gender, dateOfBirth))
                .Should().Throw<ArgumentException>();
        }
        
        [Fact]
        public void UserProfileConstructorShouldThrowsArgumentExceptionWhenDateOfBirthIsInTheFuture()
        {
            // Arrange
            var id = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var firstName = "John";
            var lastName = "Doe";
            var gender = GenderType.Male;
            var dateOfBirth = DateOnly.FromDateTime(DateTime.Today.AddDays(1));
            

            // Act & Assert
            new Action(() => new UserProfile(id, userId, firstName, lastName, gender, dateOfBirth))
                .Should().Throw<ArgumentException>();
        }
    }
}