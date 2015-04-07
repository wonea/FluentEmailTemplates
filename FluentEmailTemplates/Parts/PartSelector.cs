using System;

namespace VisualProduct.FluentEmailTemplates.Parts
{
    public class PartSelector : IPartSelector
    {
        /// <summary>
        /// Get an html part by name.
        /// </summary>
        public virtual IHtmlPart GetHtmlPart(string name)
        {
            switch (name)
            {
                case "container":
                    return new ContainerPart(this);
                case "html":
                    return new HtmlPart();
                case "htmlFile":
                    return new HtmlFilePart();
                case "image":
                    return new ImagePart();
                case "merge":
                    return new MergePart();
                case "span":
                    return new SpanPart();
                case "templateFile":
                    return new TemplateFilePart(this);
                case "value":
                    return new ValuePart();
                default:
                    throw new NotSupportedException(@"Unknown html part """ + name + @""".");
            }
        }

        /// <summary>
        /// Get the string part by name.
        /// </summary>
        public virtual IStringPart GetStringPart(string name)
        {
            switch (name)
            {
                case "merge":
                    return new MergePart();
                case "value":
                    return new ValuePart();
                default:
                    throw new NotSupportedException(@"Unknown string part """ + name + @""".");
            }
        }
    }
}