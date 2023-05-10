using System.Drawing;
using System.Reflection.Emit;

using Xamarin.Essentials;
using Xamarin.Forms.Xaml;
using Xamarin.Forms;
using System;
using System.Net.NetworkInformation;

namespace MyWebApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WebView : ContentPage
    {
        public WebView()
        {
            InitializeComponent();

            // Check internet availability
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                // Check that our web target is accessible
                var sbUcenieUrl = "https://ucenie.smartbooks.sk";
                var webUri = new System.Uri(sbUcenieUrl);

                if (PingHost(sbUcenieUrl))
                {
                    // Show the BL content in the web view
                    TheWebView.Source = sbUcenieUrl;
                }
                else
                {
                    // Web site is not accessible
                    TheWebView.Source = "<html><body><h1>Server SmartBooks je nedostupný.</h1></body></html>";
                }
            }
            else
            {
                // Display no internet connection error
                var errorMessage = "<html><body><h1>Chyba: Nie je k dispozícii pripojenie k internetu.</h1></body></html>";
                var errorUrl = "https://www.google.com/";
                TheWebView.Source = errorMessage;
                NavigationPage.SetTitleView(this, new Label { Text = "Chyba: Nie je k dispozícii pripojenie k internetu.", TextColor = Color.White });
                ToolbarItems.Add(new ToolbarItem("Info", "", async () => await Browser.OpenAsync(errorUrl, BrowserLaunchMode.SystemPreferred)));
            }
        }

        private bool PingHost(string nameOrAddress, int timeout = 1000, int count = 1)
        {
            using (var pinger = new Ping())
            {
                for (int i = 0; i < count; i++)
                {
                    try
                    {
                        var reply = pinger.Send(nameOrAddress, timeout);
                        if (reply.Status == IPStatus.Success)
                        {
                            return true;
                        }
                    }
                    catch (PingException)
                    {
                        // Failed to ping the URL, or internet connection not available
                        return false;
                    }
                }
            }

            return false;
        }
    }
}