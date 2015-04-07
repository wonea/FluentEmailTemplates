using System;
using NUnit.Framework;
using VisualProduct.FluentEmailTemplates.Parts;

namespace VisualProduct.FluentEmailTemplates.UnitTests.MockUnitTests.Parts
{
    [TestFixture]
    public class FilePartBaseTests : TestBase
    {
        /// <summary>
        /// A class derived from the abstract class so we can
        /// call protected methods in the base class.
        /// </summary>
        public class MyFilePartBase : FilePartBase
        {
            public MyFilePartBase() 
                : base("testFile")
            {
            }

            /// <summary>
            /// Public method to expose the protected GetFilePathFromStoredAttributes method.
            /// </summary>
            public string GetFilePathFromStoredAttributesPublic()
            {
                var value = GetFilePathFromStoredAttributes();
                return value;
            }

            public override string GetHtml(MergeData mergeData)
            {
                return "nada";
            }
        }

        private MyFilePartBase _myFilePartBase;

        /// <Summary>
        /// Setup the unit test.
        /// </Summary>
        [SetUp]
        public void SetUp()
        {
            _myFilePartBase = new MyFilePartBase();
        }

        [Test]
        public void Missing_FilePath_And_RelativePath_Throws_Exception()
        {
            //
            // Arrange.
            //
            const string xml = @"<testFile>";
            _myFilePartBase.LoadXml(xml);

            //
            // Act.
            //
            var ex = Assert.Throws<ArgumentException>(() => _myFilePartBase.GetFilePathFromStoredAttributesPublic());

            //
            // Assert.
            //
            Assert.That(ex.Message, Is.EqualTo(@"Missing ""filePath"" or ""relativePath"" attribute for html file part."));
        }

        [Test]
        public void Missing_File_At_FilePath_Throws_Exception()
        {
            //
            // Arrange.
            //
            const string filePath = @"C:\Some\random\file\path\that\does\not\exist.html";
            var xml = string.Format(@"<testFile filePath=""{0}"">", filePath);
            _myFilePartBase.LoadXml(xml);

            //
            // Act.
            //
            var ex = Assert.Throws<ArgumentException>(() => _myFilePartBase.GetFilePathFromStoredAttributesPublic());

            //
            // Assert.
            //
            Assert.That(ex.Message, Is.EqualTo(string.Format(@"Unable to find html part at file location ""{0}"" as referenced by attribute ""filePath"".", filePath)));
        }

        [Test]
        public void Missing_File_At_RelativePath_Throws_Exception()
        {
            //
            // Arrange.
            //
            const string relativePath = @"Some\random\file\path\that\does\not\exist.html";
            var xml = string.Format(@"<testFile relativePath=""{0}"">", relativePath);
            _myFilePartBase.LoadXml(xml);

            //
            // Act.
            //
            var ex = Assert.Throws<ArgumentException>(() => _myFilePartBase.GetFilePathFromStoredAttributesPublic());

            //
            // Assert.
            //
            Assert.That(ex.Message, Is.StringContaining(@"referenced by app setting ""FluentEmailTemplatesFilePath"""));
        }

        [Test]
        public void LoadXml_With_FilePath_Is_Successful()
        {
            //
            // Arrange.
            //
            var filePath = GetProjectRelativeFilePath(@"Files\Parts\HtmlFilePart_001.html");
            var xml = string.Format(@"<testFile filePath=""{0}"" />", filePath);

            //
            // Act.
            //
            _myFilePartBase.LoadXml(xml);

            //
            // Assert.
            //
            var result = _myFilePartBase.GetHtml(null);
            Assert.That(result, Is.EqualTo("nada"));
        }

        [Test]
        public void LoadXml_With_RelativePath_Is_Successful()
        {
            //
            // Arrange.
            //
            const string xml = @"<testFile relativePath=""Parts\HtmlFilePart_001.html"" />";

            //
            // Act.
            //
            _myFilePartBase.LoadXml(xml);

            //
            // Assert.
            //
            var result = _myFilePartBase.GetHtml(null);
            Assert.That(result, Is.EqualTo("nada"));
        }

        [Test]
        public void LoadXml_With_FilePath_And_MergeData_Is_Successful()
        {
            //
            // Arrange.
            //
            const string xml = @"<testFile filePath=""ignored"" />";;
            var mergeData = new MergeData()
                .Add("FirstName", "John");

            //
            // Act.
            //
            _myFilePartBase.LoadXml(xml);

            //
            // Assert.
            //
            var result = _myFilePartBase.GetHtml(mergeData);
            Assert.That(result, Is.EqualTo("nada"));
        }

        [Test]
        public void LoadXml_With_RelativePath_And_MergeData_Is_Successful()
        {
            //
            // Arrange.
            //
            const string xml = @"<testFile relativePath=""Parts\HtmlFilePart_002.html"" />";
            var mergeData = new MergeData()
                .Add("FirstName", "John");

            //
            // Act.
            //
            _myFilePartBase.LoadXml(xml);

            //
            // Assert.
            //
            var result = _myFilePartBase.GetHtml(mergeData);
            Assert.That(result, Is.EqualTo("nada"));
        }
    }
}