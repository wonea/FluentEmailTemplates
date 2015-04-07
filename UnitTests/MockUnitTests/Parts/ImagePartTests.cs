using System.Collections.Generic;
using NUnit.Framework;
using VisualProduct.FluentEmailTemplates.Parts;

namespace VisualProduct.FluentEmailTemplates.UnitTests.MockUnitTests.Parts
{
    public class ImagePartTests
    {
        private ImagePart _imagePart;

        /// <Summary>
        /// Setup the unit test.
        /// </Summary>
        [SetUp]
        public void SetUp()
        {
            _imagePart = new ImagePart();
        }

        [TestCaseSource("GetHtml_TestCaseSource")]
        public void GetHtml_Is_Successful(string xml, MergeData mergeData, string expected)
        {
            //
            // Arrange.
            //
            _imagePart.LoadXml(xml);

            //
            // Act.
            //
            var result = _imagePart.GetHtml(mergeData);

            //
            // Assert.
            //
            Assert.That(result, Is.EqualTo(expected));
        }

        public IEnumerable<object[]> GetHtml_TestCaseSource()
        {
            yield return new object[]
            {
                @"<image src=""http://example.com/images/test.png"" />",
                null,
                @"<img src=""http://example.com/images/test.png"" />"
            };

            yield return new object[]
            {
                @"<image src=""http://example.com/images/test.png"" alt=""Test"" title=""My title"" style=""width:10px;"" unknown=""not rendered"" />",
                null,
                @"<img alt=""Test"" src=""http://example.com/images/test.png"" style=""width:10px;"" title=""My title"" />"
            };

            yield return new object[]
            {
                @"<image src=""*|TestImageUrl|*"" alt=""*|TestImageAlt|*"" title=""*|TestImageTitle|*"" style=""*|TestImageStyle|*"" />",
                new MergeData()
                    .Add("TestImageUrl", "http://example.com/images/test.png")
                    .Add("TestImageAlt", "The alt & text")
                    .Add("TestImageTitle", "The & title")
                    .Add("TestImageStyle", "width:99px;"),
                @"<img alt=""The alt &amp; text"" src=""http://example.com/images/test.png"" style=""width:99px;"" title=""The &amp; title"" />"
            };
        }
    }
}