using Egil.AngleSharp.Diffing.Strategies.TextNodeStrategies;

namespace Egil.AngleSharp.Diffing
{
    public static partial class DiffBuilderExtensions
    {
        /// <summary>
        /// Enables the one-to-one node-matching strategy during diffing.
        /// </summary>
        public static DiffBuilder WithTextComparer(this DiffBuilder builder, WhitespaceOption whitespaceOption, bool ignoreCase)
        {
            builder.WithFilter(new TextNodeFilter(whitespaceOption).Filter, isSpecializedFilter: true);
            builder.WithComparer(new TextNodeComparer(whitespaceOption, ignoreCase).Compare, isSpecializedComparer: false);
            return builder;
        }

        /// <summary>
        /// Enables the special style-tag style sheet text comparer.
        /// </summary>
        public static DiffBuilder WithStyleSheetComparer(this DiffBuilder builder)
        {
            builder.WithComparer(StyleSheetTextNodeComparer.Compare, isSpecializedComparer: true);
            return builder;
        }
    }
}
