namespace SpaManagementSystem.Domain.Enums
{
    /// <summary>
    /// Specifies the gender categories available within the Spa Management System.
    /// This enumeration allows for the representation of gender in a way that includes the most commonly recognized categories.
    /// </summary>
    public enum GenderType
    {
        /// <summary>
        /// Represents the male gender.
        /// </summary>
        Male,

        /// <summary>
        /// Represents the female gender.
        /// </summary>
        Female,

        /// <summary>
        /// Represents genders that do not conform to the binary categories of male or female.
        /// </summary>
        Other
    }
    
    /// <summary>
    /// Provides utility methods for converting strings to <see cref="GenderType"/> values.
    /// </summary>
    public static class GenderTypeHelper
    {
        /// <summary>
        /// Converts a string representation of a gender into the corresponding <see cref="GenderType"/> enum.
        /// This method performs a case-insensitive comparison of the input string with known gender types.
        /// </summary>
        /// <param name="genderString">The gender string to convert.</param>
        /// <returns>A <see cref="GenderType"/> that corresponds to the given string.
        /// Returns <see cref="GenderType.Other"/> if the input does not match known gender types.</returns>
        public static GenderType ConvertToGenderType(string genderString)
        {
            if (genderString.ToLower() == "male")
            {
                return GenderType.Male;
            }
            else if (genderString.ToLower() == "female")
            {
                return GenderType.Female;
            }
            else if (genderString.ToLower() == "other")
            {
                return GenderType.Other;
            }

            return GenderType.Other;
        }
    }
}