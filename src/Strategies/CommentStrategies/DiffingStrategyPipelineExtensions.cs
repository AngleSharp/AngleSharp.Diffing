using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Egil.AngleSharp.Diffing.Strategies.AttributeStrategies;

namespace Egil.AngleSharp.Diffing.Strategies.CommentStrategies
{
    public static class DiffingStrategyPipelineExtensions
    {
        /// <summary>
        /// Enables ignoring HTML comments during diffing.
        /// </summary>
        public static DiffingStrategyPipeline IgnoreComments(this DiffingStrategyPipeline pipeline)
        {
            pipeline.AddFilter(IgnoreCommentsFilter.Filter, true);
            return pipeline;
        }
    }
}
