using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Egil.AngleSharp.Diffing.Strategies.AttributeStrategies;
using Egil.AngleSharp.Diffing.Strategies.NodeStrategies;

namespace Egil.AngleSharp.Diffing.Strategies.ElementStrategies
{
    public static class DiffingStrategyPipelineExtensions
    {
        /// <summary>
        /// Enables the CSS-selector matcher strategy during diffing.
        /// </summary>
        public static DiffingStrategyPipeline WithCssSelectorMatcher(this DiffingStrategyPipeline pipeline)
        {
            pipeline.AddMatcher(CssSelectorElementMatcher.Match, isSpecializedMatcher: true);
            return pipeline;
        }

        /// <summary>
        /// Enables the ignore element `diff:ignore` attribute during diffing.
        /// </summary>
        /// <param name="pipeline"></param>
        /// <returns></returns>
        public static DiffingStrategyPipeline WithIgnoreElementSupport(this DiffingStrategyPipeline pipeline)
        {
            pipeline.AddComparer(IgnoreElementComparer.Compare, isSpecializedComparer: true);
            return pipeline;
        }
    }
}
