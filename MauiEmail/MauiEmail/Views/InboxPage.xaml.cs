using MailKit.Security;
using MauiEmail.Configs;
using MauiEmail.Model.Interfaces;
using MauiEmail.Models;
using MauiEmail.Services;
using MimeKit;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace MauiEmail.Views;

public partial class InboxPage : ContentPage, INotifyPropertyChanged
{
    private IEmailService _emailService;
    private ObservableMessage _observableMessage;
    private ObservableCollection<MimeMessage> _inbox;
    private List<ObservableMessage> _inbox2;
    private ObservableCollection<ObservableMessage> _inbox3;

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
        }
    }

    public List<ObservableMessage> Inbox2
    {
        get 
        { 
            return _inbox2; 
        }
        set 
        { 
            _inbox2 = value;        
        }
    }

    public ObservableCollection<ObservableMessage> Inbox3
    {
        get 
        {             
            return _inbox3;        
        }
        set 
        { 
            _inbox3 = value; 
        }
    }

    private async Task DownloadCurrentInbox()
    {
        var inboxes = await _emailService.DownloadAllEmailsAsync();
        var inboxesObservableMessage = await _emailService.FetchAllMessages();
        
        Inbox = new ObservableCollection<MimeMessage>(inboxes);
        Inbox2 = new List<ObservableMessage>(inboxesObservableMessage);
        Inbox3 = new ObservableCollection<ObservableMessage>(inboxesObservableMessage);
    }

	public static IMailConfig ConfigureMail()
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

        ViewMessage(_inbox3[0]);
    }

    private async void ViewMessage(ObservableMessage message)
    {
       await Navigation.PushAsync(new ReadPage(message));
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new WritePage(_emailService));
    }
}