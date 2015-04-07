using System.IO;
using System.Linq;
using System.Net.Mail;
using FluentValidation;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using VisualProduct.FluentEmailTemplates.Parts;

namespace VisualProduct.FluentEmailTemplates.UnitTests.MockUnitTests
{
    [TestFixture]
    public class EmailTests : TestBase
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
        /// A simple fluent email example.
        /// </summary>
        [Test]
        public void Simple_Fluent_Email_Example_Is_Successful()
        {
            //
            // Act.
            //
            _email
                .From("mailer@example.com", "Mailer")
                .To("donald.duck@example.com", "Donald Duck")
                .WithSubject("Happy New Year from FluentEmailTemplates")
                .WithHtmlBody("<html><body>Happy New Year!</body></html>")
                .Send();

            //
            // Assert.
            //
            AssertExpectedEmailJson(_email, @"Files\EmailTests\SimpleFluent_Expected.json");
        }

        /// <summary>
        /// Call all fluent methods.
        /// Some details will be overridden by subsequent methods.
        /// </summary>
        [Test]
        public void All_Fluent_Methods_Is_Successful()
        {
            //
            // Arrange.
            //
            var loadFilePath = GetProjectRelativeFilePath(@"Files\EmailTests\Load.json");
            var htmlBodyFilePath = GetProjectRelativeFilePath(@"Files\EmailTests\Load.json");
            var saveFilePath = GetProjectRelativeFilePath(@"Files\EmailTests\Save.json");
            var emailMessage = new EmailMessage { Subject = "Ignored subject again - overwritten in WithSubject." };
            var mergeData = new MergeData();

            // If the save file already exists delete it.
            if (File.Exists(saveFilePath))
            {
                File.Delete(saveFilePath);
            }

            //
            // Act.
            //
            var json = _email
                .WithSubject("A subject that is overridden below") // Overridden in following methods.
                .Load(loadFilePath) // Overridden in following methods.
                .WithEmailMessage(emailMessage) // Overridden in following methods.
                .WithHtmlBody("<html></html>") // Overridden in following methods.
                .WithHtmlBodyFromFile(htmlBodyFilePath) // Overridden in following methods.
                .Bcc("donald.duck@example.com", "Donald Duck")
                .Bcc("daffy.duck@example.com")
                .Cc("mickey.mouse@example.com", "Mickey Mouse")
                .Cc("goofy@example.com")
                .From("mailer@example.com", "Mailer")
                .To("donald.duck@example.com", "Donald Duck")
                .To("peter.griffin@example.com", "Peter Griffin")
                .WithMergeData(mergeData)
                .WithSubject("The final subject")
                .Save(saveFilePath)
                .Send()
                .GetEmailMessageJson();

            //
            // Assert.
            //
            Assert.That(File.Exists(saveFilePath), Is.True);
            // Clean up.
            File.Delete(saveFilePath);

            // Ensure the json is as expected.
            AssertExpectedEmailJson(_email, @"Files\EmailTests\AllFluent_Expected.json");
        }

        [Test]
        public void Save_Is_Successful()
        {
            //
            // Arrange.
            //
            var filePath = GetProjectRelativeFilePath(@"Files\EmailTests\Save.json");
            // If the file already exists delete it.
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            //
            // Act.
            //
            _email.Save(filePath);

            //
            // Assert.
            //
            Assert.That(File.Exists(filePath), Is.True);
            // Clean up.
            File.Delete(filePath);
        }

        [Test]
        public void Load_Is_Successful()
        {
            //
            // Arrange.
            //
            var filePath = GetProjectRelativeFilePath(@"Files\EmailTests\Load.json");

            //
            // Act.
            //
            _email.Load(filePath);

            //
            // Assert.
            //
            var emailMessage = _email.GetEmailMessage();
            Assert.That(emailMessage, Is.Not.Null);
            Assert.That(emailMessage.Subject, Is.EqualTo("My load subject"));
        }

        [Test]
        public void Send_Is_Successful()
        {
            //
            // Arrange.
            //
            var filePath = GetProjectRelativeFilePath(@"Files\EmailTests\Send.json");
            _email.Load(filePath);

            //
            // Act.
            //
            _email.Send();

            //
            // Assert.
            //
            _mockSmtpSender.Verify(o => o.Send(It.IsAny<MailMessage>()), Times.Once);
        }

        [Test]
        public void Send_With_Invalid_Email_Throws_Exception()
        {
            //
            // Arrange.
            //
            // No recipient.
            _email
                .WithSubject("My subject")
                .WithHtmlBody("<html></html>")
                .From("donald@example.com");

            //
            // Act.
            //
            var ex = Assert.Throws<ValidationException>(() => _email.Send());

            //
            // Assert.
            //
            Assert.That(ex.Message, Is.StringStarting("Validation failed"));
            Assert.That(ex.Errors.Any(), Is.True);
        }

        [TestCase("test1@example.com", null)]
        [TestCase("donald.duck@example.com", "Donald Duck")]
        public void From_Is_Successful(string emailAddress, string displayName)
        {
            //
            // Arrange.
            //

            //
            // Act.
            //
            _email.From(emailAddress, displayName);

            //
            // Assert.
            //
            var emailMessage = _email.GetEmailMessage();
            Assert.That(emailMessage.From, Is.Not.Null);
            Assert.That(emailMessage.From.DisplayName, Is.EqualTo(displayName));
            Assert.That(emailMessage.From.Address, Is.EqualTo(emailAddress));
        }

        [Test]
        public void WithEmailMessage_Is_Successful()
        {
            //
            // Arrange.
            //
            const string subject = "My subject";
            var emailMessage = new EmailMessage {Subject = subject};

            //
            // Act.
            //
            _email.WithEmailMessage(emailMessage);

            //
            // Assert.
            //
            emailMessage = _email.GetEmailMessage();
            Assert.That(emailMessage.Subject, Is.EqualTo(subject));
        }

        [Test]
        public void WithHtmlBodyFromFile_Is_Successful()
        {
            //
            // Arrange.
            //
            var filePath = GetProjectRelativeFilePath(@"Files\EmailTests\WithHtmlBodyFromFile.html");

            //
            // Act.
            //
            _email.WithHtmlBodyFromFile(filePath);

            //
            // Assert.
            //
            var emailMessage = _email.GetEmailMessage();
            Assert.That(emailMessage.HtmlBody, Is.StringContaining(@"<b>EmailTests.WithHtmlBodyFromFile_Is_Successful</b>"));
        }

        [Test]
        public void WithHtmlBody_Is_Successful()
        {
            //
            // Arrange.
            //
            const string html = "<html></html>";

            //
            // Act.
            //
            _email.WithHtmlBody(html);

            //
            // Assert.
            //
            var emailMessage = _email.GetEmailMessage();
            Assert.That(emailMessage.HtmlBody, Is.EqualTo(html));
        }

        [Test]
        public void WithSubject_Is_Successful()
        {
            //
            // Arrange.
            //
            const string subject = "My test subject";

            //
            // Act.
            //
            _email.WithSubject(subject);

            //
            // Assert.
            //
            var emailMessage = _email.GetEmailMessage();
            Assert.That(emailMessage.Subject, Is.EqualTo(subject));
        }

        [Test]
        public void GetEmailMessageJson_Empty_Is_Successful()
        {
            //
            // Arrange.
            //

            //
            // Act.
            //
            var json = _email.GetEmailMessageJson(Formatting.None);

            //
            // Assert.
            //
            Assert.That(json, Is.EqualTo(@"{""Bcc"":[],""Cc"":[],""From"":null,""HtmlBody"":null,""Subject"":null,""To"":[]}"));
        }

        [Test]
        public void GetEmailMessageJson_With_Subject_No_Formatting_Is_Successful()
        {
            //
            // Arrange.
            //
            _email.WithSubject("My test subject");
            
            //
            // Act.
            //
            var json = _email.GetEmailMessageJson(Formatting.None);

            //
            // Assert.
            //
            Assert.That(json, Is.EqualTo(@"{""Bcc"":[],""Cc"":[],""From"":null,""HtmlBody"":null,""Subject"":""My test subject"",""To"":[]}"));
        }

        [TestCase("test1@example.com", null)]
        [TestCase("donald.duck@example.com", "Donald Duck")]
        public void Bcc_Single_Recipient_Is_Successful(string emailAddress, string displayName)
        {
            //
            // Arrange.
            //

            //
            // Act.
            //
            _email.Bcc(emailAddress, displayName);

            //
            // Assert.
            //
            var emailMessage = _email.GetEmailMessage();
            Assert.That(emailMessage.Bcc.Count, Is.EqualTo(1));
            Assert.That(emailMessage.Bcc.Any(o => o.Address == emailAddress && o.DisplayName == displayName), Is.True);
        }

        [TestCase("test1@example.com", null)]
        [TestCase("donald.duck@example.com", "Donald Duck")]
        public void Bcc_Mulitiple_Recipients_Is_Successful(string emailAddress, string displayName)
        {
            //
            // Arrange.
            //
            // Add an existing recipient.
            const string existingEmailAddress = "mickey.mouse@example.com";
            _email.Bcc(existingEmailAddress);

            //
            // Act.
            //
            _email.Bcc(emailAddress, displayName);

            //
            // Assert.
            //
            var emailMessage = _email.GetEmailMessage();
            Assert.That(emailMessage.Bcc.Count, Is.EqualTo(2));
            Assert.That(emailMessage.Bcc.Any(o => o.Address == emailAddress && o.DisplayName == displayName), Is.True);
            Assert.That(emailMessage.Bcc.Any(o => o.Address == existingEmailAddress && o.DisplayName == null), Is.True);
        }

        [TestCase("test1@example.com", null)]
        [TestCase("donald.duck@example.com", "Donald Duck")]
        public void Cc_Single_Recipient_Is_Successful(string emailAddress, string displayName)
        {
            //
            // Arrange.
            //

            //
            // Act.
            //
            _email.Cc(emailAddress, displayName);

            //
            // Assert.
            //
            var emailMessage = _email.GetEmailMessage();
            Assert.That(emailMessage.Cc.Count, Is.EqualTo(1));
            Assert.That(emailMessage.Cc.Any(o => o.Address == emailAddress && o.DisplayName == displayName), Is.True);
        }

        [TestCase("test1@example.com", null)]
        [TestCase("donald.duck@example.com", "Donald Duck")]
        public void Cc_Mulitiple_Recipients_Is_Successful(string emailAddress, string displayName)
        {
            //
            // Arrange.
            //
            // Add an existing recipient.
            const string existingEmailAddress = "mickey.mouse@example.com";
            _email.Cc(existingEmailAddress);

            //
            // Act.
            //
            _email.Cc(emailAddress, displayName);

            //
            // Assert.
            //
            var emailMessage = _email.GetEmailMessage();
            Assert.That(emailMessage.Cc.Count, Is.EqualTo(2));
            Assert.That(emailMessage.Cc.Any(o => o.Address == emailAddress && o.DisplayName == displayName), Is.True);
            Assert.That(emailMessage.Cc.Any(o => o.Address == existingEmailAddress && o.DisplayName == null), Is.True);
        }

        [TestCase("test1@example.com", null)]
        [TestCase("donald.duck@example.com", "Donald Duck")]
        public void To_Single_Recipient_Is_Successful(string emailAddress, string displayName)
        {
            //
            // Arrange.
            //

            //
            // Act.
            //
            _email.To(emailAddress, displayName);

            //
            // Assert.
            //
            var emailMessage = _email.GetEmailMessage();
            Assert.That(emailMessage.To.Count, Is.EqualTo(1));
            Assert.That(emailMessage.To.Any(o => o.Address == emailAddress && o.DisplayName == displayName), Is.True);
        }

        [TestCase("test1@example.com", null)]
        [TestCase("donald.duck@example.com", "Donald Duck")]
        public void To_Mulitiple_Recipients_Is_Successful(string emailAddress, string displayName)
        {
            //
            // Arrange.
            //
            // Add an existing recipient.
            const string existingEmailAddress = "mickey.mouse@example.com";
            _email.To(existingEmailAddress);

            //
            // Act.
            //
            _email.To(emailAddress, displayName);

            //
            // Assert.
            //
            var emailMessage = _email.GetEmailMessage();
            Assert.That(emailMessage.To.Count, Is.EqualTo(2));
            Assert.That(emailMessage.To.Any(o => o.Address == emailAddress && o.DisplayName == displayName), Is.True);
            Assert.That(emailMessage.To.Any(o => o.Address == existingEmailAddress && o.DisplayName == null), Is.True);
        }

        [Test]
        public void Merge_With_Subject_Is_Successful()
        {
            //
            // Arrange.
            //
            const string subject = "My subject";
            var mockEmailTemplate = new Mock<IEmailTemplate>();
            var mockStringPart = new Mock<IStringPart>();
            mockStringPart.Setup(o => o.GetString(It.IsAny<MergeData>())).Returns(subject);
            mockEmailTemplate.Setup(o => o.Subject).Returns(mockStringPart.Object);
            var mergeData = new MergeData();

            //
            // Act.
            //
            // Merge happens when the email has an email template AND merge data.
            _email
                .WithEmailTemplate(mockEmailTemplate.Object)
                .WithMergeData(mergeData);

            //
            // Assert.
            //
            mockEmailTemplate.VerifyGet(o => o.Subject, Times.Exactly(2));
            var emailMessage = _email.GetEmailMessage();
            Assert.That(emailMessage.Subject, Is.EqualTo(subject));
        }

        [Test]
        public void Merge_With_No_Subject_Is_Successful()
        {
            //
            // Arrange.
            //
            var mockEmailTemplate = new Mock<IEmailTemplate>();
            mockEmailTemplate.Setup(o => o.Subject).Returns<IStringPart>(null);
            var mergeData = new MergeData();

            //
            // Act.
            //
            // Merge happens when the email has an email template AND merge data.
            _email
                .WithEmailTemplate(mockEmailTemplate.Object)
                .WithMergeData(mergeData);

            //
            // Assert.
            //
            mockEmailTemplate.VerifyGet(o => o.Subject, Times.Once);
            var emailMessage = _email.GetEmailMessage();
            Assert.That(emailMessage.Subject, Is.Null);
        }

        [Test]
        public void Merge_With_HtmlBody_Is_Successful()
        {
            //
            // Arrange.
            //
            const string htmlBody = "My html body";
            var mockEmailTemplate = new Mock<IEmailTemplate>();
            var mockHtmlPart = new Mock<IHtmlPart>();
            mockHtmlPart.Setup(o => o.GetHtml(It.IsAny<MergeData>())).Returns(htmlBody);
            mockEmailTemplate.Setup(o => o.HtmlBody).Returns(mockHtmlPart.Object);
            var mergeData = new MergeData();

            //
            // Act.
            //
            // Merge happens when the email has an email template AND merge data.
            _email
                .WithEmailTemplate(mockEmailTemplate.Object)
                .WithMergeData(mergeData);

            //
            // Assert.
            //
            mockEmailTemplate.VerifyGet(o => o.HtmlBody, Times.Exactly(2));
            var emailMessage = _email.GetEmailMessage();
            Assert.That(emailMessage.HtmlBody, Is.EqualTo(htmlBody));
        }

        [Test]
        public void Merge_With_No_HtmlBody_Is_Successful()
        {
            //
            // Arrange.
            //
            var mockEmailTemplate = new Mock<IEmailTemplate>();
            mockEmailTemplate.Setup(o => o.HtmlBody).Returns<IHtmlPart>(null);
            var mergeData = new MergeData();

            //
            // Act.
            //
            // Merge happens when the email has an email template AND merge data.
            _email
                .WithEmailTemplate(mockEmailTemplate.Object)
                .WithMergeData(mergeData);

            //
            // Assert.
            //
            mockEmailTemplate.VerifyGet(o => o.HtmlBody, Times.Once);
            var emailMessage = _email.GetEmailMessage();
            Assert.That(emailMessage.HtmlBody, Is.Null);
        }

        [Test]
        public void Validate_Empty_Email_Message_Is_Successful()
        {
            //
            // Act.
            //
            var result = _email.Validate();

            //
            // Assert.
            //
            Assert.That(result.IsValid, Is.False);
        }

        [Test]
        public void Validate_Is_Successful()
        {
            //
            // Arrange.
            //
            _email
                .WithSubject("My subject")
                .WithHtmlBody("<html></html>")
                .To("daffy@example.com")
                .From("donald@example.com");

            //
            // Act.
            //
            var result = _email.Validate();

            //
            // Assert.
            //
            Assert.That(result.IsValid, Is.True);
        }
    }
}