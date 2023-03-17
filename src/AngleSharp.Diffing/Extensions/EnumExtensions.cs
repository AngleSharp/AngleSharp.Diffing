namespace AngleSharp.Diffing.Extensions;

/// <summary>
/// Helper methods for enums
/// </summary>
public static class EnumExtensions
{
    /// <summary>
    /// Parse a string to an enum.
    /// </summary>
    public static TEnum Parse<TEnum>(this string enumString) where TEnum : System.Enum
    {
        return (TEnum)Enum.Parse(typeof(TEnum), enumString, true);
    }

}
