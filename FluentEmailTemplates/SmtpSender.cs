using System.Net.Mail;

namespace VisualProduct.FluentEmailTemplates
{
    /// <summary>
    /// Sends an smtp mail message.
    /// </summary>
    public class SmtpSender : ISmtpSender
    {
        /// <summary>
        /// Sends the mail message.
        /// </summary>
        public void Send(MailMessage mailMessage)
        {
            using (var smtpClient = new SmtpClient())
            {
                // Make sure your app.config file has the correct mail settings.
                // See documentation at https://github.com/chchmatt/FluentEmailTemplates
                smtpClient.Send(mailMessage);
            }
        }
    }
}