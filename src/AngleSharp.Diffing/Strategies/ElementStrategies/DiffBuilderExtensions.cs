using AngleSharp.Diffing.Strategies.ElementStrategies;

namespace AngleSharp.Diffing
{
    public static partial class DiffBuilderExtensions
    {
        /// <summary>
        /// Enables the CSS-selector matcher strategy during diffing.
        /// </summary>
        public static DiffBuilder WithCssSelectorMatcher(this DiffBuilder builder)
        {
            builder.WithMatcher(CssSelectorElementMatcher.Match, isSpecializedMatcher: true);
            return builder;
        }

        /// <summary>
        /// Enables the ignore element `diff:ignore` attribute during diffing.
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static DiffBuilder WithIgnoreElementSupport(this DiffBuilder builder)
        {
            builder.WithComparer(IgnoreElementComparer.Compare, isSpecializedComparer: true);
            return builder;
        }
    }
}
