using MailKit.Security;
using MauiEmail.Models;
using MauiEmail.Model.Interfaces;
using MauiEmail.Services;
using MauiEmail.Configs;
using MimeKit;
using System.Collections.ObjectModel;

namespace MauiEmail.Views;

public partial class InboxPage : ContentPage
{
	private static IEmailService _emailService;
    private ObservableCollection<MimeMessage> _inbox;
    private List<ObservableMessage> _inbox2;

	public InboxPage()
	{
		InitializeComponent();
		_emailService = new EmailService(ConfigureMail());

        Task.Run(async ()=> { await EmailConnectionAndAuthentication(); });
        BindingContext = this;
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

    public List<ObservableMessage> Inbox2
    {
        get { return _inbox2; }
        set { _inbox2 = value; }
    }

    private async Task DownloadCurrentInbox()
    {
        var inboxes = await _emailService.DownloadAllEmailsAsync();
        var inboxes2 = await _emailService.FetchAllMessages();
        
        Inbox = new ObservableCollection<MimeMessage>(inboxes);
        Inbox2 = new List<ObservableMessage>(inboxes2);

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
    
    private async Task EmailConnectionAndAuthentication()
    {        
        try
        {
            await StartSession();
            await DownloadCurrentInbox();
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    private async Task StartSession()
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

    private async Task TerminateSession()
    {
        await _emailService.DisconnectSendClientAsync();
        await _emailService.DisconnectRetreiveClientAsync();
        Console.WriteLine("Logged out. Press any key to continue...");
        Console.ReadKey();
    }

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {

    }
}