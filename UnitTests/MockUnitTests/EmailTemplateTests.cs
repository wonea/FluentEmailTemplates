using System;
using System.Xml.Schema;
using Moq;
using NUnit.Framework;
using VisualProduct.FluentEmailTemplates.Parts;

namespace VisualProduct.FluentEmailTemplates.UnitTests.MockUnitTests
{
    [TestFixture]
    public class EmailTemplateTests : TestBase
    {
        private Mock<IPartSelector> _mockPartSelector;
        private EmailTemplate _emailTemplate;
        private Mock<IStringPart> _mockStringPart;
        private Mock<IHtmlPart> _mockHtmlPart;

        /// <Summary>
        /// Setup the unit test.
        /// </Summary>
        [SetUp]
        public void SetUp()
        {
            _mockStringPart = new Mock<IStringPart>();
            _mockHtmlPart = new Mock<IHtmlPart>();

            _mockPartSelector = new Mock<IPartSelector>();
            _mockPartSelector.Setup(o => o.GetStringPart(It.IsAny<string>())).Returns(_mockStringPart.Object);
            _mockPartSelector.Setup(o => o.GetHtmlPart(It.IsAny<string>())).Returns(_mockHtmlPart.Object);

            _emailTemplate = new EmailTemplate(
                 _mockPartSelector.Object);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("    ")]
        [TestCase("  \r\n\t  ")]
        public void LoadXml_With_Null_Or_Missing_Xml_Throws_Exception(string xml)
        {
            //
            // Act.
            //
            var ex = Assert.Throws<ArgumentNullException>(() => _emailTemplate.LoadXml(xml));

            //
            // Assert.
            //
            Assert.That(ex.Message, Is.EqualTo("Invalid xml.\r\nParameter name: xml"));
        }

        [Test]
        public void LoadXml_With_Missing_Xml_Namespace_Throws_Exception()
        {
            //
            // Arrange.
            //
            var xml = ReadAllTextFromProjectRelativeFilePath(@"Files\EmailTemplates\EmailTemplate_001.xml");

            //
            // Act.
            //
            var ex = Assert.Throws<ArgumentException>(() => _emailTemplate.LoadXml(xml));

            //
            // Assert.
            //
            Assert.That(ex.Message, Is.EqualTo("emailTemplate node must contain the xmlns attribute value \"http://visualproduct.com/emailtemplate.xsd\".\r\nParameter name: xml"));
        }

        [Test]
        public void LoadXml_With_Missing_EmailTemplate_Element_Throws_Exception()
        {
            //
            // Arrange.
            //
            var xml = ReadAllTextFromProjectRelativeFilePath(@"Files\EmailTemplates\EmailTemplate_002.xml");

            //
            // Act.
            //
            var ex = Assert.Throws<ArgumentException>(() => _emailTemplate.LoadXml(xml));

            //
            // Assert.
            //
            Assert.That(ex.Message, Is.EqualTo("Missing emailTemplate element.\r\nParameter name: xml"));
        }

        [Test]
        public void LoadXml_With_Invalid_Schema_Throws_Exception()
        {
            //
            // Arrange.
            //
            var xml = ReadAllTextFromProjectRelativeFilePath(@"Files\EmailTemplates\EmailTemplate_003.xml");

            //
            // Act.
            //
            var ex = Assert.Throws<XmlSchemaException>(() => _emailTemplate.LoadXml(xml));

            //
            // Assert.
            //
            Assert.That(ex.Message, Is.StringContaining("invalid child element 'subjectInvalid'"));
        }

        [Test]
        public void LoadXml_With_Schema_Is_Successful()
        {
            //
            // Arrange.
            //
            var xml = ReadAllTextFromProjectRelativeFilePath(@"Files\EmailTemplates\EmailTemplate_004.xml");

            //
            // Act.
            //
            _emailTemplate.LoadXml(xml);

            //
            // Assert.
            //
            _mockPartSelector.Verify(o => o.GetStringPart(It.IsAny<string>()), Times.Once);
            _mockPartSelector.Verify(o => o.GetHtmlPart(It.IsAny<string>()), Times.Once);
        }
    }
}