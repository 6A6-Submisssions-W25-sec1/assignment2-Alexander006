using MauiEmail.Model;
using MailKit.Net.Imap;
using MailKit.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using MauiEmail.Model.Interfaces;

namespace MauiEmail.Model
{
    public class MailConfig : IMailConfig
    {
        /// <summary>
        /// Email configuration service
        /// </summary>
        public MailConfig() { }

        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public string ReceiveHost { get; set; }
        public SecureSocketOptions RecieveSocketOptions { get; set; }
        public int ReceivePort { get; set; }
        public string SendHost { get; set; }
        public int SendPort { get; set; }
        public SecureSocketOptions SendSocketOption { get; set; }
        public string OAuth2ClientId { get; set; }
        public string OAuth2ClientSecret { get ; set; }
        public string OAuthRefreshToken { get ; set; }
        public string SenderEmailAddress { get ; set; }
        public string SenderPassword { get; set; }
    }
}
