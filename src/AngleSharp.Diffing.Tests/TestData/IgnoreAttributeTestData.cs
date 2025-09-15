namespace AngleSharp.Diffing.TestData;

internal static class IgnoreAttributeTestData
{
    public static TheoryData<string, string> ControlAndHtmlData()
    {
        var theoryData = new TheoryData<string, string>();
        foreach (var (controlHtml, expectedHtml, _) in TestCases)
        {
            theoryData.Add(controlHtml, expectedHtml);
        }

        return theoryData;
    }

    public static TheoryData<string, string, DiffResult> ControlHtmlAndDiffData()
    {
        var theoryData = new TheoryData<string, string, DiffResult>();
        foreach (var (controlHtml, expectedHtml, expectedDiffResult) in TestCases)
        {
            theoryData.Add(controlHtml, expectedHtml, expectedDiffResult);
        }

        return theoryData;
    }

    private static readonly IEnumerable<(string controlHtml, string expectedHtml, DiffResult expectedDiffResult)>
        TestCases =
        [
            ("<div class:ignore></div>", "<div class=\"ian-fleming\"></div>", DiffResult.Different),
            ("<div class:ignore></div>", "<div class=\"\"></div>", DiffResult.Different),
            ("<div class:ignore></div>", "<div class></div>", DiffResult.Different),
            ("<div class:ignore></div>", "<div></div>", DiffResult.Missing),
            ("<input required:ignore/>", "<input required=\"required\"/>", DiffResult.Different),
            ("<input required:ignore/>", "<input required=\"\"/>", DiffResult.Different),
            ("<input required:ignore/>", "<input required/>", DiffResult.Different),
            ("<input required:ignore/>", "<input/>", DiffResult.Missing),
            ("<button onclick:ignore/></button>", "<button onclick=\"alert(1)\"></button>", DiffResult.Different),
            ("<button onclick:ignore/></button>", "<button/></button>", DiffResult.Missing),
            ("<a aria-disabled:ignore/></a>", "<a aria-disabled=\"true\"/></a>", DiffResult.Different),
            ("<a aria-disabled:ignore/></a>", "<a/></a>", DiffResult.Missing),
            ("<span style:ignore/></span>", "<span style=\"color:red;\"/></span>", DiffResult.Different),
            ("<span style:ignore/></span>", "<span/></span>", DiffResult.Missing),
        ];
}