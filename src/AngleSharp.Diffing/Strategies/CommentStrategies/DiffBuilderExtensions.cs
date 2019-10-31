using Egil.AngleSharp.Diffing.Strategies.CommentStrategies;

namespace Egil.AngleSharp.Diffing
{
    public static partial class DiffBuilderExtensions
    {
        /// <summary>
        /// Enables ignoring HTML comments during diffing.
        /// </summary>
        public static DiffBuilder IgnoreComments(this DiffBuilder builder)
        {
            builder.WithFilter(IgnoreCommentsFilter.Filter, true);
            return builder;
        }
    }
}
