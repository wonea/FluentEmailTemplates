namespace VisualProduct.FluentEmailTemplates.Parts
{
    /// <summary>
    /// String part. Must inherit IHtmlPart to get any calls to GetHtml(...);
    /// </summary>
    public interface IStringPart : IHtmlPart
    {
        /// <summary>
        /// Get the string of the part.
        /// </summary>
        /// <param name="mergeData">The merge data.</param>
        /// <returns>The string of the part after merge.</returns>
        string GetString(MergeData mergeData);
    }
}