﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Css.Dom;
using AngleSharp.Css.Parser;
using AngleSharp.Dom;
using Egil.AngleSharp.Diffing.Core;

namespace Egil.AngleSharp.Diffing.Strategies.TextNodeStrategies
{
    public static class StyleSheetTextNodeComparer
    {
        public static CompareResult Compare(in Comparison comparison, CompareResult currentDecision)
        {
            if (currentDecision.IsDecisionFinal()) return currentDecision;
            if (TryGetStyleDeclaretions(comparison, out var controlStyles, out var testStyles))
                return Compare(controlStyles, testStyles);
            else
                return currentDecision;
        }

        private static bool TryGetStyleDeclaretions(in Comparison comparison, [NotNullWhen(true)]out IStyleSheet? controlStyles, [NotNullWhen(true)]out IStyleSheet? testStyles)
        {
            controlStyles = default;
            testStyles = default;

            if (comparison.TryGetNodesAsType<IText>(out var controlTextNode, out var testTextNode))
            {
                var controlParentStyle = controlTextNode.ParentElement as ILinkStyle;
                var testParentStyle = testTextNode.ParentElement as ILinkStyle;
                controlStyles = controlParentStyle?.Sheet;
                testStyles = testParentStyle?.Sheet;

                return controlStyles is { } && testStyles is { };
            }
            else
                return false;
        }

        private static CompareResult Compare(IStyleSheet controlStyles, IStyleSheet testStyles)
        {
            var control = controlStyles.ToCss();
            var test = testStyles.ToCss();

            return control.Equals(test, StringComparison.Ordinal)
                ? CompareResult.Same
                : CompareResult.Different;
        }
    }
}