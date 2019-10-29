using System;

namespace Egil.AngleSharp.Diffing.Extensions
{
    public static class EnumExtensions
    {
        public static TEnum Parse<TEnum>(this string enumString) where TEnum : System.Enum
        {
            return (TEnum)Enum.Parse(typeof(TEnum), enumString, true);
        }

    }
}
