using System.Collections.Generic;
using System.Linq;
using FluentValidation;

namespace VisualProduct.FluentEmailTemplates.Validators
{
    public class EmailMessageValidator : AbstractValidator<EmailMessage>
    {
        public EmailMessageValidator()
        {
            RuleFor(o => o.Subject)
                .NotEmpty()
                .WithMessage("Subject is a required field.");

            RuleFor(o => o.HtmlBody)
                .NotEmpty()
                .WithMessage("Html body is a required field.");

            var mailAddressValidator = new MailAddressValidator();

            RuleFor(o => o.From)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotNull()
                .WithMessage("From email address is required.")
                .SetValidator(mailAddressValidator);

            RuleFor(o => o.To)
                .SetCollectionValidator(mailAddressValidator);

            RuleFor(o => o.Cc)
                .SetCollectionValidator(mailAddressValidator);

            RuleFor(o => o.Bcc)
                .SetCollectionValidator(mailAddressValidator);

            RuleFor(o => o.To)
                .Must(HaveAtLeastOneRecipient)
                .WithMessage("The email message must have at least one recipient: To, Cc or Bcc.");
        }

        private static bool HaveAtLeastOneRecipient(EmailMessage emailMessage, List<EmailMessage.MailAddress> to)
        {
            return
                emailMessage.To.Any() ||
                emailMessage.Cc.Any() ||
                emailMessage.Bcc.Any()
                ;
        }
    }
}