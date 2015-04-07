using Moq;
using NUnit.Framework;
using VisualProduct.FluentEmailTemplates.Parts;

namespace VisualProduct.FluentEmailTemplates.UnitTests.MockUnitTests.Parts
{
    [TestFixture]
    public class TemplateFilePartTests : TestBase
    {
        private Mock<IHtmlPart> _mockHtmlPart;
        private Mock<IPartSelector> _mockPartSelector;
        private TemplateFilePart _templateFilePart;

        /// <Summary>
        /// Setup the unit test.
        /// </Summary>
        [SetUp]
        public void SetUp()
        {
            _mockHtmlPart = new Mock<IHtmlPart>();
            _mockHtmlPart.Setup(o => o.GetHtml(It.IsAny<MergeData>())).Returns("<span>testing</span>");

            _mockPartSelector = new Mock<IPartSelector>();
            _mockPartSelector.Setup(o => o.GetHtmlPart(It.IsAny<string>())).Returns(_mockHtmlPart.Object);

            _templateFilePart = new TemplateFilePart(
                _mockPartSelector.Object);
        }

        [Test]
        public void LoadXml_And_GetHtml_With_FilePath_Is_Successful()
        {
            //
            // Arrange.
            //
            var filePath = GetProjectRelativeFilePath(@"Files\Parts\TemplateFilePart_001.html");
            var xml = string.Format(@"<templateFile filePath=""{0}"" />", filePath);

            //
            // Act.
            //
            _templateFilePart.LoadXml(xml);
            var result = _templateFilePart.GetHtml(null);

            //
            // Assert.
            //
            Assert.That(result, Is.EqualTo("<span>testing</span>"));
        }

        [Test]
        public void LoadXml_And_GetHtml_With_RelativePath_Is_Successful()
        {
            //
            // Arrange.
            //
            const string xml = @"<templateFile relativePath=""Parts\TemplateFilePart_001.html"" />";

            //
            // Act.
            //
            _templateFilePart.LoadXml(xml);

            //
            // Assert.
            //
            var result = _templateFilePart.GetHtml(null);
            Assert.That(result, Is.EqualTo("<span>testing</span>"));
        }

        [Test]
        public void LoadXml_And_GetHtml_With_FilePath_And_MergeData_Is_Successful()
        {
            //
            // Arrange.
            //
            var filePath = GetProjectRelativeFilePath(@"Files\Parts\TemplateFilePart_001.html");
            var xml = string.Format(@"<templateFile filePath=""{0}"" />", filePath);
            var mergeData = new MergeData()
                .Add("FirstName", "John");

            //
            // Act.
            //
            _templateFilePart.LoadXml(xml);

            //
            // Assert.
            //
            var result = _templateFilePart.GetHtml(mergeData);
            Assert.That(result, Is.EqualTo("<span>testing</span>"));
        }

        [Test]
        public void LoadXml_And_GetHtml_With_RelativePath_And_MergeData_Is_Successful()
        {
            //
            // Arrange.
            //
            const string xml = @"<templateFile relativePath=""Parts\TemplateFilePart_001.html"" />";
            var mergeData = new MergeData()
                .Add("FirstName", "John");

            //
            // Act.
            //
            _templateFilePart.LoadXml(xml);

            //
            // Assert.
            //
            var result = _templateFilePart.GetHtml(mergeData);
            Assert.That(result, Is.EqualTo("<span>testing</span>"));
        }
    }
}