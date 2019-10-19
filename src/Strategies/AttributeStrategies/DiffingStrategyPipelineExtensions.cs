using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Egil.AngleSharp.Diffing.Strategies.AttributeStrategies
{
    public static class DiffingStrategyPipelineExtensions
    {
        /// <summary>
        /// Enables ignoring of the special "diff"-attributes during diffing.
        /// </summary>
        public static DiffingStrategyPipeline IgnoreDiffAttributes(this DiffingStrategyPipeline pipeline)
        {
            pipeline.AddFilter(IgnoreDiffAttributesFilter.Filter, true);
            return pipeline;
        }

        /// <summary>
        /// Enables the name-based attribute matching strategy during diffing.
        /// </summary>
        public static DiffingStrategyPipeline WithAttributeNameMatcher(this DiffingStrategyPipeline pipeline)
        {
            pipeline.AddMatcher(AttributeNameMatcher.Match, isSpecializedMatcher: false);
            return pipeline;
        }

        /// <summary>
        /// Enables the basic name and value attribute comparer during diffing.
        /// </summary>
        public static DiffingStrategyPipeline WithAttributeComparer(this DiffingStrategyPipeline pipeline)
        {
            pipeline.AddMatcher(PostfixedAttributeMatcher.Match, isSpecializedMatcher: true);
            pipeline.AddComparer(AttributeComparer.Compare, isSpecializedComparer: false);
            return pipeline;
        }

        /// <summary>
        /// Enables the special class attribute comparer during diffing.
        /// </summary>
        public static DiffingStrategyPipeline WithClassAttributeComparer(this DiffingStrategyPipeline pipeline)
        {
            pipeline.AddComparer(ClassAttributeComparer.Compare, isSpecializedComparer: true);
            return pipeline;
        }

        /// <summary>
        /// Enables the special boolean attribute comparer during diffing.
        /// </summary>
        public static DiffingStrategyPipeline WithBooleanAttributeComparer(this DiffingStrategyPipeline pipeline, BooleanAttributeComparision booleanAttributeComparision)
        {
            pipeline.AddComparer(new BooleanAttributeComparer(booleanAttributeComparision).Compare, isSpecializedComparer: true);
            return pipeline;
        }

        /// <summary>
        /// Enables ignoring attributes using the :ignore postfix for attributes during diffing.
        /// </summary>
        public static DiffingStrategyPipeline WithInlineAttributeIgnoreSupport(this DiffingStrategyPipeline pipeline)
        {
            pipeline.AddMatcher(IgnoreAttributeMatcher.Match, isSpecializedMatcher: true);
            pipeline.AddComparer(IgnoreAttributeComparer.Compare, isSpecializedComparer: true);
            return pipeline;
        }

        /// <summary>
        /// Enables the special style attributes comparer during diffing.
        /// </summary>
        public static DiffingStrategyPipeline WithStyleAttributeComparer(this DiffingStrategyPipeline pipeline)
        {
            pipeline.AddComparer(StyleAttributeComparer.Compare, isSpecializedComparer: true);
            return pipeline;
        }
    }
}
