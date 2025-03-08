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
    private ObservableCollection<ObservableMessage> _inbox;

	public InboxPage()
	{
		InitializeComponent();
        _emailService = new EmailService(ConfigureMail());       
        Task.Run(async ()=> { await AuthenticateBothClients(); });
        //Task.Run(async ()=> { await UpdateInbox(); });
        BindingContext = this;
    }


    public ObservableCollection<ObservableMessage> Inbox
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

    private async Task DownloadCurrentInbox()
    {
        var inboxes = await _emailService.DownloadAllEmailsAsync();
        var inboxesObservableMessage = await _emailService.FetchAllMessages();
        Inbox = new ObservableCollection<ObservableMessage>(inboxesObservableMessage);
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

    public async Task AuthenticateBothClients()
    {
        try
        {
            await SendAndRetrieveClientStartSession();
            await DownloadCurrentInbox();
            //await SendAndRetrieveClientDisconnect();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"There was an error. Please try again later.", "Ok");
        }
    }

    private async Task SendAndRetrieveClientStartSession()
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

    private async Task SendAndRetrieveClientDisconnect()
    {
        await _emailService.DisconnectSendClientAsync();
        await _emailService.DisconnectRetreiveClientAsync();
    }

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        if (sender is Grid)
        {
            Grid details = sender as Grid;
            ObservableMessage email = (ObservableMessage) details.BindingContext ;

            ViewMessage(email);
        }
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