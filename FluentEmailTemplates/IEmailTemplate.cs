using VisualProduct.FluentEmailTemplates.Parts;

namespace VisualProduct.FluentEmailTemplates
{
    public interface IEmailTemplate
    {
        /// <summary>
        /// Load an email template from a file.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        void Load(string filePath);

        /// <summary>
        /// Load an email template from an xml string.
        /// </summary>
        /// <param name="xml">The xml string.</param>
        void LoadXml(string xml);

        /// <summary>
        /// The email template subject part.
        /// </summary>
        IStringPart Subject { get; }

        /// <summary>
        /// The email template body html part.
        /// </summary>
        IHtmlPart HtmlBody { get; }
    }
}