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
        public UniqueId UniqueId { get; set; }
        public DateTimeOffset Date { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string HtmlBody { get; set; }
        public MailboxAddress From { get; set; }
        public List<MailboxAddress> To { get; set; }
        public bool IsRead { get; set; }
        public bool IsFavorite { get; set; }

        public ObservableMessage(IMessageSummary message)
        {
            UniqueId = message.UniqueId;
            Date = message.Date;
            Subject = message.NormalizedSubject;
            Body = message.PreviewText;
            //HtmlBody = message.HtmlBody.ToString();
            From = (MailboxAddress)message.Envelope.From[0];            
            //To = (MailboxAddress)message.Envelope.To;           
            IsRead = (message.Flags == MessageFlags.Seen);
            IsFavorite = false;
        }

        public ObservableMessage(MimeMessage mimeMessage, UniqueId uniqueId)
        {
            UniqueId = uniqueId;
            Date = mimeMessage.Date;
            Subject = mimeMessage.Subject;
            Body = mimeMessage.Body.ToString();
            //HtmlBody = mimeMessage.HtmlBody.ToString();
            From = (MailboxAddress)mimeMessage.From[0];
            //To = (MailboxAddress)mimeMessage.To;
            HtmlBody = mimeMessage.HtmlBody;
            IsRead = false;
            IsFavorite = false;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Converts ObservableMessage to Mime
        /// </summary>
        /// <returns>An instance of MimeMessage</returns>
        public MimeMessage ToMime()
        {
            MimeMessage mimeMessage = new MimeMessage();
            mimeMessage.Date = Date;
            mimeMessage.Subject = Subject;
            //mimeMessage.Body = Body as MimeEntity;
            //mimeMessage.From = (MimeMessage)From;    
            return mimeMessage;
        }

        public EmailMessage GetForward()
        {
            return null;
        }       
    }
}
