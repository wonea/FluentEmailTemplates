using System.Collections.Generic;

namespace VisualProduct.FluentEmailTemplates
{
    public class EmailMessage
    {
        public EmailMessage()
        {
            Bcc = new List<MailAddress>();
            Cc = new List<MailAddress>();
            To = new List<MailAddress>();
        }

        public class MailAddress
        {
            public string DisplayName { get; set; }
            public string Address { get; set; }
        }

        public List<MailAddress> Bcc { get; set; }
        public List<MailAddress> Cc { get; set; }
        public MailAddress From { get; set; }
        public string HtmlBody { get; set; }
        public string Subject { get; set; }
        public List<MailAddress> To { get; set; }
    }
}