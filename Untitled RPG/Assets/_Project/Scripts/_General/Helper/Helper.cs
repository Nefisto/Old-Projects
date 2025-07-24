using System.Text.RegularExpressions;

public static partial class Helper
{
    /// <summary>
    /// Some attribute names appear on screen using a different name then it actual name on code, this helper create this difference
    /// </summary>
    /// <param name="attributeType">Properly attribute type</param>
    /// <returns>Formatted name</returns>
    public static string AttributeTypeToAttributeName (AttributeType attributeType)
    {
        var attributeName = attributeType switch
        {
            AttributeType.HealthMax => $"HP. Max",
            AttributeType.HealthRegen => $"HP. Regen",
            _ => attributeType.ToString()
        };

        return FormatAttributeName(attributeName);
    }

    private static string FormatAttributeName (string attributeName)
        => Regex.Replace(attributeName, @"((?<=\p{Ll})\p{Lu})|((?!\A)\p{Lu}(?>\p{Ll}))", " $0");
}