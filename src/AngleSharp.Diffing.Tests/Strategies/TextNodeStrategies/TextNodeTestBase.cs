using System.Globalization;

namespace AngleSharp.Diffing.Strategies.TextNodeStrategies;

public abstract class TextNodeTestBase : DiffingTestBase
{
    public static readonly char[] AllWhitespaceCharacters = new[]
{
        // SpaceSeparator category
        '\u0020', '\u00A0', '\u1680', '\u2000', '\u2001', '\u2002', '\u2003', '\u2004', '\u2005', '\u2006', '\u2007', '\u2008', '\u2009', '\u200A', '\u202F', '\u205F', '\u3000',
        // LineSeparator category
        '\u2028',
        //ParagraphSeparator category
        '\u2029',
        // CHARACTER TABULATION
        '\u0009','\u000A','\u000B','\u000C','\u000D','\u0085'
    };

    public static readonly IEnumerable<object[]> WhitespaceCharStrings = AllWhitespaceCharacters.Select(c => new string[] { c.ToString(CultureInfo.InvariantCulture) }).ToArray();

    protected TextNodeTestBase(DiffingTestFixture fixture) : base(fixture)
    {
    }
}


