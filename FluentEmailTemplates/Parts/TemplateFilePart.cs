using System.IO;

namespace VisualProduct.FluentEmailTemplates.Parts
{
    public class TemplateFilePart : FilePartBase
    {
        private readonly IPartSelector _partSelector;

        public TemplateFilePart(IPartSelector partSelector)
            : base("templateFile")
        {
            _partSelector = partSelector;
        }

        public override string GetHtml(MergeData mergeData)
        {
            // Get the file path to the template file.
            var filePath = GetFilePathFromStoredAttributes();

            // The template file is a container for another template.
            // Get the containter part.
            var htmlPart = _partSelector.GetHtmlPart("container");

            // Get the templage file xml from disk.
            var xml = File.ReadAllText(filePath);

            // Load the xml.
            htmlPart.LoadXml(xml);

            // Get the html from the html part and perform a merge.
            var html = htmlPart.GetHtml(mergeData);

            // Return the html.
            return html;
        }
    }
}