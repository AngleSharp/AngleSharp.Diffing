using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp.Dom;
using Egil.AngleSharp.Diffing.Core;

namespace Egil.AngleSharp.Diffing.Strategies.TextNodeStrategies
{
    public class TextNodeFilter
    {
        public WhitespaceOption Option { get; }

        public TextNodeFilter(WhitespaceOption option)
        {
            Option = option;
        }

        public FilterDecision Filter(in ComparisonSource source, FilterDecision currentDecision)
        {
            if (currentDecision.IsExclude() || Option == WhitespaceOption.Preserve) return currentDecision;

            if (source.Node is IText text && string.IsNullOrWhiteSpace(text.Data)) return FilterDecision.Exclude;

            return currentDecision;
        }
    }
}
