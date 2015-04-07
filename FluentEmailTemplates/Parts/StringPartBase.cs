using System.Web;
using System.Xml;

namespace VisualProduct.FluentEmailTemplates.Parts
{
    /// <summary>
    /// Base class for string parts. Derives from HtmlPartBase as
    /// the string parts can be used with html parts, so GetHtml(...)
    /// must be implemented.
    /// </summary>
    public abstract class StringPartBase : HtmlPartBase, IStringPart
    {
        protected StringPartBase(string localName) 
            : base(localName)
        {
        }

        public abstract string GetString(MergeData mergeData);

        /// <summary>
        /// Get the html.
        /// </summary>
        public override string GetHtml(MergeData mergeData)
        {
            // Same as GetString() but html encoded.
            var value = GetString(mergeData);
            var html = HttpUtility.HtmlEncode(value);
            return html;
        }
    }
}