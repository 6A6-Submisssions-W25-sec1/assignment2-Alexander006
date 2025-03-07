using MauiEmail.Models;

namespace MauiEmail.Views;

public partial class WritePage : ContentPage
{
	private ObservableMessage _observableMessage { get; set; }

	public WritePage()
	{
		InitializeComponent();
		_observableMessage = new ObservableMessage(null);
	}

    private void To_TextChanged(object sender, TextChangedEventArgs e)
    {
        _observableMessage.To = null;
    }

    private void Subject_TextChanged(object sender, TextChangedEventArgs e)
    {
        _observableMessage.Subject = null;
    }

    private void Body_TextChanged(object sender, TextChangedEventArgs e)
    {
        _observableMessage.Body = null;
    }

    private void SendEmail()
    {

    }
}