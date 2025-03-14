using MauiEmail.Models;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Net.Smtp;

namespace MauiEmail.Model.Interfaces
{
    public interface IEmailService
    {
        // Connection and authentication
        Task StartSendClientAsync();
        Task DisconnectSendClientAsync();
        Task StartRetreiveClientAsync();
        Task DisconnectRetreiveClientAsync();

        // Emailing functionality and Observable messages
        Task SendMessageAsync(MimeMessage message);
        Task<IEnumerable<ObservableMessage>?> FetchAllMessages();
        ObservableMessage DownloadMessage(ObservableMessage message);
        Task DeleteMessageAsync(UniqueId uniqueId);
        Task<IEnumerable<ObservableMessage>?> SearchMessageAsync(string content);

        //Flags
        public void MarkRead(UniqueId uniqueId);
        public void MarkFavorite(UniqueId uniqueId);
    }
}
