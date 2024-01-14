using anilibria.Common;
using anilibria.Exceptions;
using anilibria.Models;
using CommunityToolkit.WinUI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

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

        internal int ViewedEps = 0;
        internal int ViewedPercent = 0;

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

                    FavBtnText.Text = release.InFavStr;
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

                        CalcViewed(rnd.Next(release.Player.Episodes.First - 1, release.Player.Episodes.Last), true);

                        Debug.WriteLine($"episodes viewed: {ViewedEps}");
                        Debug.WriteLine($"episodes viewed percent: {ViewedPercent}");

                        if (ViewedEps > 0)
                        {
                            EpisodesProgressText.Text = $"Просмотрено {ViewedEps} {(ViewedEps == 1 ? "эпизод" : (ViewedEps < 5 ? "эпизода" : ("эпизодов")))} из {release.Player.Episodes.Last}";
                        }

                        for (int i = 0; i < ViewedEps; i++)
                        {
                            Episodes[i].Viewed = true;
                        }
                    }

                }
                catch (ApiException ex)
                {
                    ErrorInfo.Title = "Api error";
                    ErrorInfo.Message = string.Format("{0} ({1})", ex.Message, ex.Code);
                    ErrorInfo.IsOpen = true;
                    return;
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

        internal void CalcViewed(int epDelta, bool rewrite = false)
        {
            if (rewrite)
            {
                ViewedEps = 0;
            }

            ViewedEps += epDelta;
            ViewedPercent = (int)((float)ViewedEps / release.Player.Episodes.Last * 100);

            if (ViewedEps > 0)
            {
                EpisodesProgressText.Text = $"Просмотрено {ViewedEps} {(ViewedEps == 1 ? "эпизод" : (ViewedEps < 5 ? "эпизода" : "эпизодов"))} из {release.Player.Episodes.Last}";
            }
            else
            {
                EpisodesProgressText.Text = "Не просмотрено ни одного эпизода";
            }

            EpisodesProgress.Value = ViewedPercent;
        }

        private void EpisodeMarkViewed_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as MenuFlyoutItem;

            CalcViewed(1);

            int i = Episodes.IndexOf(Episodes.First(x => x.EpisodeNum.ToString() == btn.Tag.ToString()));
            Episodes[i].Viewed = true;

            if (EpisodesList.FindDescendant<FontIcon>(x => x.Name is "ViewedMark" && x.Tag.ToString() == btn.Tag.ToString()) is FontIcon viewedMark)
            {
                viewedMark.Visibility = Visibility.Visible;
            }

            Debug.WriteLine($"ep i: {i}, ep viewed: {Episodes[i].Viewed}");
        }

        private void EpisodeUnmarkViewed_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as MenuFlyoutItem;

            CalcViewed(-1);

            int i = Episodes.IndexOf(Episodes.First(x => x.EpisodeNum.ToString() == btn.Tag.ToString()));
            Episodes[i].Viewed = false;

            if (EpisodesList.FindDescendant<FontIcon>(x => x.Name is "ViewedMark" && x.Tag.ToString() == btn.Tag.ToString()) is FontIcon viewedMark)
            {
                viewedMark.Visibility = Visibility.Collapsed;
            }

            Debug.WriteLine($"ep i: {i}, ep viewed: {Episodes[i].Viewed}");
        }
    }
}
