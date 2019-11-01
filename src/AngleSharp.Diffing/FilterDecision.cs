namespace AngleSharp.Diffing
{
    public enum FilterDecision
    {
        Keep = 0,
        Exclude
    }

    public static class FilterDecisionExtensions
    {
        public static bool IsExclude(this FilterDecision decision) => decision == FilterDecision.Exclude;
        public static bool IsKeep(this FilterDecision decision) => decision == FilterDecision.Keep;
    }
}
