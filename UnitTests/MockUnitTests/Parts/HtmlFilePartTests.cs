using NUnit.Framework;
using VisualProduct.FluentEmailTemplates.Parts;

namespace VisualProduct.FluentEmailTemplates.UnitTests.MockUnitTests.Parts
{
    [TestFixture]
    public class HtmlFilePartTests : TestBase
    {
        private HtmlFilePart _htmlFilePart;

        /// <Summary>
        /// Setup the unit test.
        /// </Summary>
        [SetUp]
        public void SetUp()
        {
            _htmlFilePart = new HtmlFilePart();
        }

        [Test]
        public void LoadXml_With_FilePath_Is_Successful()
        {
            //
            // Arrange.
            //
            var filePath = GetProjectRelativeFilePath(@"Files\Parts\HtmlFilePart_001.html");
            var xml = string.Format(@"<htmlFile filePath=""{0}"" />", filePath);

            //
            // Act.
            //
            _htmlFilePart.LoadXml(xml);

            //
            // Assert.
            //
            var result = _htmlFilePart.GetHtml(null);
            AssertExpectedFileContents(result, @"Files\Parts\HtmlFilePart_001_Expected.html");
        }

        [Test]
        public void LoadXml_With_RelativePath_Is_Successful()
        {
            //
            // Arrange.
            //
            const string xml = @"<htmlFile relativePath=""Parts\HtmlFilePart_001.html"" />";

            //
            // Act.
            //
            _htmlFilePart.LoadXml(xml);

            //
            // Assert.
            //
            var result = _htmlFilePart.GetHtml(null);
            AssertExpectedFileContents(result, @"Files\Parts\HtmlFilePart_001_Expected.html");
        }

        [Test]
        public void LoadXml_With_FilePath_And_MergeData_Is_Successful()
        {
            //
            // Arrange.
            //
            var filePath = GetProjectRelativeFilePath(@"Files\Parts\HtmlFilePart_002.html");
            var xml = string.Format(@"<htmlFile filePath=""{0}"" />", filePath);
            var mergeData = new MergeData()
                .Add("FirstName", "John");

            //
            // Act.
            //
            _htmlFilePart.LoadXml(xml);

            //
            // Assert.
            //
            var result = _htmlFilePart.GetHtml(mergeData);
            AssertExpectedFileContents(result, @"Files\Parts\HtmlFilePart_002_Expected.html");
        }

        [Test]
        public void LoadXml_With_RelativePath_And_MergeData_Is_Successful()
        {
            //
            // Arrange.
            //
            const string xml = @"<htmlFile relativePath=""Parts\HtmlFilePart_002.html"" />";
            var mergeData = new MergeData()
                .Add("FirstName", "John");

            //
            // Act.
            //
            _htmlFilePart.LoadXml(xml);

            //
            // Assert.
            //
            var result = _htmlFilePart.GetHtml(mergeData);
            AssertExpectedFileContents(result, @"Files\Parts\HtmlFilePart_002_Expected.html");
        }
    }
}