namespace VisualProduct.FluentEmailTemplates.Parts
{
    /// <summary>
    /// An html file part is a reference to an html file on disk.
    /// </summary>
    public class HtmlFilePart : FilePartBase
    {
        public HtmlFilePart()
            : base("htmlFile")
        {
        }
    }
}