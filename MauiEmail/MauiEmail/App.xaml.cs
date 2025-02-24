using MauiEmail.Models;

namespace MauiEmail
{
    public partial class App : Application
    {
        //public static EmailConfig emailConfig;
        //public static EmailService emailService;

        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }
    }
}
