﻿using MailKit;
using MimeKit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MailKit;
using MauiEmail.Model.Interfaces;

namespace MauiEmail.Models
{
    /// <summary>
    /// Serves to alleviate unnecessary operations and improves 
    /// performance on rendering an email on the View
    /// </summary>
    public class ObservableMessage : INotifyPropertyChanged
    {
        private IMailConfig mailConfig;
        public UniqueId UniqueId { get;  }
        public DateTimeOffset Date { get; }
        public string Subject { get; set; }
        public string? Body { get; set; }
        public string? HtmlBody { get; }
        public MailboxAddress From { get; set; }
        public List<MailboxAddress> To { get; set; }
        public bool IsRead { get; set; }
        public bool IsFavorite { get; set; }

        /// <summary>
        /// ObservableMessage configuration to write an email to
        /// </summary>
        /// <param name="config"></param>
        public ObservableMessage() 
        {
            mailConfig = App.MailConfig;
            Date = DateTime.Now;
            To = new List<MailboxAddress>();
            From = new MailboxAddress("Vince McMahon", mailConfig.EmailAddress);            
        }

        /// <summary>
        /// ObservableMessage configuration to read an email using the IMessage interface
        /// </summary>
        /// <param name="message"></param>
        public ObservableMessage(IMessageSummary message)
        {
            UniqueId = message.UniqueId;
            Date = message.Date;
            Subject = message.NormalizedSubject;
            Body = message.PreviewText;
            HtmlBody = message.HtmlBody?.ToString();
            From = (MailboxAddress)message.Envelope.From[0];
            var mailboxes = message.Envelope.To.Mailboxes;
            To = new List<MailboxAddress>(mailboxes);
            IsRead = (message.Flags == MessageFlags.Seen);            
        }

        /// <summary>
        /// ObservableMessage configuration to read an email using MimeMessage
        /// </summary>
        /// <param name="mimeMessage"></param>
        /// <param name="uniqueId"></param>
        public ObservableMessage(MimeMessage mimeMessage, UniqueId uniqueId)
        {
            UniqueId = uniqueId;
            Date = mimeMessage.Date;
            Subject = mimeMessage.Subject;
            Body = mimeMessage.Body?.ToString();
            HtmlBody = mimeMessage.HtmlBody?.ToString();
            From = (MailboxAddress)mimeMessage.From[0];
            To = (List<MailboxAddress>)mimeMessage.To.Mailboxes;
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
            MailboxAddress address = new MailboxAddress(To.First().Name, To.First().Address);
            mimeMessage.Date = Date;
            mimeMessage.Subject = Subject;            
            mimeMessage.Body = new TextPart()
            {
                Text = Body
            };            
            mimeMessage.From.Add(From);
            mimeMessage.To.Add(address);
                     
            return mimeMessage;
        }

        /// <summary>
        /// Forwards an email to another user
        /// </summary>
        /// <returns></returns>
        public EmailMessage GetForward()
        {
            return null;
        }       
    }
}
