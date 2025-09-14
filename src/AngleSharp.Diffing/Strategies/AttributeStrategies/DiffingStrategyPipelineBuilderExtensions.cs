using AngleSharp.Diffing.Strategies;
using AngleSharp.Diffing.Strategies.AttributeStrategies;

namespace AngleSharp.Diffing;

/// <summary>
/// Helper methods for registering strategies.
/// </summary>
public static partial class DiffingStrategyPipelineBuilderExtensions
{
    /// <summary>
    /// Enables ignoring of the special "diff"-attributes during diffing.
    /// </summary>
    public static IDiffingStrategyCollection IgnoreDiffAttributes(this IDiffingStrategyCollection builder)
    {
        builder.AddFilter(IgnoreDiffAttributesFilter.Filter, StrategyType.Specialized);
        return builder;
    }

    /// <summary>
    /// Enables the name-based attribute matching strategy during diffing.
    /// </summary>
    public static IDiffingStrategyCollection AddAttributeNameMatcher(this IDiffingStrategyCollection builder)
    {
        builder.AddMatcher(AttributeNameMatcher.Match, StrategyType.Generalized);
        builder.AddMatcher(IgnoreAttributeStrategy.Match, StrategyType.Generalized);
        return builder;
    }

    /// <summary>
    /// Enables the basic name and value attribute comparer during diffing.
    /// </summary>
    public static IDiffingStrategyCollection AddAttributeComparer(this IDiffingStrategyCollection builder)
    {
        builder.AddMatcher(PostfixedAttributeMatcher.Match, StrategyType.Specialized);
        builder.AddComparer(AttributeComparer.Compare, StrategyType.Generalized);
        builder.AddComparer(IgnoreAttributeStrategy.Compare, StrategyType.Specialized);
        return builder;
    }

    /// <summary>
    /// Enables the special class attribute comparer during diffing.
    /// </summary>
    public static IDiffingStrategyCollection AddClassAttributeComparer(this IDiffingStrategyCollection builder)
    {
        builder.AddComparer(ClassAttributeComparer.Compare, StrategyType.Specialized);
        return builder;
    }

    /// <summary>
    /// Enables the special boolean attribute comparer during diffing.
    /// </summary>
    public static IDiffingStrategyCollection AddBooleanAttributeComparer(this IDiffingStrategyCollection builder, BooleanAttributeComparision booleanAttributeComparision)
    {
        builder.AddComparer(new BooleanAttributeComparer(booleanAttributeComparision).Compare, StrategyType.Specialized);
        return builder;
    }

    /// <summary>
    /// Enables the special style attributes comparer during diffing.
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="ignoreOrder">Then the flag is true, the comparer orders the styles in ascending order before comparing them.
    /// Therefore two styles are identical if they have the same properties and values but in different order.
    /// </param>
    public static IDiffingStrategyCollection AddStyleAttributeComparer(this IDiffingStrategyCollection builder, bool ignoreOrder = false)
    {
        if (ignoreOrder)
        {
            builder.AddComparer(OrderingStyleAttributeComparer.Compare, StrategyType.Specialized);
        }
        else
        {
            builder.AddComparer(StyleAttributeComparer.Compare, StrategyType.Specialized);
        }
        return builder;
    }
}
