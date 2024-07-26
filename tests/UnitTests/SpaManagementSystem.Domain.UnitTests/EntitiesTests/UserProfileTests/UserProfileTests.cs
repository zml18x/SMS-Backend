using FluentAssertions;
using SpaManagementSystem.Domain.Enums;
using SpaManagementSystem.Domain.Entities;

namespace SpaManagementSystem.Domain.UnitTests.EntitiesTests.UserProfileTests;

public class UserProfileTests
{
    [Fact]
    public void UserProfile_Constructor_SetsPropertiesCorrectly()
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
    public void UserProfile_Constructor_ShouldThrowArgumentException_WhenIdIsEmpty()
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
    public void UserProfile_Constructor_ShouldThrowArgumentException_WhenUserIdIsEmpty()
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
    public void UserProfile_Constructor_ShouldThrowArgumentException_WhenFirstNameIsNull()
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
    public void UserProfile_Constructor_ShouldThrowArgumentException_WhenLastNameIsNull()
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
    public void UserProfile_Constructor_ShouldThrowArgumentException_WhenGenderIsInvalid()
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
    public void UserProfile_Constructor_ShouldThrowArgumentException_WhenDateOfBirthIsInTheFuture()
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