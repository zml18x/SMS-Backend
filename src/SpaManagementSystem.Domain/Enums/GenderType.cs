namespace SpaManagementSystem.Domain.Enums;

public enum GenderType
{
    Male,
    Female,
    Other
}

public static class GenderTypeHelper
{
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