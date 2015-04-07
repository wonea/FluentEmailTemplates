using System.Xml;

namespace VisualProduct.FluentEmailTemplates.Parts
{
    public class ValuePart : StringPartBase
    {
        private string _value;

        public ValuePart()
            : base("value")
        {
        }

        public override string GetString(MergeData mergeData)
        {
            var value = Merge(_value, mergeData, false);
            return value;
        }

        protected override void Load(XmlReader xmlReader)
        {
            // Expecting:
            // <value>some string</value>
            _value = xmlReader.ReadInnerXml();
        }
    }
}