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

    private ObservableCollection<ObservableMessage> _inbox;
    private ObservableCollection<ObservableMessage> _allInboxes;

    public InboxPage()
	{
		InitializeComponent();
        try
        {
            //Task.Run(async () => { await AuthenticateBothClients(); });
            AuthenticateBothClients();
            BindingContext = this;            
        }
        catch
        {
            DisplayAlert("App Error", "There was a problem. Please try again This app will now close.","Ok");            
        }
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

    public string FirstChar
    {
        get
        {
            return _inbox.Select(s => s.From).First().ToString().Substring(0, 0);
        }
    }

    private async Task DownloadCurrentInbox()
    {
        var inboxesObservableMessage = await App.EmailService.FetchAllMessages();
        _allInboxes = new ObservableCollection<ObservableMessage>(inboxesObservableMessage);
        Inbox = new ObservableCollection<ObservableMessage>(inboxesObservableMessage);
    }



    public async Task AuthenticateBothClients()
    {
        try
        {
            await SendAndRetrieveClientStartSession();
            await DownloadCurrentInbox();
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
            await App.EmailService.StartSendClientAsync();
            await App.EmailService.StartRetreiveClientAsync();
        }
        catch (Exception e)
        {
            throw new Exception($"IMAP or SMTP client connection failed.\n\nDetails: {e.Message}");
        }
    }

    private async Task SendAndRetrieveClientDisconnect()
    {
        await App.EmailService.DisconnectSendClientAsync();
        await App.EmailService.DisconnectRetreiveClientAsync();
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
        await Navigation.PushAsync(new WritePage(App.EmailService));       
    }

    private async void Favorite_SwipeItem_Invoked(object sender, EventArgs e)
    {
        var swipe = (sender as SwipeItem);
        ObservableMessage item = swipe.BindingContext as ObservableMessage;
        item.IsFavorite = true;
        App.EmailService.MarkFavorite(item.UniqueId);
    }

    private async void Delete_SwipeItem_Invoked(object sender, EventArgs e)
    {
        var swipe = (sender as SwipeItem);
        ObservableMessage item = swipe.BindingContext as ObservableMessage;       
        await App.EmailService.DeleteMessageAsync(item.UniqueId);
        await DisplayAlert("Email Delete", $"Your email has been deleted.", "Ok");
        await UpdateView();
    }

    private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            var text = e.NewTextValue;
            var list = Inbox.Where(i => i.Subject.ToLower().Contains(text.ToLower())).Select(s => s).ToList();


            if (text == "")
            {
                Inbox = _allInboxes;
            }
            else
            {
                Inbox = new ObservableCollection<ObservableMessage>(list);
            }           
        }
        catch { }
    }

    private async Task UpdateView()
    {
        await DownloadCurrentInbox();
    }
}