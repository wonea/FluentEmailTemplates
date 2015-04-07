using NUnit.Framework;
using VisualProduct.FluentEmailTemplates.Parts;

namespace VisualProduct.FluentEmailTemplates.UnitTests.MockUnitTests.Parts
{
    [TestFixture]
    public class MergePartTests
    {
        [TestCase(@"<merge key=""FirstName"" />", "Daffy", "Daffy")]
        [TestCase(@"<merge key=""LastName"" />", "Daffy", null)]
        [TestCase(@"<merge key=""FirstName"" />", "Duck & Dawg", "Duck &amp; Dawg")]
        public void GetHtml_Is_Successful(string xml, string mergeDataValue, string expected)
        {
            //
            // Arrange.
            //
            var mergePart = new MergePart();
            mergePart.LoadXml(xml);
            var mergeData = new MergeData()
                .Add("FirstName", mergeDataValue);

            //
            // Act.
            //
            var result = mergePart.GetHtml(mergeData);

            //
            // Assert.
            //
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void GetHtml_With_Null_MergeData_Is_Successful()
        {
            //
            // Arrange.
            //
            var mergePart = new MergePart();
            mergePart.LoadXml(@"<merge key=""FirstName"" />");

            //
            // Act.
            //
            var result = mergePart.GetHtml(null);

            //
            // Assert.
            //
            Assert.That(result, Is.EqualTo(null));
        }

        [Test]
        public void GetHtml_With_Empty_MergeData_Is_Successful()
        {
            //
            // Arrange.
            //
            var mergePart = new MergePart();
            mergePart.LoadXml(@"<merge key=""FirstName"" />");
            // Merge data with no keys & values.
            var mergeData = new MergeData();

            //
            // Act.
            //
            var result = mergePart.GetHtml(mergeData);

            //
            // Assert.
            //
            Assert.That(result, Is.EqualTo(null));
        }

        [TestCase(@"<merge key=""FirstName"" />", "Daffy", "Daffy")]
        [TestCase(@"<merge key=""LastName"" />", "Daffy", null)]
        [TestCase(@"<merge key=""FirstName"" />", "Duck & Dawg", "Duck & Dawg")]
        public void GetString_Is_Successful(string xml, string mergeDataValue, string expected)
        {
            //
            // Arrange.
            //
            var mergePart = new MergePart();
            mergePart.LoadXml(xml);
            var mergeData = new MergeData()
                .Add("FirstName", mergeDataValue);

            //
            // Act.
            //
            var result = mergePart.GetString(mergeData);

            //
            // Assert.
            //
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void GetString_With_Null_MergeData_Is_Successful()
        {
            //
            // Arrange.
            //
            var mergePart = new MergePart();
            mergePart.LoadXml(@"<merge key=""FirstName"" />");

            //
            // Act.
            //
            var result = mergePart.GetString(null);

            //
            // Assert.
            //
            Assert.That(result, Is.EqualTo(null));
        }

        [Test]
        public void GetString_With_Empty_MergeData_Is_Successful()
        {
            //
            // Arrange.
            //
            var mergePart = new MergePart();
            mergePart.LoadXml(@"<merge key=""FirstName"" />");
            // Merge data with no keys & values.
            var mergeData = new MergeData();

            //
            // Act.
            //
            var result = mergePart.GetString(mergeData);

            //
            // Assert.
            //
            Assert.That(result, Is.EqualTo(null));
        }
    }
}