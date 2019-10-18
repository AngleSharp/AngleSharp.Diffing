using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Egil.AngleSharp.Diffing.Strategies.AttributeStrategies;
using Egil.AngleSharp.Diffing.Strategies.NodeStrategies;

namespace Egil.AngleSharp.Diffing.Strategies.TextNodeStrategies
{
    public static class DiffingStrategyPipelineExtensions
    {
        /// <summary>
        /// Enables the one-to-one node-matching strategy during diffing.
        /// </summary>
        public static DiffingStrategyPipeline WithTextComparer(this DiffingStrategyPipeline pipeline, WhitespaceOption whitespaceOption, bool ignoreCase)
        {
            pipeline.AddFilter(new TextNodeFilter(whitespaceOption).Filter, isSpecializedFilter: true);
            pipeline.AddComparer(new TextNodeComparer(whitespaceOption, ignoreCase).Compare, isSpecializedComparer: false);
            return pipeline;
        }

        /// <summary>
        /// Enables the special style-tag style sheet text comparer.
        /// </summary>
        public static DiffingStrategyPipeline WithStyleSheetComparer(this DiffingStrategyPipeline pipeline)
        {
            pipeline.AddComparer(StyleSheetTextNodeComparer.Compare, isSpecializedComparer: true);
            return pipeline;
        }
    }
}
