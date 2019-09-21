using System;
using System.Collections.Generic;
using System.Linq;
using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using Shouldly;
using Xunit;

namespace Egil.AngleSharp.Diffing
{
  internal abstract class AngleSharpDiffingTestBase 
  {
    private readonly IBrowsingContext _context = BrowsingContext.New();
    private readonly IHtmlParser _htmlParser;
    private readonly IDocument _document;

    protected INodeList EmptyNodeList => ToNodeList("");

    protected DifferenceEngineTest()
    {
      _htmlParser = _context.GetService<IHtmlParser>();
      _document = _context.OpenNewAsync().Result;
    }

    protected INodeList ToNodeList(string htmlsnippet)
    {
      var fragment = _htmlParser.ParseFragment(htmlsnippet, _document.Body);
      return fragment;
    }
  }

  public class DifferenceEngineTest2 : AngleSharpDiffingTestBase
  {
    [Fact]
    public void EngineUsesProvidedHooksToDriveComparison() 
    {
        var sut = new DifferenceEngine();
    }
  }
}