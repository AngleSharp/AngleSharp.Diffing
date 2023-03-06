namespace AngleSharp.Diffing.Extensions;

/// <summary>
/// Helper methods for <see cref="IElement"/> types.
/// </summary>
public static class ElementExtensions
{
    /// <summary>
    /// Try to get an attribute value off of an element.
    /// Returns true when the attribute was found, false otherwise.
    /// </summary>
    public static bool TryGetAttrValue(this IElement element, string attributeName, out bool result)
    {
        return TryGetAttrValue(element, attributeName, ParseBoolAttribute, out result);

        static bool ParseBoolAttribute(string boolValue) => string.IsNullOrWhiteSpace(boolValue) || bool.Parse(boolValue);
    }

    /// <summary>
    /// Try to get an attribute value off of an element.
    /// Returns true when the attribute was found, false otherwise.
    /// </summary>
    public static bool TryGetAttrValue(this IElement element, string attributeName, [NotNullWhen(true)] out string result)
    {
        return TryGetAttrValue(element, attributeName, GetStringAttrValue, out result);

        static string GetStringAttrValue(string value) => value;
    }

    /// <summary>
    /// Try to get an attribute value off of an element.
    /// Returns true when the attribute was found, false otherwise.
    /// </summary>
    public static bool TryGetAttrValue<T>(this IElement element, string attributeName, out T result) where T : System.Enum
    {
        return TryGetAttrValue(element, attributeName, ParseEnum, out result);

        static T ParseEnum(string enumValue) => (T)Enum.Parse(typeof(T), enumValue, true);
    }

    /// <summary>
    /// Try to get an attribute value off of an element.
    /// Returns true when the attribute was found, false otherwise.
    /// </summary>
    public static bool TryGetAttrValue<T>(this IElement element, string attributeName, Func<string, T> resultFunc, [NotNullWhen(true)] out T result) where T : notnull
    {
        if (element is null)
            throw new ArgumentNullException(nameof(element));
        if (resultFunc is null)
            throw new ArgumentNullException(nameof(resultFunc));

        if (element.Attributes[attributeName] is IAttr optAttr)
        {
            result = resultFunc(optAttr.Value);
            return true;
        }
        else
        {
#pragma warning disable CS8601 // Possible null reference assignment.
            result = default(T);
#pragma warning restore CS8601 // Possible null reference assignment.
            return false;
        }
    }

    /// <summary>
    /// Try to get an inline diff option off of an element.
    /// Returns true when the attribute was found, false otherwise.
    /// </summary>
    public static TEnum GetInlineOptionOrDefault<TEnum>(this IElement startElement, string optionName, TEnum defaultValue)
        where TEnum : System.Enum => GetInlineOptionOrDefault(startElement, optionName, x => x.Parse<TEnum>(), defaultValue);

    /// <summary>
    /// Try to get an inline diff option off of an element.
    /// Returns true when the attribute was found, false otherwise.
    /// </summary>
    public static bool GetInlineOptionOrDefault(this IElement startElement, string optionName, bool defaultValue)
        => GetInlineOptionOrDefault(startElement, optionName, x => string.IsNullOrWhiteSpace(x) || bool.Parse(x), defaultValue);

    /// <summary>
    /// Try to get an inline diff option off of an element.
    /// Returns true when the attribute was found, false otherwise.
    /// </summary>
    public static T GetInlineOptionOrDefault<T>(this IElement startElement, string optionName, Func<string, T> resultFunc, T defaultValue)
    {
        if (startElement is null)
            throw new ArgumentNullException(nameof(startElement));
        if (resultFunc is null)
            throw new ArgumentNullException(nameof(resultFunc));

        var element = startElement;

        while (element is not null)
        {
            if (element.Attributes[optionName] is IAttr attr)
            {
                return resultFunc(attr.Value);
            }
            element = element.ParentElement;
        }

        return defaultValue;
    }

    /// <summary>
    /// Try to get the index of the node in its parent's ChildNodes list.
    /// Returns true if index was found. False otherwise.
    /// </summary>
    public static bool TryGetNodeIndex(this INode node, [NotNullWhen(true)] out int index)
    {
        index = -1;

        if (node.ParentElement is null)
            return false;

        var parentElement = node.ParentElement;

        for (int i = 0; i < parentElement.ChildNodes.Length; i++)
        {
            if (parentElement.ChildNodes[i].Equals(node))
            {
                index = i;
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Gets all parents of a node, starting with the nearest node. 
    /// </summary>
    public static IEnumerable<INode> GetParents(this INode node)
    {
        var parent = node.Parent;
        while (parent is not null)
        {
            yield return parent;
            parent = parent.Parent;
        }
    }
}
