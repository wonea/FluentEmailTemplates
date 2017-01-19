using System.Collections.Generic;
using System.Xml;

namespace VisualProduct.FluentEmailTemplates.Parts
{
    public class ContainerPart : HtmlPartBase
    {
        /// <summary>
        /// Containers are not renderOuterDived by default.
        /// To renderOuterDiv a container use the attribute renderOuterDiv="true"
        /// </summary>
        private readonly IPartSelector _partSelector;
        private readonly IList<IHtmlPart> _parts = new List<IHtmlPart>();

        public ContainerPart(IPartSelector partSelector)
            : base("container")
        {
            _partSelector = partSelector;
        }

        protected override void Load(XmlReader xmlReader)
        {
            // Expecting container with zero or more children.
            // <container>
            //     <value>something</value>
            //     <value>something else</value>
            // </container>
            StoreAttributes(xmlReader, "renderOuterDiv", "style");

            xmlReader.MoveToElement();
            xmlReader.Read();
            while (!xmlReader.EOF)
            {
                if (xmlReader.NodeType == XmlNodeType.Element)
                {
                    var part = _partSelector.GetHtmlPart(xmlReader.LocalName);
                    var xml = xmlReader.ReadOuterXml();
                    part.LoadXml(xml);
                    _parts.Add(part);
                }
                else
                {
                    xmlReader.Read();
                }
            }
        }

        protected override void WriteHtml(XmlWriter xmlWriter, MergeData mergeData)
        {
            // Don't renderOuterDiv the div if we explicitly set renderOuterDiv="false".
            var renderOuterDivOuterDiv = !StoredAttributes.ContainsKey("renderOuterDiv") || StoredAttributes["renderOuterDiv"] == "true";

            if (renderOuterDivOuterDiv)
            {
                xmlWriter.WriteStartElement("div");
                WriteStoredAttributes(xmlWriter, mergeData, "renderOuterDiv");
            }

            foreach (var part in _parts)
            {
                var html = part.GetHtml(mergeData);
                xmlWriter.WriteRaw(html);
            }

            if (renderOuterDivOuterDiv)
            {
                xmlWriter.WriteEndElement();
            }
        }
    }
}