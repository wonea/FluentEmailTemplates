using NUnit.Framework;
using VisualProduct.FluentEmailTemplates.Parts;

namespace VisualProduct.FluentEmailTemplates.UnitTests.MockUnitTests.Parts
{
    [TestFixture]
    public class ValuePartTests
    {
        [TestCase(@"<value>*|FirstName|*</value>", "Daffy", "Daffy")]
        [TestCase(@"<value>*|LastName|*</value>", "Daffy", "*|LastName|*")]
        [TestCase(@"<value>*|FirstName|*</value>", "Duck & Dawg", "Duck &amp; Dawg")]
        public void GetHtml_Is_Successful(string xml, string mergeDataValue, string expected)
        {
            //
            // Arrange.
            //
            var mergePart = new ValuePart();
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
            var mergePart = new ValuePart();
            mergePart.LoadXml(@"<value>*|FirstName|*</value>");

            //
            // Act.
            //
            var result = mergePart.GetHtml(null);

            //
            // Assert.
            //
            Assert.That(result, Is.EqualTo("*|FirstName|*"));
        }

        [Test]
        public void GetHtml_With_Empty_MergeData_Is_Successful()
        {
            //
            // Arrange.
            //
            var mergePart = new ValuePart();
            mergePart.LoadXml(@"<value>*|FirstName|*</value>");
            // Merge data with no keys & values.
            var mergeData = new MergeData();

            //
            // Act.
            //
            var result = mergePart.GetHtml(mergeData);

            //
            // Assert.
            //
            Assert.That(result, Is.EqualTo("*|FirstName|*"));
        }

        [TestCase(@"<value>*|FirstName|*</value>", "Daffy", "Daffy")]
        [TestCase(@"<value>*|LastName|*</value>", "Daffy", "*|LastName|*")]
        [TestCase(@"<value>*|FirstName|*</value>", "Duck & Dawg", "Duck & Dawg")]
        public void GetString_Is_Successful(string xml, string mergeDataValue, string expected)
        {
            //
            // Arrange.
            //
            var mergePart = new ValuePart();
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
            var mergePart = new ValuePart();
            mergePart.LoadXml(@"<value>*|FirstName|*</value>");

            //
            // Act.
            //
            var result = mergePart.GetString(null);

            //
            // Assert.
            //
            Assert.That(result, Is.EqualTo("*|FirstName|*"));
        }

        [Test]
        public void GetString_With_Empty_MergeData_Is_Successful()
        {
            //
            // Arrange.
            //
            var mergePart = new ValuePart();
            mergePart.LoadXml(@"<value>*|FirstName|*</value>");
            // Merge data with no keys & values.
            var mergeData = new MergeData();

            //
            // Act.
            //
            var result = mergePart.GetString(mergeData);

            //
            // Assert.
            //
            Assert.That(result, Is.EqualTo("*|FirstName|*"));
        }
    }
}