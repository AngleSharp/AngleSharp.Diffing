namespace AngleSharp.Diffing.Core;

/// <summary>
/// The diffing context provides access to the control and test DOM tree during diffing.
/// </summary>
public interface IDiffContext
{
    /// <summary>
    /// Query the control nodes using a CSS selector.
    /// </summary>
    /// <param name="cssSelector">The CSS selector to use in the query.</param>
    /// <returns>A <see cref="IHtmlCollection{IElement}"/></returns>
    IHtmlCollection<IElement> QueryControlNodes(string cssSelector);

    /// <summary>
    /// Query the test nodes using a CSS selector.
    /// </summary>
    /// <param name="cssSelector">The CSS selector to use in the query.</param>
    /// <returns>A <see cref="IHtmlCollection{IElement}"/></returns>
    IHtmlCollection<IElement> QueryTestNodes(string cssSelector);
}