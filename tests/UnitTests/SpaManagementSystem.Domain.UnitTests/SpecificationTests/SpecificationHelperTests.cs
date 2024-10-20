using FluentAssertions;
using SpaManagementSystem.Domain.Specifications;

namespace SpaManagementSystem.Domain.UnitTests.SpecificationTests;

public class SpecificationHelperTests
{
    [Fact]
    public void ValidateGuid_Should_AddError_When_GuidIsEmpty()
    {
        // Arrange
        var result = new ValidationResult(true);
        var emptyGuid = Guid.Empty;
        var errorMessage = "UserId is required.";

        // Act
        SpecificationHelper.ValidateGuid(emptyGuid, result, errorMessage);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().Which.Should().Be(errorMessage);
    }
    
    [Fact]
    public void ValidateGuid_Should_NotAddError_When_GuidIsNotEmpty()
    {
        // Arrange
        var result = new ValidationResult(true);
        var validGuid = Guid.NewGuid();
        var errorMessage = "UserId is required.";

        // Act
        SpecificationHelper.ValidateGuid(validGuid, result, errorMessage);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }
}