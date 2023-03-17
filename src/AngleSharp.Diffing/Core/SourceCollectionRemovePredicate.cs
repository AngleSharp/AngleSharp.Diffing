namespace AngleSharp.Diffing.Core;

/// <summary>
/// Predicate used to remove matched nodes from a source collection.
/// </summary>
public delegate FilterDecision SourceCollectionRemovePredicate(in ComparisonSource source);
