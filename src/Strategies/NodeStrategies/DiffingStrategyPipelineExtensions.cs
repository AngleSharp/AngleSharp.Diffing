using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Egil.AngleSharp.Diffing.Strategies.AttributeStrategies;

namespace Egil.AngleSharp.Diffing.Strategies.NodeStrategies
{
    public static class DiffingStrategyPipelineExtensions
    {
        /// <summary>
        /// Enables the one-to-one node-matching strategy during diffing.
        /// </summary>
        public static DiffingStrategyPipeline WithOneToOneNodeMatcher(this DiffingStrategyPipeline pipeline)
        {
            pipeline.AddMatcher(OneToOneNodeMatcher.Match, isSpecializedMatcher: false);
            return pipeline;
        }

        /// <summary>
        /// Enables the forward-searching node-matcher strategy during diffing.
        /// </summary>
        public static DiffingStrategyPipeline WithSearchingNodeMatcher(this DiffingStrategyPipeline pipeline)
        {
            pipeline.AddMatcher(ForwardSearchingNodeMatcher.Match, isSpecializedMatcher: false);
            return pipeline;
        }

        /// <summary>
        /// Enables the basic node compare strategy during diffing.
        /// </summary>
        public static DiffingStrategyPipeline WithNodeNameComparer(this DiffingStrategyPipeline pipeline)
        {
            pipeline.AddComparer(NodeComparer.Compare, isSpecializedComparer: false);
            return pipeline;
        }
    }
}
