namespace AngleSharp.Diffing.Core
{
    public interface IDiff
    {
        /// <summary>
        /// Gets the difference type, i.e. <see cref="DiffResult.Different"/>, <see cref="DiffResult.Missing"/>, or <see cref="DiffResult.Unexpected"/>.
        /// </summary>
        DiffResult Result { get; }

        /// <summary>
        /// Gets the target type in that failed the comparison, e.g. <see cref="DiffTarget.Element"/> or <see cref="DiffTarget.Attribute"/>.
        /// </summary>
        DiffTarget Target { get; }
    }
}
