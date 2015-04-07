namespace VisualProduct.FluentEmailTemplates.Parts
{
    public interface IPart
    {
        /// <summary>
        /// Load the part from an xml string.
        /// </summary>
        /// <param name="xml">The xml to load.</param>
        void LoadXml(string xml);
    }
}