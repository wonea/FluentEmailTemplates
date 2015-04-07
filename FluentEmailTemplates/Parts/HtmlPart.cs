using System.Xml;

namespace VisualProduct.FluentEmailTemplates.Parts
{
    public class HtmlPart : HtmlPartBase
    {
        private string _html;

        public HtmlPart()
            : base("html")
        {
        }

        protected override void Load(XmlReader xmlReader)
        {
            xmlReader.MoveToElement();
            _html = xmlReader.ReadInnerXml();
        }

        protected override void WriteHtml(XmlWriter xmlWriter, MergeData mergeData)
        {
            var html = Merge(_html, mergeData, true);
            xmlWriter.WriteRaw(html);
        }
    }
}