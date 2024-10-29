namespace SpaManagementSystem.Domain.Common.Helpers;

public static class EntityUpdater
{
    public static bool ApplyChanges(Dictionary<Action, bool> propertyChanges)
        => propertyChanges
            .Where(change => change.Value)
            .Select(change =>
            {
                change.Key();
                return true;
            })
            .Any();
}