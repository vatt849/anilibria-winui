using anilibria.Common;
using anilibria.Exceptions;
using anilibria.Models;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Navigation;
using System;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace anilibria.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TitlePage : Page
    {
        internal Release release;

        public TitlePage()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is int rID && rID > 0)
            {
                var api = new Anilibria();

                try
                {
                    release = await api.GetRelease(rID);

                    TitleName.Text = release.Title;
                    TitleDescription.Text = release.Description;
                    TitleImage.Source = new BitmapImage(new Uri(release.PosterUrl));
                }
                catch (ApiException ex)
                {
                    ErrorInfo.Title = "Api error";
                    ErrorInfo.Message = string.Format("{0} ({1})", ex.Message, ex.Code);
                    ErrorInfo.IsOpen = true;
                    return;
                    //ReleasesView.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                ErrorInfo.Title = "App error";
                ErrorInfo.Message = "Unknown release";
                ErrorInfo.IsOpen = true;
            }

            base.OnNavigatedTo(e);
        }

        private void ErrorInfo_Closing(InfoBar sender, InfoBarClosingEventArgs args)
        {
            if (args.Reason == InfoBarCloseReason.CloseButton)
            {
                var app = Application.Current as App;

                app?.MWindow.TryGoBack();
            }
        }
    }
}
