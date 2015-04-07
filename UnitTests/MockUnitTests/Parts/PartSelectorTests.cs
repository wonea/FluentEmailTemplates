using System;
using System.Collections.Generic;
using NUnit.Framework;
using VisualProduct.FluentEmailTemplates.Parts;

namespace VisualProduct.FluentEmailTemplates.UnitTests.MockUnitTests.Parts
{
    [TestFixture]
    public class PartSelectorTests
    {
        [TestCaseSource("GetHtmlPart_TestCaseSource")]
        public void GetHtmlPart_Is_Successful(string name, Type expectedType)
        {
            //
            // Arrange.
            //
            var partSelector = new PartSelector();

            //
            // Act.
            //
            var result = partSelector.GetHtmlPart(name);

            //
            // Assert.
            //
            Assert.That(result, Is.TypeOf(expectedType));
        }

        public IEnumerable<object[]> GetHtmlPart_TestCaseSource()
        {
            yield return new object[] { "container", typeof(ContainerPart) };
            yield return new object[] { "html", typeof(HtmlPart) };
            yield return new object[] { "htmlFile", typeof(HtmlFilePart) };
            yield return new object[] { "image", typeof(ImagePart) };
            yield return new object[] { "merge", typeof(MergePart) };
            yield return new object[] { "span", typeof(SpanPart) };
            yield return new object[] { "templateFile", typeof(TemplateFilePart) };
            yield return new object[] { "value", typeof(ValuePart) };
        }

        [Test]
        public void GetHtmlPart_Invalid_Name_Throws_Exception()
        {
            //
            // Arrange.
            //
            const string name = "gaga";
            var partSelector = new PartSelector();

            //
            // Act.
            //
            var ex = Assert.Throws<NotSupportedException>(() => partSelector.GetHtmlPart(name));


            //
            // Assert.
            //
            Assert.That(ex.Message, Is.EqualTo(@"Unknown html part ""gaga""."));
        }

        [TestCaseSource("GetStringPart_TestCaseSource")]
        public void GetStringPart_Is_Successful(string name, Type expectedType)
        {
            //
            // Arrange.
            //
            var partSelector = new PartSelector();

            //
            // Act.
            //
            var result = partSelector.GetStringPart(name);

            //
            // Assert.
            //
            Assert.That(result, Is.TypeOf(expectedType));
        }

        public IEnumerable<object[]> GetStringPart_TestCaseSource()
        {
            yield return new object[] { "merge", typeof(MergePart) };
            yield return new object[] { "value", typeof(ValuePart) };
        }

        [Test]
        public void GetStringPart_Invalid_Name_Throws_Exception()
        {
            //
            // Arrange.
            //
            const string name = "gaga";
            var partSelector = new PartSelector();

            //
            // Act.
            //
            var ex = Assert.Throws<NotSupportedException>(() => partSelector.GetStringPart(name));


            //
            // Assert.
            //
            Assert.That(ex.Message, Is.EqualTo(@"Unknown string part ""gaga""."));
        }
    }
}
