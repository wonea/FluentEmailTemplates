using System;
using NUnit.Framework;
using VisualProduct.FluentEmailTemplates.Parts;

namespace VisualProduct.FluentEmailTemplates.UnitTests.MockUnitTests.Parts
{
    [TestFixture]
    public class HtmlPartTests : TestBase
    {
        private HtmlPart _htmlPart;

        /// <Summary>
        /// Setup the unit test.
        /// </Summary>
        [SetUp]
        public void SetUp()
        {
            _htmlPart = new HtmlPart();
        }

        [Test]
        public void LoadXml_Is_Successful()
        {
            //
            // Arrange.
            //
            var xml = ReadAllTextFromProjectRelativeFilePath(@"Files\Parts\HtmlPart_001.xml");

            //
            // Act.
            //
            _htmlPart.LoadXml(xml);

            //
            // Assert.
            //
            var result = _htmlPart.GetHtml(new MergeData());
            AssertExpectedFileContents(result, @"Files\Parts\HtmlPart_001_Expected.html");
        }

        [Test]
        public void GetHtml_With_No_MergeData_Is_Successful()
        {
            //
            // Arrange.
            //
            var xml = ReadAllTextFromProjectRelativeFilePath(@"Files\Parts\HtmlPart_004.xml");
            _htmlPart.LoadXml(xml);
            var mergeData = new MergeData();

            //
            // Act.
            //
            var result = _htmlPart.GetHtml(mergeData);

            //
            // Assert.
            //
            var expected = ReadAllTextFromProjectRelativeFilePath(@"Files\Parts\HtmlPart_004_Expected.html");
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void GetHtml_Is_Successful()
        {
            //
            // Arrange.
            //
            var xml = ReadAllTextFromProjectRelativeFilePath(@"Files\Parts\HtmlPart_003.xml");
            _htmlPart.LoadXml(xml);
            var mergeData = new MergeData()
                .Add("FirstName", "Daffy");

            //
            // Act.
            //
            var result = _htmlPart.GetHtml(mergeData);

            //
            // Assert.
            //
            var expected = ReadAllTextFromProjectRelativeFilePath(@"Files\Parts\HtmlPart_003_Expected.html");
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void GetHtml_With_More_Keys_Than_Required_Is_Successful()
        {
            //
            // Arrange.
            //
            var xml = ReadAllTextFromProjectRelativeFilePath(@"Files\Parts\HtmlPart_003.xml");
            _htmlPart.LoadXml(xml);
            var mergeData = new MergeData()
                .Add("FirstName", "Daffy")
                .Add("LastName", "Duck");

            //
            // Act.
            //
            var result = _htmlPart.GetHtml(mergeData);

            //
            // Assert.
            //
            var expected = ReadAllTextFromProjectRelativeFilePath(@"Files\Parts\HtmlPart_003_Expected.html");
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void Merge_Is_Successful()
        {
            //
            // Arrange.
            //
            var xml = ReadAllTextFromProjectRelativeFilePath(@"Files\Parts\HtmlPart_005.xml");
            _htmlPart.LoadXml(xml);
            var mergeData = new MergeData()
                .Add("FirstName", "Daffy")
                .Add("LastName", "Duck");

            //
            // Act.
            //
            var result = _htmlPart.GetHtml(mergeData);

            //
            // Assert.
            //
            var expected = ReadAllTextFromProjectRelativeFilePath(@"Files\Parts\HtmlPart_005_Expected.html");
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void Merge_With_No_MergeData_Is_Successful()
        {
            //
            // Arrange.
            //
            var xml = ReadAllTextFromProjectRelativeFilePath(@"Files\Parts\HtmlPart_006.xml");
            _htmlPart.LoadXml(xml);
            var mergeData = new MergeData();

            //
            // Act.
            //
            var result = _htmlPart.GetHtml(mergeData);

            //
            // Assert.
            //
            var expected = ReadAllTextFromProjectRelativeFilePath(@"Files\Parts\HtmlPart_006_Expected.html");
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void Merge_With_Null_MergeData_Is_Successful()
        {
            //
            // Arrange.
            //
            var xml = ReadAllTextFromProjectRelativeFilePath(@"Files\Parts\HtmlPart_006.xml");
            _htmlPart.LoadXml(xml);

            //
            // Act.
            //
            var result = _htmlPart.GetHtml(null);

            //
            // Assert.
            //
            var expected = ReadAllTextFromProjectRelativeFilePath(@"Files\Parts\HtmlPart_006_Expected.html");
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}