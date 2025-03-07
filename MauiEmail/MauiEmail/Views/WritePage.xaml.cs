using MauiEmail.Models;

namespace MauiEmail.Views;

public partial class WritePage : ContentPage
{
	private ObservableMessage _observableMessage { get; set; }

	public WritePage()
	{
		InitializeComponent();
	}
}