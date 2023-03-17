namespace AngleSharp.Diffing.Core;

/// <summary>
/// Represents a delegate for removing sources from a <see cref="SourceMap"/>.
/// </summary>
public delegate FilterDecision SourceMapRemovePredicate(in AttributeComparisonSource source);
