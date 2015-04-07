using System.Net.Mail;

namespace VisualProduct.FluentEmailTemplates
{
    /// <summary>
    /// Sends an smtp message.
    /// </summary>
    public interface ISmtpSender
    {
        /// <summary>
        /// Sends the mail message.
        /// </summary>
        void Send(MailMessage mailMessage);
    }
}