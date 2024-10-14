using System.Text.RegularExpressions;

namespace AngleSharp.Diffing.Strategies.TextNodeStrategies;

/// <summary>
/// Represents the text node comparer strategy.
/// </summary>
public class TextNodeComparer
{
    private static readonly string[] DefaultPreserveElement = new string[] { "PRE", "SCRIPT", "STYLE" };
    private const string WHITESPACE_ATTR_NAME = "diff:whitespace";
    private const string IGNORECASE_ATTR_NAME = "diff:ignorecase";
    private const string REGEX_ATTR_NAME = "diff:regex";
    private static readonly Regex WhitespaceReplace = new(@"\s+", RegexOptions.Compiled | RegexOptions.CultureInvariant, TimeSpan.FromSeconds(5));

    /// <summary>
    /// Gets the whitespace option of the comparer instance.
    /// </summary>
    public WhitespaceOption Whitespace { get; }

    /// <summary>
    /// Gets whether the comparer has been configured to ignore case.
    /// </summary>
    public bool IgnoreCase { get; }

    /// <summary>
    /// Instantiates a <see cref="TextNodeComparer"/> with the provided configuration.
    /// </summary>
    public TextNodeComparer(WhitespaceOption option = WhitespaceOption.Preserve, bool ignoreCase = false)
    {
        Whitespace = option;
        IgnoreCase = ignoreCase;
    }

    /// <summary>
    /// The text node comparer strategy.
    /// </summary>
    public CompareResult Compare(in Comparison comparison, CompareResult currentDecision)
    {
        if (currentDecision.IsSameOrSkip)
            return currentDecision;

        if (comparison.TryGetNodesAsType<IText>(out var controlTextNode, out var testTextNode))
            return Compare(comparison, controlTextNode, testTextNode);
        else
            return currentDecision;
    }

    private CompareResult Compare(in Comparison comparison, IText controlTextNode, IText testTextNode)
    {
        var option = GetWhitespaceOption(controlTextNode);
        var compareMethod = GetCompareMethod(controlTextNode);
        var controlText = controlTextNode.Data;
        var testText = testTextNode.Data;

        if(option == WhitespaceOption.Normalize || option == WhitespaceOption.RemoveWhitespaceNodes)
        {
            controlText = controlText.Trim();
            testText = testText.Trim();
        }

        if (option == WhitespaceOption.Normalize)
        {
            controlText = WhitespaceReplace.Replace(controlText, " ");
            testText = WhitespaceReplace.Replace(testText, " ");
        }

        var isRegexCompare = GetIsRegexComparison(controlTextNode);

        return isRegexCompare
            ? PerformRegexCompare(comparison, compareMethod, controlText, testText)
            : PerformStringCompare(comparison, compareMethod, controlText, testText);
    }

    private static CompareResult PerformRegexCompare(in Comparison comparison, StringComparison compareMethod, string controlText, string testText)
    {
        var regexOptions = compareMethod == StringComparison.OrdinalIgnoreCase
            ? RegexOptions.IgnoreCase
            : RegexOptions.None;

        return Regex.IsMatch(testText, controlText, regexOptions, TimeSpan.FromSeconds(5))
            ? CompareResult.Same
            : CompareResult.FromDiff(new TextDiff(comparison));
    }

    private static CompareResult PerformStringCompare(in Comparison comparison, StringComparison compareMethod, string controlText, string testText)
    {
        return controlText.Equals(testText, compareMethod)
            ? CompareResult.Same
            : CompareResult.FromDiff(new TextDiff(comparison));
    }

    private static bool GetIsRegexComparison(IText controlTextNode)
    {
        var parent = controlTextNode.ParentElement;
        return parent is not null && parent.TryGetAttrValue(REGEX_ATTR_NAME, out bool isRegex) && isRegex;
    }

    private WhitespaceOption GetWhitespaceOption(IText textNode)
    {
        var parent = textNode.ParentElement ?? throw new UnexpectedDOMTreeStructureException();
        foreach (var tagName in DefaultPreserveElement)
        {
            if (parent.NodeName.Equals(tagName, StringComparison.Ordinal))
            {
                return parent.TryGetAttrValue(WHITESPACE_ATTR_NAME, out WhitespaceOption option)
                    ? option
                    : WhitespaceOption.Preserve;
            }
        }
        return parent.GetInlineOptionOrDefault<WhitespaceOption>(WHITESPACE_ATTR_NAME, Whitespace);
    }

    private StringComparison GetCompareMethod(IText controlTextNode)
    {
        var parent = controlTextNode.ParentElement ?? throw new UnexpectedDOMTreeStructureException();
        return parent.GetInlineOptionOrDefault(IGNORECASE_ATTR_NAME, IgnoreCase)
            ? StringComparison.OrdinalIgnoreCase
            : StringComparison.Ordinal;
    }
}
