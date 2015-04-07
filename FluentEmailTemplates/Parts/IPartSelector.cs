namespace VisualProduct.FluentEmailTemplates.Parts
{
    public interface IPartSelector
    {
        /// <summary>
        /// Get an html part by name.
        /// </summary>
        IHtmlPart GetHtmlPart(string name);

        /// <summary>
        /// Get the string part by name.
        /// </summary>
        IStringPart GetStringPart(string name);
    }
}