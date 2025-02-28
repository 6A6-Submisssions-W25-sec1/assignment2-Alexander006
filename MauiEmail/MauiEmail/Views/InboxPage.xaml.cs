using MailKit.Security;
using MauiEmail.Model;
using MauiEmail.Model.Interfaces;
using MauiEmail.Services;
using MimeKit;
using System.Collections.ObjectModel;

namespace MauiEmail.Views;

public partial class InboxPage : ContentPage
{
	private static IEmailService _emailService;
    private ObservableCollection<MimeMessage> _inbox;

	public InboxPage()
	{
		InitializeComponent();
		_emailService = new EmailService(ConfigureMail());
        BindingContext = this;

        Task.Run(async ()=> { await EmailConnectionAndAuthentication(); });
        Task.Run(async () => { await DownloadCurrentInbox(); });
	}

    public ObservableCollection<MimeMessage> Inbox
    {
        get
        {
            return _inbox;
        }
        set 
        { 
            _inbox = value;
            OnPropertyChanged(nameof(Inbox));
        }
    }

    private async Task DownloadCurrentInbox()
    {
         Inbox = (ObservableCollection<MimeMessage>)await _emailService.DownloadAllEmailsAsync();
    }

	private IMailConfig ConfigureMail()
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
    
    private async static Task EmailConnectionAndAuthentication()
    {
        Console.WriteLine("Connecting and Authenticating....");
        try
        {
            await StartSession();
        }
        catch (Exception e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\n-----CONNECTION AND AUTHENTICATION FAILED-----\n");
            Thread.Sleep(2000);
            Console.ForegroundColor = ConsoleColor.White;
            throw new Exception(e.Message);
        }

        Console.ForegroundColor = ConsoleColor.DarkGreen;
        Console.WriteLine("\n-----CONNECTION AND AUTHENTICATION SUCCESSFUL-----\n");
        Thread.Sleep(1000);
        Console.ForegroundColor = ConsoleColor.White;
        Console.Clear();
    }

    private async static Task StartSession()
    {
        try
        {
            await _emailService.StartSendClientAsync();
            await _emailService.StartRetreiveClientAsync();
        }
        catch (Exception e)
        {
            throw new Exception($"IMAP or SMTP client connection failed.\n\nDetails: {e.Message}");
        }
    }

    private async static Task TerminateSession()
    {
        await _emailService.DisconnectSendClientAsync();
        await _emailService.DisconnectRetreiveClientAsync();
        Console.WriteLine("Logged out. Press any key to continue...");
        Console.ReadKey();
    }
}