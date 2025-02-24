using MailKit;
using MimeKit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MailKit;

namespace MauiEmail.Models
{
    public class ObservableMessage : INotifyPropertyChanged
    {
        UniqueId UniqueId { get; set; }
        DateTimeOffset Date { get; set; }
        string Subject { get; set; }
        string Body { get; set; }
        string HtmlBody { get; set; }
        MailboxAddress From { get; set; }
        List<MailboxAddress> To { get; set; }
        bool IsRead { get; set; }


        public ObservableMessage(IMessageSummary message)
        {
            IsRead = (message.Flags == MessageFlags.Seen);
            Body = null;
            HtmlBody = null;
            From = (MailboxAddress)message.Envelope.From[0];

        }

        public ObservableMessage(MimeMessage mimeMessage, UniqueId uniqueId)
        {
            UniqueId = uniqueId;
            IsRead = false;
            Body = mimeMessage.Body.ToString();
            HtmlBody = mimeMessage.HtmlBody;
            Date = mimeMessage.Date;
            Subject = mimeMessage.Subject;           
        }

        public void ToMime()
        {
            
        }

        public EmailMessage GetForward()
        {
            return null;
        }

        public event PropertyChangedEventHandler? PropertyChanged;        
    }
}
