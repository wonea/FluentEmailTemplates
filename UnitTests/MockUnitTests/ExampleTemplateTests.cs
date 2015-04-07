using FluentValidation;
using Moq;
using NUnit.Framework;

namespace VisualProduct.FluentEmailTemplates.UnitTests.MockUnitTests
{
    [TestFixture]
    public class ExampleTemplateTests : TestBase
    {
        private Mock<ISmtpSender> _mockSmtpSender;
        private Email _email;

        /// <Summary>
        /// Setup the unit test.
        /// </Summary>
        [SetUp]
        public void SetUp()
        {
            _mockSmtpSender = new Mock<ISmtpSender>();

            _email = new Email(
                _mockSmtpSender.Object);
        }

        /// <summary>
        /// A simple fluent email template merge.
        /// </summary>
        [Test]
        public void Example_001_Is_Successful()
        {
            //
            // Arrange.
            //
            var filePath = GetProjectRelativeFilePath(@"Files\EmailTemplates\Example_001.xml");
            var mergeData = new MergeData()
                .Add("FirstName", "Goofy")
                .Add("LastName", "Dawg")
                .Add("Address1", "1 Goof Street")
                .Add("Address2", "The Kennel")
                .Add("City", "Nuke Town")
                .Add("PostCode", "007")
                .Add("Country", "New Zealand");

            //
            // Act.
            //
            _email
                .From("mailer@example.com", "Mailer")
                .To("donald.duck@example.com", "Donald Duck")
                .WithEmailTemplateFromFile(filePath)
                .WithMergeData(mergeData)
                .Send();

            //
            // Assert.
            //
            AssertExpectedEmailSubjectAndHtmlBody(_email, "Example 001 subject for Goofy Dawg", @"Files\EmailTemplates\Example_001_Expected.html");
        }
    }
}