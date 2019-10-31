namespace System.Diagnostics.CodeAnalysis
{
    // TODO: Figure out how to do multi-platform targeting, such that nullable types is part of the assembly for folks using .net core 3.
    //       This is added since it is not available in .net standard 2.0
    [AttributeUsage(AttributeTargets.Parameter, Inherited = false)]
    public sealed class NotNullWhenAttribute : Attribute
    {
        public bool ReturnValue { get; }

        public NotNullWhenAttribute(bool returnValue)
        {
            ReturnValue = returnValue;
        }
    }
}
