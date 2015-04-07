using System;
using Moq;
using NUnit.Framework;
using VisualProduct.FluentEmailTemplates.Parts;

namespace VisualProduct.FluentEmailTemplates.UnitTests.MockUnitTests.Parts
{
    [TestFixture]
    public class ContainerPartTests : TestBase
    {
        private Mock<IHtmlPart> _mockHtmlPart;
        private Mock<IPartSelector> _mockPartSelector;
        private ContainerPart _containerPart;

        /// <Summary>
        /// Setup the unit test.
        /// </Summary>
        [SetUp]
        public void SetUp()
        {
            _mockHtmlPart = new Mock<IHtmlPart>();
            _mockHtmlPart.Setup(o => o.GetHtml(It.IsAny<MergeData>())).Returns("<span></span>");

            _mockPartSelector = new Mock<IPartSelector>();
            _mockPartSelector.Setup(o => o.GetHtmlPart(It.IsAny<string>())).Returns(_mockHtmlPart.Object);

            _containerPart = new ContainerPart(
                _mockPartSelector.Object);
        }

        [Test]
        public void LoadXml_Is_Successful()
        {
            //
            // Arrange.
            //
            var xml = ReadAllTextFromProjectRelativeFilePath(@"Files\Parts\ContainerPart_001.xml");

            //
            // Act & Assert.
            //
            Assert.DoesNotThrow(() => _containerPart.LoadXml(xml));
        }

        [TestCase(true, "100px", @"<div style=""width:100px;""><span></span></div>")]
        [TestCase(true, "50px", @"<div style=""width:50px;""><span></span></div>")]
        [TestCase(false, null, @"<div style=""width:*|Width|*;""><span></span></div>")]
        public void LoadXml_With_Render_True_Is_Successful(bool hasMergeData, string width, string expected)
        {
            //
            // Arrange.
            //
            var xml = ReadAllTextFromProjectRelativeFilePath(@"Files\Parts\ContainerPart_002.xml");
            var mergeData = hasMergeData
                ? new MergeData().Add("Width", width)
                : null;

            //
            // Act.
            //
            _containerPart.LoadXml(xml);

            //
            // Assert.
            //
            var html = _containerPart.GetHtml(mergeData);
            Assert.That(html, Is.EqualTo(expected));
        }

        [Test]
        public void LoadXml_With_Invalid_Element_Name_Throws_Exception()
        {
            //
            // Arrange.
            //
            var xml = ReadAllTextFromProjectRelativeFilePath(@"Files\Parts\ContainerPart_003.xml");

            //
            // Act.
            //
            var ex = Assert.Throws<NotSupportedException>(() => _containerPart.LoadXml(xml));

            //
            // Assert.
            //
            Assert.That(ex.Message, Is.EqualTo(@"Expected xml element name ""container"" but was ""incorrectElementName""."));
        }
    }
}