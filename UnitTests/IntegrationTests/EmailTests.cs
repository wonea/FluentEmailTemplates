using System.Configuration;
using NUnit.Framework;

namespace VisualProduct.FluentEmailTemplates.UnitTests.IntegrationTests
{
    /// <summary>
    /// These tests actually send an email. Hence the [Explicit] attribute.
    /// 
    /// Change the username & password settings in the app.config file of this project
    /// to get this working for your smtp server / account.
    /// 
    /// If you're using smtp.gmail.com you may also need to configure your account
    /// to allow less secure app access:
    /// https://www.google.com/settings/security/lesssecureapps
    /// 
    /// For other secure connection issues try the answers on this stack overflow thread:
    /// http://stackoverflow.com/questions/20906077/gmail-error-the-smtp-server-requires-a-secure-connection-or-the-client-was-not
    /// </summary>
    [TestFixture]
    [Explicit]
    public class EmailTests : TestBase
    {
        /// <summary>
        /// Recipient of the test emails.
        /// </summary>
        private static readonly string FromEmailAddress;
        private static readonly string FromDisplayName;

        /// <summary>
        /// Sender of the test emails.
        /// </summary>
        private static readonly string ToEmailAddress;
        private static readonly string ToDisplayName;

        private Email _email;

        static EmailTests()
        {
            FromEmailAddress = ConfigurationManager.AppSettings["FromEmailAddress"];
            FromDisplayName = ConfigurationManager.AppSettings["FromDisplayName"];
            ToEmailAddress = ConfigurationManager.AppSettings["ToEmailAddress"];
            ToDisplayName = ConfigurationManager.AppSettings["ToDisplayName"];
        }

        /// <Summary>
        /// Setup the unit test.
        /// </Summary>
        [SetUp]
        public void SetUp()
        {
            var smtpSender = new SmtpSender();

            _email = new Email(smtpSender);
        }

        /// <summary>
        /// A simple fluent email example.
        /// </summary>
        [Test]
        public void Send_Love_For_FluentEmailTemplates()
        {
            //
            // Act.
            //
            _email
                .From(FromEmailAddress, FromDisplayName)
                .To(ToEmailAddress, ToDisplayName)
                .WithSubject("Loving FluentEmailTemplates")
                .WithHtmlBody("<html><body>OMG, FluentEmailTemplates is perfect!</body></html>")
                .Send();
        }

        /// <summary>
        /// A simple fluent email template merge.
        /// </summary>
        [Test]
        public void Send_Example_001()
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
                .From(FromEmailAddress, FromDisplayName)
                .To(ToEmailAddress, ToDisplayName)
                .WithEmailTemplateFromFile(filePath)
                .WithMergeData(mergeData)
                .Send();
        }
    }
}