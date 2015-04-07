using NUnit.Framework;

namespace VisualProduct.FluentEmailTemplates.UnitTests.MockUnitTests
{
    [TestFixture]
    public class MergeDataTests
    {
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(100)]
        public void Count_Is_Successful(int addCount)
        {
            //
            // Arrange.
            //
            var mergeData = new MergeData();
            for (var i = 0; i < addCount; i++)
            {
                mergeData.Add("Key " + i, "Value " + i);
            }

            //
            // Act.
            //
            var count = mergeData.Count;

            //
            // Assert.
            //
            Assert.That(count, Is.EqualTo(addCount));
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(100)]
        public void GetValue_Is_Successful(int addCount)
        {
            //
            // Arrange.
            //
            var mergeData = new MergeData();
            for (var i = 0; i < addCount; i++)
            {
                mergeData.Add("Key " + i, "Value " + i);
            }

            //
            // Act.
            //


            //
            // Assert.
            //
            for (var i = 0; i < addCount; i++)
            {
                var value = mergeData.GetValue("Key " + i);
                Assert.That(value, Is.EqualTo("Value " + i));
            }
        }
    }
}