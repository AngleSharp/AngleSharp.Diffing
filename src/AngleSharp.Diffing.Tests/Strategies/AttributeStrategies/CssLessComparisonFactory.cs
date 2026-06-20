using AngleSharp.Html.Parser;

namespace AngleSharp.Diffing.Strategies.AttributeStrategies;

/// <summary>
/// Builds style-attribute comparisons whose elements are parsed in a browsing context without CSS support.
/// With no <see cref="AngleSharp.Css.Parser.ICssParser"/> registered, <c>IElement.GetStyle()</c> returns
/// <c>null</c> — the situation CSS-less consumers such as bUnit hit when diffing markup that contains inline
/// SVG. The shared <see cref="DiffingTestFixture"/> enables CSS, so it cannot reproduce this on its own.
/// </summary>
internal static class CssLessComparisonFactory
{
    public static AttributeComparison ToStyleAttributeComparison(string controlHtml, string testHtml)
    {
        // Same parser setup as DiffingTestFixture, but deliberately without .WithCss().
        var config = Configuration.Default
            .With<IHtmlParser>(_ =>
                new HtmlParser(
                    new()
                    {
                        IsKeepingSourceReferences = true
                    },
                    _));
        var context = BrowsingContext.New(config);
        var parser = context.GetService<IHtmlParser>()!;
        var document = context.OpenNewAsync().Result;

        return new AttributeComparison(
            ToStyleSource(ComparisonSourceType.Control, controlHtml),
            ToStyleSource(ComparisonSourceType.Test, testHtml));

        AttributeComparisonSource ToStyleSource(ComparisonSourceType sourceType, string html)
        {
            var element = parser.ParseFragment(html, document.Body!)[0];
            return new("style", element.ToComparisonSource(0, sourceType));
        }
    }
}
