using MauiEmail.Models;
using MauiEmail.Services;
using MimeKit;
using MauiEmail.Model.Interfaces;

namespace MauiEmail.Views;

public partial class WritePage : ContentPage
{
	private ObservableMessage _observableMessage { get; set; }
    private IEmailService _emailService;
    private string emailAddress { get;set; }

	public WritePage(IEmailService emailService)
	{
		InitializeComponent();
        _emailService = emailService;
		_observableMessage = new ObservableMessage(InboxPage.ConfigureMail());
	}

    private void To_TextChanged(object sender, TextChangedEventArgs e)
    {
        emailAddress = e.NewTextValue;        
    }

    private void Subject_TextChanged(object sender, TextChangedEventArgs e)
    {
        _observableMessage.Subject = e.NewTextValue;
    }

    private void Body_TextChanged(object sender, TextChangedEventArgs e)
    {
        _observableMessage.Body = e.NewTextValue;
    }

    private async void Send_Button_Clicked(object sender, EventArgs e)
    {
        _observableMessage.To.Add(new MailboxAddress("Vince McTosh", emailAddress));
        MimeMessage msg = _observableMessage.ToMime();
        await _emailService.SendMessageAsync(msg);
    }
}