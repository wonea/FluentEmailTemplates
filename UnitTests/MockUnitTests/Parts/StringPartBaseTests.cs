using Moq;
using NUnit.Framework;
using VisualProduct.FluentEmailTemplates.Parts;

namespace VisualProduct.FluentEmailTemplates.UnitTests.MockUnitTests.Parts
{
    public class StringPartBaseTests
    {
        private Mock<StringPartBase> _mockStringPartBase;

        /// <Summary>
        /// Setup the unit test.
        /// </Summary>
        [SetUp]
        public void SetUp()
        {
            _mockStringPartBase = new Mock<StringPartBase>(
                "nada")
            {
                CallBase = true
            };

            _mockStringPartBase.Setup(o => o.GetString(It.IsAny<MergeData>())).Returns("the value");
            _mockStringPartBase.Setup(o => o.GetString(It.Is<MergeData>(x => x == null))).Returns<string>(null);
        }

        [Test]
        public void GetHtml_Is_Successful()
        {
            //
            // Arrange.
            //
            var mergeData = new MergeData();

            //
            // Act.
            //
            var result = _mockStringPartBase.Object.GetHtml(mergeData);

            //
            // Assert.
            //
            Assert.That(result, Is.EqualTo("the value"));
        }

        [Test]
        public void GetHtml_With_Null_MergeData_Is_Successful()
        {
            //
            // Act & Assert.
            //
            var result = _mockStringPartBase.Object.GetHtml(null);

            //
            // Assert.
            //
            Assert.That(result, Is.EqualTo(null));
        }
    }
}