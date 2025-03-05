using MauiEmail.Models;

namespace MauiEmail.Views;

public partial class ReadPage : ContentView
{
	public ReadPage(ObservableMessage observableMessage)
	{
		InitializeComponent();
		BindingContext = observableMessage;
	}
}