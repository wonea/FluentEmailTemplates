using System;
using System.Configuration;
using System.IO;
using System.Xml;

namespace VisualProduct.FluentEmailTemplates.Parts
{
    /// <summary>
    /// Base class for "file" type parts.
    /// </summary>
    public abstract class FilePartBase : HtmlPartBase
    {
        protected FilePartBase(string localName) 
            : base(localName)
        {
        }

        protected override void Load(XmlReader xmlReader)
        {
            // Expecting:
            // <elementName filePath="C:\Somewhere\Example.xml" />
            // or
            // <elementName relativePath="Files\Example.xml" />
            StoreAttributes(xmlReader, "filePath", "relativePath");
        }

        /// <summary>
        /// Gets the file path from "filePath" or "relativeFilePath" attribute.
        /// </summary>
        /// <returns>The full file path to a file.</returns>
        protected string GetFilePathFromStoredAttributes()
        {
            string filePath;
            if (StoredAttributes.ContainsKey("filePath"))
            {
                // Full file path is used.
                filePath = StoredAttributes["filePath"];

                if (!File.Exists(filePath))
                {
                    throw new ArgumentException(string.Format(@"Unable to find html part at file location ""{0}"" as referenced by attribute ""filePath"".", filePath));
                }
            }
            else if (StoredAttributes.ContainsKey("relativePath"))
            {
                var relativePath = StoredAttributes["relativePath"];

                // Relative path to the app setting "FluentEmailTemplatesFilePath".
                filePath = GetFilePathFromRelativePath(relativePath);

                if (!File.Exists(filePath))
                {
                    throw new ArgumentException(string.Format(@"Unable to find html part at file location ""{0}"" referenced by app setting ""FluentEmailTemplatesFilePath"" and ""relativePath"" attribute.", filePath));
                }
            }
            else
            {
                throw new ArgumentException(@"Missing ""filePath"" or ""relativePath"" attribute for html file part.");
            }

            return filePath;
        }

        /// <summary>
        /// Gets the full file path from a relative file path using the app setting
        /// "FluentEmailTemplatesFilePath"
        /// </summary>
        protected static string GetFilePathFromRelativePath(string relativePath)
        {
            var rootFilePath = ConfigurationManager.AppSettings["FluentEmailTemplatesFilePath"];
            if (string.IsNullOrEmpty(rootFilePath))
            {
                throw new ArgumentException(@"Missing app setting ""FluentEmailTemplatesFilePath"" required for ""relativePath"" attribute for html file part.");
            }

            var filePath = Path.Combine(rootFilePath, relativePath);
            return filePath;
        }

        protected override void WriteHtml(XmlWriter xmlWriter, MergeData mergeData)
        {
            var filePath = GetFilePathFromStoredAttributes();

            // Get the html from the file.
            var html = File.ReadAllText(filePath);

            // Peform a merge.
            html = Merge(html, mergeData, true);

            // Write the html to the xml writer.
            xmlWriter.WriteRaw(html);
        }
    }
}