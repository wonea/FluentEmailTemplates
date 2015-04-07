using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;

namespace VisualProduct.FluentEmailTemplates.Parts
{
    public abstract class HtmlPartBase : IHtmlPart
    {
        private readonly string _localName;

        protected HtmlPartBase(string localName)
        {
            _localName = localName;
            StoredAttributes = new Dictionary<string, string>();
        }

        protected abstract void Load(XmlReader xmlReader);

        protected virtual void WriteHtml(XmlWriter xmlWriter, MergeData mergeData)
        {
        }
    
        public virtual string GetHtml(MergeData mergeData)
        {
            var html = Write(xmlWriter => WriteHtml(xmlWriter, mergeData));
            return html;
        }

        public virtual void LoadXml(string xml)
        {
            Read(xml, xmlReader =>
            {
                xmlReader.MoveToContent();

                if (xmlReader.LocalName != _localName)
                {
                    throw new NotSupportedException(string.Format(@"Expected xml element name ""{0}"" but was ""{1}"".", _localName, xmlReader.LocalName));
                }

                Load(xmlReader);
            });
        }

        protected static string Merge(string value, MergeData mergeData, bool htmlEncodeMergeValue = false)
        {
            if (mergeData == null)
            {
                // No merge data. The value is the result.
                return value;
            }

            var regex = new Regex(@"\*\|(?<FieldName>.+?)\|\*");
            var result = regex.Replace(value, delegate(Match m)
            {
                var mergeField = m.Groups["FieldName"].Value;

                if (!string.IsNullOrEmpty(mergeField))
                {
                    var mergeValue = mergeData.GetValue(mergeField);

                    if (mergeValue != null)
                    {
                        // Only html encode if we passed true as htmlEncodeMergeValue and the mergfield doesn't end with "Html".
                        if (htmlEncodeMergeValue && !mergeField.EndsWith("Html"))
                        {
                            mergeValue = HttpUtility.HtmlEncode(mergeValue);
                        }

                        return mergeValue;
                    }
                }

                // Didn't find it in the dictionary.
                return m.Value;
            });

            return result;
        }

        private static void Read(string xml, Action<XmlReader> readAction)
        {
            using (var stringReader = new StringReader(xml))
            {
                using (var xmlReader = XmlReader.Create(stringReader))
                {
                    readAction(xmlReader);
                }
            }
        }

        protected void StoreAttributes(XmlReader xmlReader, params string[] names)
        {
            foreach (var name in names)
            {
                if (xmlReader.MoveToAttribute(name))
                {
                    StoredAttributes.Add(name, xmlReader.ReadInnerXml());
                }
            }
        }

        private static string Write(Action<XmlWriter> writerAction)
        {
            var xmlWriterSettings = new XmlWriterSettings
            {
                Encoding = Encoding.UTF8,
                OmitXmlDeclaration = true
            };

            using (var stringWriter = new StringWriter())
            {
                using (var xmlWriter = XmlWriter.Create(stringWriter, xmlWriterSettings))
                {
                    writerAction(xmlWriter);
                }

                return stringWriter.ToString();
            }
        }

        /// <summary>
        /// Write the stored attributes to the xml writer.
        /// </summary>
        /// <param name="xmlWriter">The xml writer.</param>
        /// <param name="mergeData">The merge data to merge (if any).</param>
        /// <param name="exclusions">The attribute names to exclude.</param>
        protected void WriteStoredAttributes(XmlWriter xmlWriter, MergeData mergeData = null, params string[] exclusions)
        {
            var attributes = StoredAttributes.Where(o => !exclusions.Contains(o.Key));

            if (mergeData == null || mergeData.Count == 0)
            {
                // No merge data or keys.
                // Write out the stored attributes.
                foreach (var kvp in attributes)
                {
                    xmlWriter.WriteAttributeString(kvp.Key, kvp.Value);
                }

                return;
            }

            // Write out the stored attributes with any possible merge values.
            foreach (var kvp in attributes)
            {
                var value = Merge(kvp.Value, mergeData);
                xmlWriter.WriteAttributeString(kvp.Key, value);
            }
        }

        /// <summary>
        /// Gets the stored attributes.
        /// </summary>
        protected IDictionary<string, string> StoredAttributes { get; private set; }
    }
}