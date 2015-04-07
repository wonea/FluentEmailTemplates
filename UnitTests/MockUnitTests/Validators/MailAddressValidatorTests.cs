using System.Collections.Generic;
using FluentValidation.TestHelper;
using NUnit.Framework;
using VisualProduct.FluentEmailTemplates.Validators;

namespace VisualProduct.FluentEmailTemplates.UnitTests.MockUnitTests.Validators
{
    [TestFixture]
    public class MailAddressValidatorTests
    {
        private MailAddressValidator _validator;

        [SetUp]
        public void SetUp()
        {
            _validator = new MailAddressValidator();
        }

        [TestCaseSource("From_Error_TestCaseSource")]
        public void From_Error_Is_Successful(EmailMessage.MailAddress mailAddress)
        {
            //
            // Act.
            //
            _validator.ShouldHaveValidationErrorFor(o => o.Address, mailAddress);
        }

        public IEnumerable<object[]> From_Error_TestCaseSource()
        {
            // Null mail address.
            yield return new object[]
            {
                new EmailMessage.MailAddress()
            };

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
                new EmailMessage.MailAddress { Address = "http://example.com" }
            };
        }
    }
}