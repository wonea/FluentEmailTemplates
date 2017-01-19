using System;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Xml.XPath;
using VisualProduct.FluentEmailTemplates.Parts;

namespace VisualProduct.FluentEmailTemplates
{
    public class EmailTemplate : IEmailTemplate
    {
        private readonly IPartSelector _partSelector;
        private string _xml;

        public EmailTemplate(IPartSelector partSelector)
        {
            _partSelector = partSelector;
        }

        /// <summary>
        /// Load an email template from a file.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        public void Load(string filePath)
        {
            _xml = File.ReadAllText(filePath);
            LoadXml(_xml);
        }

        /// <summary>
        /// Load an email template from an xml string.
        /// </summary>
        /// <param name="xml">The xml string.</param>
        public void LoadXml(string xml)
        {
            // The email template xml must look similar to this:
            // <emailTemplate xmlns="http://visualproduct.com/emailtemplate.xsd">
            //     <subject>
            //         <value>My subject</value>
            //     </subject>
            //     <htmlBody>
            //         The html body.
            //     </htmlBody>
            // </emailTemplate>

            // Cannot have empty xml.
            if (string.IsNullOrWhiteSpace(xml))
            {
                throw new ArgumentNullException("xml", "Invalid xml.");
            }

            // Crude and basic check that the xml namespace is present.
            // If it's not present the schema validation passes because it doesn't fit the schema.
            if (!xml.Contains(@"<emailTemplate"))
            {
                throw new ArgumentException("Missing emailTemplate element.", "xml");
            }

            // Crude and basic check that the xml namespace is present.
            // If it's not present the schema validation passes because it doesn't fit the schema.
            if (!xml.Contains(@"<emailTemplate xmlns=""http://visualproduct.com/emailtemplate.xsd"""))
            {
                throw new ArgumentException(@"emailTemplate node must contain the xmlns attribute value ""http://visualproduct.com/emailtemplate.xsd"".", "xml");
            }

            using (var stringReader = new StringReader(xml))
            {
                using (var xmlReader = XmlReader.Create(stringReader))
                {
                    var doc = new XPathDocument(xmlReader, XmlSpace.Preserve);
                    var nav = doc.CreateNavigator();

                    var schemaSet = GetXmlSchemaSet();

                    // Validate the xml.
                    var validation = new ValidationEventHandler(SchemaValidationHandler);
                    nav.CheckValidity(schemaSet, validation);

                    // Namespace manager.
                    var manager = new XmlNamespaceManager(nav.NameTable);
                    manager.AddNamespace("et", "http://visualproduct.com/emailtemplate.xsd");

                    // Set and load the subject.
                    SetAndLoadSubject(nav, manager);

                    // Set and load the html body.
                    SetAndLoadHtmlBody(nav, manager);
                }
            }
        }

        /// <summary>
        /// Set and load the HtmlBody property.
        /// </summary>
        private void SetAndLoadHtmlBody(XPathNavigator nav, XmlNamespaceManager manager)
        {
            // Get the subject's first child navigator.
            var htmlBodyNav = nav.SelectSingleNode("/et:emailTemplate/et:htmlBody", manager);

            // Get the html part for the body as a non renderOuterDived container.
            HtmlBody = _partSelector.GetHtmlPart("container");

            var xml = @"<container renderOuterDiv=""false"">" + htmlBodyNav?.InnerXml + "</container>";

            // Load the string part.
            HtmlBody.LoadXml(xml);
        }

        /// <summary>
        /// Set and load the Subject property.
        /// </summary>
        private void SetAndLoadSubject(XPathNavigator nav, XmlNamespaceManager manager)
        {
            // Get the subject's first child navigator.
            var subjectNav = nav.SelectSingleNode("/et:emailTemplate/et:subject/*[1]", manager);

            // Get the string part for the subject. e.g. "value".
            Subject = _partSelector.GetStringPart(subjectNav?.Name);

            // Load the string part.
            Subject.LoadXml(subjectNav?.OuterXml);
        }

        private static XmlSchemaSet GetXmlSchemaSet()
        {
            const string xml = @"<?xml version=""1.0""?>
<xs:schema id=""emailTemplate"" targetNamespace=""http://visualproduct.com/emailtemplate.xsd"" elementFormDefault=""qualified"" xmlns=""http://visualproduct.com/emailtemplate.xsd"" xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
    
    <xs:element name=""emailTemplate"">
        <xs:complexType>
            <xs:sequence>
                <xs:element name=""subject"" />
                <xs:element name=""htmlBody"" />
            </xs:sequence>
        </xs:complexType>
    </xs:element>
</xs:schema>";

            XmlSchema schema;
            using (var stringReader = new StringReader(xml))
            {
                schema = XmlSchema.Read(stringReader, SchemaValidationHandler);
            }

            var schemaSet = new XmlSchemaSet();
            schemaSet.Add(schema);
            return schemaSet;
        }

        private static void SchemaValidationHandler(object sender, ValidationEventArgs args)
        {
            // Always throw when there's an error or warning. Don't care about the severity.
            var message = string.Format("Schema Validation Error: {0}", args.Message);
            throw new XmlSchemaException(message);
        }

        public IStringPart Subject { get; private set; }
        public IHtmlPart HtmlBody { get; private set; }
    }
}