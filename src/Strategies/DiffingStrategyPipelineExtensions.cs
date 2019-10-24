using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Egil.AngleSharp.Diffing.Strategies.AttributeStrategies;
using Egil.AngleSharp.Diffing.Strategies.CommentStrategies;
using Egil.AngleSharp.Diffing.Strategies.ElementStrategies;
using Egil.AngleSharp.Diffing.Strategies.NodeStrategies;
using Egil.AngleSharp.Diffing.Strategies.TextNodeStrategies;

namespace Egil.AngleSharp.Diffing.Strategies
{
    public static class DiffingStrategyPipelineExtensions
    {
        /// <summary>
        /// Sets up the diffing process using the default options.
        /// </summary>
        public static DiffingStrategyPipeline WithDefaultOptions(this DiffingStrategyPipeline pipeline)
        {
            return pipeline
                .IgnoreDiffAttributes()
                .IgnoreComments()
                .WithSearchingNodeMatcher()
                .WithCssSelectorMatcher()
                .WithAttributeNameMatcher()
                .WithNodeNameComparer()
                .WithIgnoreElementSupport()
                .WithStyleSheetComparer()
                .WithTextComparer(WhitespaceOption.Normalize, ignoreCase: false)
                .WithAttributeComparer()
                .WithClassAttributeComparer()
                .WithBooleanAttributeComparer(BooleanAttributeComparision.Strict)
                .WithStyleAttributeComparer()
                .WithInlineAttributeIgnoreSupport();
            ;
        }
    }
}
