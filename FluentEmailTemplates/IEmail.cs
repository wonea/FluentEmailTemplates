using FluentValidation.Results;
using Newtonsoft.Json;

namespace VisualProduct.FluentEmailTemplates
{
    public interface IEmail
    {
        /// <summary>
        /// BCC this email address.
        /// </summary>
        /// <param name="emailAddress">The email address of the BCC recipient.</param>
        /// <param name="displayName">The optional display name of the recipient.</param>
        IEmail Bcc(string emailAddress, string displayName = null);

        /// <summary>
        /// CC this email address.
        /// </summary>
        /// <param name="emailAddress">The email address of the CC recipient.</param>
        /// <param name="displayName">The optional display name of the recipient.</param>
        IEmail Cc(string emailAddress, string displayName = null);

        /// <summary>
        /// Send the email from this email address.
        /// </summary>
        /// <param name="emailAddress">The email address of the sender.</param>
        /// <param name="displayName">The optional display name of the sender.</param>
        IEmail From(string emailAddress, string displayName = null);

        /// <summary>
        /// Get the email message as it has been built so far.
        /// </summary>
        EmailMessage GetEmailMessage();

        /// <summary>
        /// Get the email message as it has been built so far in json format.
        /// </summary>
        /// <param name="formatting">The json formatting.</param>
        string GetEmailMessageJson(Formatting formatting = Formatting.Indented);

        /// <summary>
        /// Load the email message from a file.
        /// </summary>
        /// <param name="filePath">The file path of the previously saved email message.</param>
        IEmail Load(string filePath);

        /// <summary>
        /// Save the email message to a file.
        /// </summary>
        /// <param name="filePath">The file path to save the email message.</param>
        IEmail Save(string filePath);

        /// <summary>
        /// Send this message now.
        /// </summary>
        IEmail Send();

        /// <summary>
        /// Send the email to this email address.
        /// </summary>
        /// <param name="emailAddress">The email address of the TO recipient.</param>
        /// <param name="displayName">The optional display name of the recipient.</param>
        IEmail To(string emailAddress, string displayName = null);

        /// <summary>
        /// Load a previously built email message.
        /// </summary>
        /// <param name="emailMessage">The email message to use.</param>
        IEmail WithEmailMessage(EmailMessage emailMessage);

        /// <summary>
        /// Use an email template.
        /// </summary>
        /// <param name="emailTemplate">The email template to use.</param>
        IEmail WithEmailTemplate(IEmailTemplate emailTemplate);

        /// <summary>
        /// Use an email template from file.
        /// </summary>
        /// <param name="filePath">The file path to the email template.</param>
        IEmail WithEmailTemplateFromFile(string filePath);

        /// <summary>
        /// Explicitly set the html body.
        /// Overrides any previously set html body, email template and/or merge data.
        /// </summary>
        /// <param name="html">The html body to use.</param>
        IEmail WithHtmlBody(string html);

        /// <summary>
        /// Explicitly set the html body from a file.
        /// Overrides any previously set html body, email template and/or merge data.
        /// </summary>
        /// <param name="filePath">The file path to load the html body from.</param>
        IEmail WithHtmlBodyFromFile(string filePath);

        /// <summary>
        /// Apply merge data to the email template.
        /// </summary>
        /// <param name="mergeData">The merge data to use.</param>
        IEmail WithMergeData(MergeData mergeData);

        /// <summary>
        /// Explicity set the subject.
        /// Overrides any previously set subject, email template and/or merge data.
        /// </summary>
        /// <param name="subject">The subject to use.</param>
        IEmail WithSubject(string subject);

        /// <summary>
        /// Validate the email message. Validation is automatically run before sending the email.
        /// </summary>
        ValidationResult Validate();
    }
}