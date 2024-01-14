using anilibria.Common;
using anilibria.Exceptions;
using anilibria.Models;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.ObjectModel;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace anilibria.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ReleasePage : Page
    {
        internal Release release;
        internal ObservableCollection<Episode> Episodes = new();

        public ReleasePage()
        {
            InitializeComponent();
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
                    TitleNameEn.Text = release.Names.En;

                    TitleDescription.Text = release.Description;

                    TitleImage.Source = new BitmapImage(new Uri(release.PosterUrl));

                    FavBtn.Content = "☆ " + release.InFavStr;
                    TitleYear.Text = $"{release.Season.Year} г.";
                    TitleType.Text = release.Type.FullString;
                    TitleStatus.Text = release.Status.String;

                    foreach (var item in release.Player.List)
                    {
                        Episodes.Add(item);
                    }

                    if (release.Player.Episodes.Last > 0)
                    {
                        var rnd = new Random();
                        int viewed = rnd.Next(release.Player.Episodes.First - 1, release.Player.Episodes.Last);
                        EpisodesProgress.Value = (int)((float)viewed / release.Player.Episodes.Last * 100);
                        System.Diagnostics.Debug.WriteLine(viewed);
                        System.Diagnostics.Debug.WriteLine((int)((float)viewed / release.Player.Episodes.Last * 100));

                        if (viewed > 0)
                        {
                            EpisodesProgressText.Text = $"Просмотрено {viewed} {(viewed == 1 ? "эпизод" : (viewed < 5 ? "эпизода" : ("эпизодов")))} из {release.Player.Episodes.Last}";
                        }
                    }

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

        private void EpisodesList_ItemClick(object sender, ItemClickEventArgs e)
        {
            var ep = (Episode)e.ClickedItem;
            if (ep != null)
            {
                var app = Application.Current as App;

                app?.MWindow.NavigateToPlayerPage(new()
                {
                    Release = release,
                    Episode = ep,
                });
            }
        }
    }
}
