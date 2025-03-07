using MailKit.Security;
using MauiEmail.Models;
using MauiEmail.Model.Interfaces;
using MauiEmail.Services;
using MauiEmail.Configs;
using MimeKit;
using System.Collections.ObjectModel;

namespace MauiEmail.Views;

public partial class ReadPage : ContentPage
{
    private ObservableMessage _observableMessage { get; set; }

    public ReadPage(ObservableMessage message)
	{
		InitializeComponent();
        _observableMessage = message;
        BindingContext = _observableMessage;
    }
}