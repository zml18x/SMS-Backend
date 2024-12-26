namespace SpaManagementSystem.Domain.Common.Helpers;

public static class EntityUpdater
{
    public static bool ApplyChanges(Dictionary<Action, Func<bool>> propertyChanges)
    {
        bool anyChangesApplied = false;

        // Przechodzimy przez każdy element, wykonując zmianę tylko wtedy, gdy warunek jest spełniony
        foreach (var change in propertyChanges)
        {
            // Sprawdzamy, czy właściwość powinna zostać zmieniona
            if (change.Value()) // Evaluating the change condition
            {
                change.Key(); // Jeśli tak, wykonujemy akcję
                anyChangesApplied = true; // Zmieniono dane
            }
        }

        return anyChangesApplied;
    }
}
