namespace VisualProduct.FluentEmailTemplates.Parts
{
    public interface IHtmlPart : IPart
    {
        /// <summary>
        /// Get the html of the part.
        /// </summary>
        /// <param name="mergeData">The merge data.</param>
        /// <returns>The html of the part after merge.</returns>
        string GetHtml(MergeData mergeData);
    }
}