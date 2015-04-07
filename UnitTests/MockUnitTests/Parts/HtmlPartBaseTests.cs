using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using VisualProduct.FluentEmailTemplates.Parts;

namespace VisualProduct.FluentEmailTemplates.UnitTests.MockUnitTests.Parts
{
    [TestFixture]
    public class HtmlPartBaseTests : TestBase
    {
        /// <summary>
        /// A class derived from the abstract class we want to test so we
        /// can easily call the protected static methods.
        /// </summary>
        public class MyHtmlPart : HtmlPartBase
        {
            public MyHtmlPart() 
                : base("html")
            {
            }

            protected override void Load(XmlReader xmlReader)
            {
                LoadCount++;
            }

            public string MergePublic(string value, MergeData mergeData, bool htmlEncodeMergeValue)
            {
                var result = Merge(value, mergeData, htmlEncodeMergeValue);
                return result;
            }

            public void WriteHtmlPublic()
            {
                WriteHtml(null, null);
            }

            public int LoadCount { get; private set; }
        }

        private static Mock<HtmlPartBase> GetMockHtmlPartBase()
        {
            return new Mock<HtmlPartBase>(
                "html")
            {
                CallBase = true
            };
        }

        [Test]
        public void WriteHtml_Is_Successful()
        {
            //
            // Arrange.
            //
            var htmlPartBase = new MyHtmlPart();

            //
            // Act & Assert.
            //
            Assert.DoesNotThrow(() => htmlPartBase.WriteHtmlPublic());
        }

        [Test]
        public void LoadXml_Is_Successful()
        {
            //
            // Arrange.
            //
            var mockHtmlPartBase = GetMockHtmlPartBase();
            var xml = ReadAllTextFromProjectRelativeFilePath(@"Files\Parts\HtmlPartBase_001.xml");

            //
            // Act.
            //
            mockHtmlPartBase.Object.LoadXml(xml);

            //
            // Assert.
            //
            mockHtmlPartBase.Protected().Verify("Load", Times.Once(), ItExpr.IsAny<XmlReader>());
        }

        [Test]
        public void Incorrect_Element_Name_Throws_Exception()
        {
            //
            // Arrange.
            //
            var mockHtmlPartBase = GetMockHtmlPartBase();
            var xml = ReadAllTextFromProjectRelativeFilePath(@"Files\Parts\HtmlPartBase_002.xml");

            //
            // Act.
            //
            var ex = Assert.Throws<NotSupportedException>(() => mockHtmlPartBase.Object.LoadXml(xml));

            //
            // Assert.
            //
            Assert.That(ex.Message, Is.EqualTo(@"Expected xml element name ""html"" but was ""incorrectElementName""."));
        }

        [Test]
        public void LoadXml_Calls_Load_Is_Successful()
        {
            //
            // Arrange.
            //
            var myHtmlPart = new MyHtmlPart();
            var xml = ReadAllTextFromProjectRelativeFilePath(@"Files\Parts\HtmlPartBase_001.xml");

            //
            // Act.
            //
            myHtmlPart.LoadXml(xml);

            //
            // Assert.
            //
            Assert.That(myHtmlPart.LoadCount, Is.EqualTo(1));
        }

        [TestCaseSource("Merge_TestCaseSource")]
        public void Merge_Null_Returns_Same_Value_Is_Successful(string value, MergeData mergeData, bool htmlEncodeMergeValue, string expectedHtml)
        {
            //
            // Arrange.
            //
            var myHtmlPart = new MyHtmlPart();

            //
            // Act.
            //
            var result = myHtmlPart.MergePublic(value, mergeData, htmlEncodeMergeValue);

            //
            // Assert.
            //
            Assert.That(result, Is.EqualTo(expectedHtml));
        }

        public IEnumerable<object[]> Merge_TestCaseSource()
        {
            // Null merge data. Get what you put in.
            yield return new object[] {"whatever", null, false, "whatever"};

            // Null merge data again with merge field, put it's ignored.
            yield return new object[] {"something else *|FirstName|*", null, false, "something else *|FirstName|*"};

            // With merge data.
            yield return new object[] {"whatever", new MergeData(), false, "whatever"};

            // With merge data and matching field.
            yield return new object[]
            {
                "something else *|FirstName|*", new MergeData().Add("FirstName", "Daffy"), false,
                "something else Daffy"
            };

            // With merge data and matching field and requires html encoding
            yield return new object[]
            {
                "*|Candidate1|* and maybe *|Candidate2|*",
                new MergeData()
                    .Add("Candidate1", "Daffy & Donald")
                    .Add("Candidate2", "Goofy"),
                true,
                "Daffy &amp; Donald and maybe Goofy"
            };
        }
    }
}