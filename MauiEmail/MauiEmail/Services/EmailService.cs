using EmailConsoleApp.Interfaces;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using MailKit.Net.Imap;
using MailKit.Net.Smtp;
using MimeKit;





namespace MauiEmail.Services
{
    public class EmailService : IEmailService
    {
        private IMailConfig mailConfig;
        private ImapClient imapClient;
        private SmtpClient smtpClient;


        /// <summary>
        /// Console email service app
        /// </summary>
        /// <param name="mailConfig">Current configurations of this service</param>
        public EmailService(IMailConfig mailConfig)
        {
            this.mailConfig = mailConfig;
            imapClient = new ImapClient();
            smtpClient = new SmtpClient();
        }

        /// <summary>
        /// Deletes a message from the user's inbox
        /// </summary>
        /// <param name="uid">Uid of the email</param>
        /// <returns></returns>
        /// <exception cref="Exception">Thrown when any errors arise</exception>
        public async Task DeleteMessageAsync(int uid)
        {
            var folder = imapClient.Inbox;

            try
            {
                Console.WriteLine("Deleting email....");
                await folder.OpenAsync(FolderAccess.ReadWrite);

                await folder.StoreAsync(uid - 1, new StoreFlagsRequest(StoreAction.Add, MessageFlags.Deleted) { Silent = true });
                await folder.ExpungeAsync();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Retrieves all the emails asynchronously
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<MimeMessage>> DownloadAllEmailsAsync()
        {
            List<MimeMessage> messages = new List<MimeMessage>();

            var inbox = imapClient.Inbox;
            await inbox.OpenAsync(FolderAccess.ReadOnly);


            for (int i = 0; i < inbox.Count; i++)
            {
                var message = await inbox.GetMessageAsync(i);
                messages.Add(message);
            }

            return messages;
        }

        /// <summary>
        /// Sends a message to the client
        /// </summary>
        /// <param name="message">Message containing the subject, date, body etc....</param>
        /// <returns></returns>
        /// <exception cref="Exception">Thrown when there were some issues with authentication</exception>
        public async Task SendMessageAsync(MimeMessage message)
        {
            try
            {
                message.From.Add(new MailboxAddress("Vince McMahon", mailConfig.EmailAddress));
                message.To.Add(new MailboxAddress("Vince Mctosh", mailConfig.EmailAddress));

                if (smtpClient.IsConnected && smtpClient.IsAuthenticated)
                {
                    Console.WriteLine("Sending Email....");
                    await smtpClient.SendAsync(message);
                }
                else
                {
                    throw new Exception("Receipient authentication or connection failed!");
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        /// <summary>
        /// Disconnects the receiving client session.
        /// </summary>
        /// <returns></returns>
        public async Task DisconnectRetreiveClientAsync()
        {
            await imapClient.DisconnectAsync(true);
        }

        /// <summary>
        /// Disconnects the sending client session.
        /// </summary>
        /// <returns></returns>
        public async Task DisconnectSendClientAsync()
        {
            await imapClient.DisconnectAsync(true);
        }

        /// <summary>
        /// Connect and authenticate receipiant 
        /// </summary>
        /// <returns></returns>
        public async Task StartRetreiveClientAsync()
        {
            try
            {
                await imapClient.ConnectAsync(mailConfig.ReceiveHost, mailConfig.ReceivePort, mailConfig.RecieveSocketOptions);
                await imapClient.AuthenticateAsync(mailConfig.EmailAddress, mailConfig.Password);
            }
            catch (Exception e)
            {
                throw new Exception($"IMAP connection error: {e.Message}");
            }

        }

        /// <summary>
        /// Connect and authentical sender
        /// </summary>
        /// <returns></returns>
        public async Task StartSendClientAsync()
        {
            try
            {
                await smtpClient.ConnectAsync(mailConfig.SendHost, mailConfig.SendPort, mailConfig.SendSocketOption);
                await smtpClient.AuthenticateAsync(mailConfig.EmailAddress, mailConfig.Password);
            }
            catch (Exception e)
            {
                throw new Exception($"SMTP connection error: {e.Message}");
            }
        }
    }
}
