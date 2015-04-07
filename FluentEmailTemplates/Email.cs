using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using FluentValidation;
using FluentValidation.Results;
using Newtonsoft.Json;
using VisualProduct.FluentEmailTemplates.Parts;
using VisualProduct.FluentEmailTemplates.Validators;

namespace VisualProduct.FluentEmailTemplates
{
    public class Email : IEmail
    {
        private readonly ISmtpSender _smtpSender;
        private EmailMessage _emailMessage = new EmailMessage();
        private IEmailTemplate _emailTemplate;
        private MergeData _mergeData;

        /// <summary>
        /// Default constructor uses production ready SmtpSender.
        /// </summary>
        public Email()
            : this(new SmtpSender())
        {
        }

        /// <summary>
        /// Constructor to pass in your own implementation of ISmtpSender.
        /// </summary>
        /// <param name="smtpSender"></param>
        public Email(ISmtpSender smtpSender)
        {
            _smtpSender = smtpSender;
        }

        /// <summary>
        /// BCC this email address.
        /// </summary>
        /// <param name="emailAddress">The email address of the BCC recipient.</param>
        /// <param name="displayName">The optional display name of the recipient.</param>
        public IEmail Bcc(string emailAddress, string displayName = null)
        {
            _emailMessage.Bcc.Add(new EmailMessage.MailAddress {Address = emailAddress, DisplayName = displayName});
            return this;
        }

        /// <summary>
        /// CC this email address.
        /// </summary>
        /// <param name="emailAddress">The email address of the CC recipient.</param>
        /// <param name="displayName">The optional display name of the recipient.</param>
        public IEmail Cc(string emailAddress, string displayName = null)
        {
            _emailMessage.Cc.Add(new EmailMessage.MailAddress { Address = emailAddress, DisplayName = displayName });
            return this;
        }

        /// <summary>
        /// Send the email from this email address.
        /// </summary>
        /// <param name="emailAddress">The email address of the sender.</param>
        /// <param name="displayName">The optional display name of the sender.</param>
        public IEmail From(string emailAddress, string displayName = null)
        {
            _emailMessage.From = new EmailMessage.MailAddress {Address = emailAddress, DisplayName = displayName};
            return this;
        }

        /// <summary>
        /// Get the email message as it has been built so far.
        /// </summary>
        public EmailMessage GetEmailMessage()
        {
            return _emailMessage;
        }

        /// <summary>
        /// Get the email message as it has been built so far in json format.
        /// </summary>
        public string GetEmailMessageJson(Formatting formatting = Formatting.Indented)
        {
            var json = JsonConvert.SerializeObject(_emailMessage, formatting);
            return json;
        }

        /// <summary>
        /// Get the smtp mail message object from the email message for sending by the smtp sender.
        /// </summary>
        private MailMessage GetSmtpMailMessage()
        {
            Action<MailAddressCollection, IEnumerable<EmailMessage.MailAddress>> addRecipient = (destination, source) =>
            {
                foreach (var recipient in source)
                {
                    destination.Add(new MailAddress(recipient.Address, recipient.DisplayName));
                }
            };

            var msg = new MailMessage();
            addRecipient(msg.Bcc, _emailMessage.Bcc);
            addRecipient(msg.CC, _emailMessage.Cc);
            addRecipient(msg.To, _emailMessage.To);
            msg.Body = _emailMessage.HtmlBody;
            msg.From = new MailAddress(_emailMessage.From.Address, _emailMessage.From.DisplayName);
            msg.IsBodyHtml = true;
            msg.Subject = _emailMessage.Subject;
            return msg;
        }

        /// <summary>
        /// Load the email message from a file.
        /// </summary>
        /// <param name="filePath">The file path of the previously saved email message.</param>
        public IEmail Load(string filePath)
        {
            var json = File.ReadAllText(filePath);
            _emailMessage = JsonConvert.DeserializeObject<EmailMessage>(json);
            return this;
        }

        /// <summary>
        /// Merges the merge data into the email template if both exist.
        /// </summary>
        private void Merge()
        {
            if (_emailTemplate == null || _mergeData == null)
            {
                // Need the email template AND the merge data to be loaded before we can merge the two.
                // Not ready yet.
                return;
            }

            if (_emailTemplate.Subject != null)
            {
                _emailMessage.Subject = _emailTemplate.Subject.GetString(_mergeData);
            }

            if (_emailTemplate.HtmlBody != null)
            {
                _emailMessage.HtmlBody = _emailTemplate.HtmlBody.GetHtml(_mergeData);
            }
        }

        /// <summary>
        /// Save the email message to a file.
        /// </summary>
        /// <param name="filePath">The file path to save the email message.</param>
        public IEmail Save(string filePath)
        {
            var json = GetEmailMessageJson();
            File.WriteAllText(filePath, json);
            return this;
        }

        /// <summary>
        /// Send this message now.
        /// </summary>
        public IEmail Send()
        {
            // Validate the email message first.
            var validationResult = Validate();
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            // Get the MailMessage.
            var msg = GetSmtpMailMessage();

            _smtpSender.Send(msg);

            return this;
        }

        /// <summary>
        /// Send the email to this email address.
        /// </summary>
        /// <param name="emailAddress">The email address of the TO recipient.</param>
        /// <param name="displayName">The optional display name of the recipient.</param>
        public IEmail To(string emailAddress, string displayName = null)
        {
            _emailMessage.To.Add(new EmailMessage.MailAddress { Address = emailAddress, DisplayName = displayName });
            return this;
        }

        /// <summary>
        /// Load a previously built email message.
        /// </summary>
        /// <param name="emailMessage">The email message to use.</param>
        public IEmail WithEmailMessage(EmailMessage emailMessage)
        {
            _emailMessage = emailMessage;
            return this;
        }

        /// <summary>
        /// Use an email template.
        /// </summary>
        /// <param name="emailTemplate">The email template to use.</param>
        public IEmail WithEmailTemplate(IEmailTemplate emailTemplate)
        {
            _emailTemplate = emailTemplate;

            Merge();

            return this;
        }

        /// <summary>
        /// Use an email template from file.
        /// </summary>
        /// <param name="filePath">The file path to the email template.</param>
        public IEmail WithEmailTemplateFromFile(string filePath)
        {
            var partSelector = new PartSelector();
            var emailTemplate = new EmailTemplate(partSelector);
            emailTemplate.Load(filePath);
            return WithEmailTemplate(emailTemplate);
        }

        /// <summary>
        /// Explicitly set the html body.
        /// Overrides any previously set html body, email template and/or merge data.
        /// </summary>
        /// <param name="html">The html body to use.</param>
        public IEmail WithHtmlBody(string html)
        {
            _emailMessage.HtmlBody = html;
            return this;
        }

        /// <summary>
        /// Explicitly set the html body from a file.
        /// Overrides any previously set html body, email template and/or merge data.
        /// </summary>
        /// <param name="filePath">The file path to load the html body from.</param>
        public IEmail WithHtmlBodyFromFile(string filePath)
        {
            var html = File.ReadAllText(filePath);
            return WithHtmlBody(html);
        }

        /// <summary>
        /// Apply merge data to the email template.
        /// </summary>
        /// <param name="mergeData">The merge data to use.</param>
        public IEmail WithMergeData(MergeData mergeData)
        {
            _mergeData = mergeData;

            Merge();

            return this;
        }

        /// <summary>
        /// Explicity set the subject.
        /// Overrides any previously set subject, email template and/or merge data.
        /// </summary>
        /// <param name="subject">The subject to use.</param>
        public IEmail WithSubject(string subject)
        {
            _emailMessage.Subject = subject;
            return this;
        }

        /// <summary>
        /// Validate the email message. Validation is automatically run before sending the email.
        /// </summary>
        public ValidationResult Validate()
        {
            var validator = new EmailMessageValidator();
            var validationResult = validator.Validate(_emailMessage);
            return validationResult;
        }
    }
}