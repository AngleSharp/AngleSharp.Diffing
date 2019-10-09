using System;
using System.Diagnostics.CodeAnalysis;
using AngleSharp.Dom;

namespace Egil.AngleSharp.Diffing.Extensions
{
    public static class ElementExtensions
    {
        public static bool TryGetAttrValue(this IElement element, string attributeName, out bool result)
            => TryGetAttrValue(element, attributeName, x => string.IsNullOrWhiteSpace(x) || bool.Parse(x), out result);

        public static bool TryGetAttrValue<T>(this IElement element, string attributeName, out T result) where T : System.Enum
        {
            return TryGetAttrValue(element, attributeName, ParseEnum, out result);

            static T ParseEnum(string enumValue)
            {
                return (T)Enum.Parse(typeof(T), enumValue, true);
            }
        }

        public static bool TryGetAttrValue<T>(this IElement element, string attributeName, Func<string, T> resultFunc, [NotNullWhen(true)] out T result)
        {
            if (element is null) throw new ArgumentNullException(nameof(element));
            if (resultFunc is null) throw new ArgumentNullException(nameof(resultFunc));

            result = default;
            if (element.Attributes[attributeName] is IAttr optAttr)
            {
                result = resultFunc(optAttr.Value);
                return true;
            }
            return false;
        }

        public static TEnum GetInlineOptionOrDefault<TEnum>(this IElement startElement, string optionName, TEnum defaultValue)
            where TEnum : System.Enum => GetInlineOptionOrDefault(startElement, optionName, x => x.Parse<TEnum>(), defaultValue);

        public static bool GetInlineOptionOrDefault(this IElement startElement, string optionName, bool defaultValue)
            => GetInlineOptionOrDefault(startElement, optionName, x => string.IsNullOrWhiteSpace(x) || bool.Parse(x), defaultValue);

        public static T GetInlineOptionOrDefault<T>(this IElement startElement, string optionName, Func<string, T> resultFunc, T defaultValue)
        {
            var element = startElement;

            while (element is { })
            {
                if (element.Attributes[optionName] is IAttr attr)
                {
                    return resultFunc(attr.Value);
                }
                element = element.ParentElement;
            }

            return defaultValue;
        }
    }
}
