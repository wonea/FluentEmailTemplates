using FluentValidation;

namespace VisualProduct.FluentEmailTemplates.Validators
{
    /// <summary>
    /// Validator for mail address.
    /// </summary>
    public class MailAddressValidator : AbstractValidator<EmailMessage.MailAddress>
    {
        public MailAddressValidator()
        {
            RuleFor(o => o.Address)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty()
                .WithMessage("Email address is a required.")
                .EmailAddress()
                .WithMessage("Invalid email address.");
        }
    }
}