using AngleSharp.Diffing.Strategies.AttributeStrategies;
using AngleSharp.Diffing.Strategies.TextNodeStrategies;

namespace AngleSharp.Diffing
{
    public static partial class DiffingStrategyPipelineBuilderExtensions
    {
        /// <summary>
        /// Sets up the diffing process using the default options.
        /// </summary>
        public static IDiffingStrategyPipelineBuilder WithDefaultOptions(this IDiffingStrategyPipelineBuilder builder)
        {
            return builder
                .IgnoreDiffAttributes()
                .IgnoreComments()
                .WithSearchingNodeMatcher()
                .WithCssSelectorMatcher()
                .WithAttributeNameMatcher()
                .WithElementComparer()                
                .WithIgnoreElementSupport()
                .WithStyleSheetComparer()
                .WithTextComparer(WhitespaceOption.Normalize, ignoreCase: false)
                .WithAttributeComparer()
                .WithClassAttributeComparer()
                .WithBooleanAttributeComparer(BooleanAttributeComparision.Strict)
                .WithStyleAttributeComparer();
            ;
        }
    }
}
