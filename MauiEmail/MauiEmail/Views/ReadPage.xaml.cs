using MauiEmail.Models;

namespace MauiEmail.Views;

public partial class ReadPage : ContentView
{
	private ObservableMessage _observableMessage { get; set; }

	public ReadPage(ObservableMessage observableMessage = null)
	{
		InitializeComponent();
		BindingContext = observableMessage;
	}

}