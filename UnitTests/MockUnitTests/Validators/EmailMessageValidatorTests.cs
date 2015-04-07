using System.Collections.Generic;
using System.Linq;
using FluentValidation.TestHelper;
using NUnit.Framework;
using VisualProduct.FluentEmailTemplates.Validators;

namespace VisualProduct.FluentEmailTemplates.UnitTests.MockUnitTests.Validators
{
    [TestFixture]
    public class EmailMessageValidatorTests
    {
        private EmailMessageValidator _validator;

        [SetUp]
        public void SetUp()
        {
            _validator = new EmailMessageValidator();
        }

        [TestCase("")]
        [TestCase(null)]
        public void Subject_Error_Is_Successful(string value)
        {
            //
            // Act.
            //
            _validator.ShouldHaveValidationErrorFor(o => o.Subject, value);
        }

        [TestCase("A")]
        [TestCase("My subject")]
        public void Subject_Is_Successful(string value)
        {
            //
            // Act.
            //
            _validator.ShouldNotHaveValidationErrorFor(o => o.Subject, value);
        }

        [TestCase("")]
        [TestCase(null)]
        public void HtmlBody_Error_Is_Successful(string value)
        {
            //
            // Act.
            //
            _validator.ShouldHaveValidationErrorFor(o => o.HtmlBody, value);
        }

        [TestCase("A")]
        [TestCase("My html body")]
        public void HtmlBody_Is_Successful(string value)
        {
            //
            // Act.
            //
            _validator.ShouldNotHaveValidationErrorFor(o => o.HtmlBody, value);
        }

        [TestCaseSource("From_TestCaseSource")]
        public void From_Is_Successful(EmailMessage.MailAddress mailAddress)
        {
            //
            // Act.
            //
            _validator.ShouldNotHaveValidationErrorFor(o => o.From, mailAddress);
        }

        public IEnumerable<object[]> From_TestCaseSource()
        {
            // Fully populated mail address.
            yield return new object[]
            {
                new EmailMessage.MailAddress
                {
                    Address = "goofy@example.com",
                    DisplayName = "Goofy Goof"
                }
            };

            // Mail address without a display name.
            yield return new object[]
            {
                new EmailMessage.MailAddress { Address = "goofy@example.com" }
            };
        }

        [Test]
        public void From_Null_Is_Successful()
        {
            //
            // Act.
            //
            _validator.ShouldHaveValidationErrorFor(o => o.From, (EmailMessage.MailAddress)null);
        }

        [TestCaseSource("From_Error_TestCaseSource")]
        public void From_Error_Is_Successful(EmailMessage.MailAddress mailAddress)
        {
            //
            // Arrange.
            //
            var emailMessage = new EmailMessage
            {
                Subject = "My subject",
                HtmlBody = "<html></html>",
                From = mailAddress
            };
            emailMessage.To.Add(new EmailMessage.MailAddress { Address = "foo@example.com" });

            //
            // Act.
            //
            var validationResult = _validator.Validate(emailMessage);

            //
            // Assert.
            //
            Assert.That(validationResult.IsValid, Is.False);
            Assert.That(validationResult.Errors.Any(o => o.PropertyName == "From.Address"), Is.True);
        }

        public IEnumerable<object[]> From_Error_TestCaseSource()
        {
            // Invalid email address.
            yield return new object[]
            {
                new EmailMessage.MailAddress
                {
                    Address = "goofy.example.com",
                    DisplayName = "Goofy Goof"
                }
            };

            // Invalid email address without a display name.
            yield return new object[]
            {
                new EmailMessage.MailAddress {Address = "http://example.com"}
            };
        }

        [TestCaseSource("No_Recipients_TestCaseSource")]
        public void No_Recipients_Is_Successful(EmailMessage emailMessage)
        {
            //
            // Act.
            //
            _validator.ShouldHaveValidationErrorFor(o => o.To, emailMessage);
        }

        public IEnumerable<object[]> No_Recipients_TestCaseSource()
        {
            // Blank email message.
            yield return new object[]
            {
                new EmailMessage()
            };

            // Invalid email address.
            yield return new object[]
            {
                new EmailMessage {From = new EmailMessage.MailAddress {Address = "foo@example.com"}}
            };
        }

        [TestCaseSource("Recipients_TestCaseSource")]
        public void Recipients_Is_Successful(EmailMessage emailMessage)
        {
            //
            // Act.
            //
            _validator.ShouldNotHaveValidationErrorFor(o => o.To, emailMessage);
        }

        public IEnumerable<object[]> Recipients_TestCaseSource()
        {
            // Has To.
            yield return new object[]
            {
                new EmailMessage
                {
                    To =
                        new List<EmailMessage.MailAddress>
                        {
                            new EmailMessage.MailAddress {Address = "foo@example.com"}
                        }
                }
            };

            // Has Cc.
            yield return new object[]
            {
                new EmailMessage
                {
                    Cc =
                        new List<EmailMessage.MailAddress>
                        {
                            new EmailMessage.MailAddress {Address = "foo@example.com"}
                        }
                }
            };

            // Has Bcc.
            yield return new object[]
            {
                new EmailMessage
                {
                    Bcc =
                        new List<EmailMessage.MailAddress>
                        {
                            new EmailMessage.MailAddress {Address = "foo@example.com"}
                        }
                }
            };
        }
    }
}