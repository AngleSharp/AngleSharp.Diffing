var target = Argument("target", "Default");
var projectName = "AngleSharp.Diffing";
var solutionName = "AngleSharp.Diffing";
var frameworks = new Dictionary<String, String>
{
    { "netstandard2.0", "netstandard2.0" },
};

#load tools/anglesharp.cake

RunTarget(target);