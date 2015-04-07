using System.Collections.Generic;
using NUnit.Framework;
using VisualProduct.FluentEmailTemplates.Parts;

namespace VisualProduct.FluentEmailTemplates.UnitTests.MockUnitTests.Parts
{
    [TestFixture]
    public class SpanPartTests
    {
        private SpanPart _spanPart;

        /// <Summary>
        /// Setup the unit test.
        /// </Summary>
        [SetUp]
        public void SetUp()
        {
            _spanPart = new SpanPart();
        }

        [TestCaseSource("GetHtml_TestCaseSource")]
        public void GetHtml_Is_Successful(string xml, MergeData mergeData, string expected)
        {
            //
            // Arrange.
            //
            _spanPart.LoadXml(xml);

            //
            // Act.
            //
            var result = _spanPart.GetHtml(mergeData);

            //
            // Assert.
            //
            Assert.That(result, Is.EqualTo(expected));
        }

        public IEnumerable<object[]> GetHtml_TestCaseSource()
        {
            // Plain boring span with no content.
            yield return new object[]
            {
                @"<span></span>",
                null,
                @"<span></span>"
            };

            // Plain boring span with no content.
            yield return new object[]
            {
                @"<span/>",
                null,
                @"<span></span>"
            };

            // Plain boring span with no content - extra space in tag.
            yield return new object[]
            {
                @"<span />",
                null,
                @"<span></span>"
            };

            // Plain boring span with some content.
            yield return new object[]
            {
                @"<span>Some text</span>",
                null,
                @"<span>Some text</span>"
            };

            // Content with a merge field.
            yield return new object[]
            {
                @"<span style=""color:red;"">*|Content|*</span>",
                new MergeData()
                    .Add("Content", "Some text"),
                @"<span style=""color:red;"">Some text</span>"
            };

            // Attribute with a merge field.
            yield return new object[]
            {
                @"<span style=""*|MyStyle|*"">Some text</span>",
                new MergeData()
                    .Add("Nada", "Not used")
                    .Add("MyStyle", "padding:40px;"),
                @"<span style=""padding:40px;"">Some text</span>"
            };

            // A bit of everything.
            yield return new object[]
            {
                @"<span style=""*|MyStyle|*"">*|Content|* *|No Merge Field Match|*</span>",
                new MergeData()
                    .Add("Nada", "Not used")
                    .Add("MyStyle", @"padding:40px;""Bert & Ernie""")
                    .Add("Content", "George & Mildred"),
                @"<span style=""padding:40px;&quot;Bert &amp; Ernie&quot;"">George &amp; Mildred *|No Merge Field Match|*</span>"
            };
        }
    }
}