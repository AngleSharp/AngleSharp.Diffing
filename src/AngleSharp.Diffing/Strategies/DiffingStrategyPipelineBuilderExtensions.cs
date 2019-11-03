using AngleSharp.Diffing.Strategies.AttributeStrategies;
using AngleSharp.Diffing.Strategies.TextNodeStrategies;

namespace AngleSharp.Diffing
{
    public static partial class DiffingStrategyPipelineBuilderExtensions
    {
        /// <summary>
        /// Sets up the diffing process using the default options.
        /// </summary>
        public static IDiffingStrategyCollection AddDefaultOptions(this IDiffingStrategyCollection builder)
        {
            return builder
                .IgnoreDiffAttributes()
                .IgnoreComments()
                .AddSearchingNodeMatcher()
                .AddCssSelectorMatcher()
                .AddAttributeNameMatcher()
                .AddElementComparer()                
                .AddIgnoreElementSupport()
                .AddStyleSheetComparer()
                .AddTextComparer(WhitespaceOption.Normalize, ignoreCase: false)
                .AddAttributeComparer()
                .AddClassAttributeComparer()
                .AddBooleanAttributeComparer(BooleanAttributeComparision.Strict)
                .AddStyleAttributeComparer();
            ;
        }
    }
}
