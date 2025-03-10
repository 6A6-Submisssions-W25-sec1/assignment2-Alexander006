using MailKit.Security;
using MauiEmail.Configs;
using MauiEmail.Model.Interfaces;
using MauiEmail.Models;
using MauiEmail.Services;

namespace MauiEmail
{
    public partial class App : Application
    {
        public static IMailConfig MailConfig;
        public static IEmailService EmailService;

        public App()
        {
            InitializeComponent();
            MailConfig = ConfigureMail();
            EmailService = new EmailService(MailConfig);
            MainPage = new AppShell();
        }

        private static IMailConfig ConfigureMail()
        {
            IMailConfig mailConfig = new MailConfig();

            //Email address used
            mailConfig.EmailAddress = "vmcmahon688@gmail.com";
            mailConfig.Password = "mofk jbuz jlpz twfc";

            //Receive from client (To User "Cool Gamer")
            mailConfig.ReceiveHost = "imap.gmail.com";
            mailConfig.RecieveSocketOptions = SecureSocketOptions.SslOnConnect;
            mailConfig.ReceivePort = 993;

            //Send to (From this console as Cool Gamer)
            mailConfig.SendHost = "smtp.gmail.com";
            mailConfig.SendSocketOption = SecureSocketOptions.StartTls;
            mailConfig.SendPort = 587;

            //Real Email
            mailConfig.SenderEmailAddress = "aburlecp@gmail.com";
            mailConfig.SenderPassword = "sxxb gdei ddmf unzp";

            return mailConfig;
        }
    }
}
