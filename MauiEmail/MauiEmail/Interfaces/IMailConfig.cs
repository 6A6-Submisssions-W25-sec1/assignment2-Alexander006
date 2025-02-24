using MailKit.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiEmail.Model.Interfaces
{
    public interface IMailConfig
    {
        string EmailAddress { get; set; }
        string Password { get; set; }
        string ReceiveHost { get; set; }
        SecureSocketOptions RecieveSocketOptions { get; set;}
        int ReceivePort { get; set; }
        
        
        string SenderEmailAddress { get; set; }
        string SenderPassword { get; set; }
        string SendHost { get; set; }
        int SendPort { get; set; }
        SecureSocketOptions SendSocketOption { get; set; }
        
        //Optional
        string OAuth2ClientId { get; set; }
        string OAuth2ClientSecret { get; set; }
        public string OAuthRefreshToken { get; set; }
    }
}
