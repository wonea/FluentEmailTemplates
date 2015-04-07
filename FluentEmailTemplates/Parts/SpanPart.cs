using System.Xml;

namespace VisualProduct.FluentEmailTemplates.Parts
{
    public class SpanPart : HtmlPartBase
    {
        private string _value;

        public SpanPart()
            : base("span")
        {
        }

        protected override void Load(XmlReader xmlReader)
        {
            // Expecting (with optional style attribute):
            // <span style="color:red;">some string</span>
            StoreAttributes(xmlReader, "style");
            xmlReader.MoveToElement();
            _value = xmlReader.ReadInnerXml();
        }

        protected override void WriteHtml(XmlWriter xmlWriter, MergeData mergeData)
        {
            xmlWriter.WriteStartElement("span");
            WriteStoredAttributes(xmlWriter, mergeData);
            var value = Merge(_value, mergeData);
            xmlWriter.WriteValue(value);
            xmlWriter.WriteEndElement();
        }
    }
}