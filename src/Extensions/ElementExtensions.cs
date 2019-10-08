using System;
using AngleSharp.Dom;

namespace Egil.AngleSharp.Diffing.Extensions
{
    public static class ElementExtensions
    {
        public static bool TryGetAttrValue<T>(this IElement element, string attributeName, out T result) where T : struct
        {
            if (element is null) throw new ArgumentNullException(nameof(element));
            result = default;
            return element.Attributes[attributeName] is IAttr optAttr
                && Enum.TryParse(optAttr.Value, true, out result);
        }

        public static TEnum GetInlineOptionOrDefault<TEnum>(this IElement startElement, string optionName, TEnum defaultValue)
            where TEnum : System.Enum => GetInlineOptionOrDefault(startElement, optionName, x => x.Parse<TEnum>(), defaultValue);

        public static bool GetInlineOptionOrDefault(this IElement startElement, string optionName, bool defaultValue)
            => GetInlineOptionOrDefault(startElement, optionName, x => bool.Parse(x), defaultValue);

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
