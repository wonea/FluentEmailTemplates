using System.Web;
using System.Xml;

namespace VisualProduct.FluentEmailTemplates.Parts
{
    public class MergePart : StringPartBase
    {
        public MergePart()
            : base("merge")
        {
        }

        public override string GetHtml(MergeData mergeData)
        {
            // With no merge data there's no html.
            if (mergeData == null || mergeData.Count == 0)
            {
                return null;
            }

            // Same as GetString(...) but html encoded and replace LF with <br />.
            var value = GetString(mergeData);

            if (string.IsNullOrEmpty(value))
            {
                return null;
            }

            var html = HttpUtility.HtmlEncode(value);

            // Replace LF with <br />.
            html = html.Replace("\n", "\n<br />");

            return html;
        }

        /// <summary>
        /// The string is the key attribute's value.
        /// </summary>
        public override string GetString(MergeData mergeData)
        {
            // With no merge data there's no string.
            if (mergeData == null || mergeData.Count == 0)
            {
                return null;
            }

            var key = StoredAttributes["key"];
            var value = mergeData.GetValue(key);
            return value;
        }

        protected override void Load(XmlReader xmlReader)
        {
            // Expecting:
            // <merge key="FirstName" />
            StoreAttributes(xmlReader, "key");
        }
    }
}