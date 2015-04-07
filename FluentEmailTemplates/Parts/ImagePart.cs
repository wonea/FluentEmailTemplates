using System.Xml;

namespace VisualProduct.FluentEmailTemplates.Parts
{
    public class ImagePart : HtmlPartBase
    {
        public ImagePart()
            : base("image")
        {
        }

        protected override void Load(XmlReader xmlReader)
        {
            // Expecting (with optional style attribute):
            // <image src="/somewhere.png" alt="" style="color:red;" title="my title" />
            StoreAttributes(xmlReader, "alt", "src", "style", "title");
            xmlReader.MoveToElement();
        }

        protected override void WriteHtml(XmlWriter xmlWriter, MergeData mergeData)
        {
            xmlWriter.WriteStartElement("img");
            WriteStoredAttributes(xmlWriter, mergeData);
            xmlWriter.WriteEndElement();
        }
    }
}