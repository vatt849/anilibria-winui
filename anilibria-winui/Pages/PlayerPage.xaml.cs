using anilibria.Pages.Helpers;
using CommunityToolkit.WinUI;
using Flurl;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Storage.Streams;
using WinRT.Interop;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace anilibria.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PlayerPage : Page
    {
        PlayerData data;
        private MediaPlaybackList playbackList;
        private Dictionary<int, int> episodesMap = new();

        private AppWindow _appWindow;

        public PlayerPage()
        {
            InitializeComponent();

            EpisodePlayer.Loaded += MediaPlayerElementConrolLoaded;
            EpisodePlayer.DoubleTapped += MediaPlayerElementConrolDoubleTapped;

            playbackList = new()
            {
                ShuffleEnabled = false
            };

            _appWindow = GetAppWindowForCurrentWindow();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is PlayerData pd && pd is not null)
            {
                data = pd;

                playbackList.Items.Clear();

                string host = new Url($"https://{data.Release.Player.Host}");

                foreach (var episode in data.Release.Player.List)
                {
                    string source = episode.HLS.FHD ?? episode.HLS.HD ?? episode.HLS.SD;

                    var sourceUri = host.AppendPathSegment(source).ToUri();

                    var mediaPlaybackItem = new MediaPlaybackItem(MediaSource.CreateFromUri(sourceUri));

                    var props = mediaPlaybackItem.GetDisplayProperties();

                    props.Type = Windows.Media.MediaPlaybackType.Video;
                    props.Thumbnail = RandomAccessStreamReference.CreateFromUri(new Uri(data.Release.ThumbnailUrl));
                    props.VideoProperties.Title = data.Release.Title;
                    props.VideoProperties.Subtitle = episode.Title;
                    foreach (var g in data.Release.Genres)
                    {
                        props.VideoProperties.Genres.Add(g);
                    }
                    mediaPlaybackItem.ApplyDisplayProperties(props);
                    playbackList.Items.Add(mediaPlaybackItem);

                    episodesMap[episode.EpisodeNum] = playbackList.Items.IndexOf(mediaPlaybackItem);
                }

                EpisodePlayer.Source = playbackList;
            }
            else
            {
                var app = Application.Current as App;

                app?.MWindow.TryGoBack();
            }

            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);

            DispatcherQueue.TryEnqueue(() => EpisodePlayer?.MediaPlayer.Dispose());
        }

        private void MediaPlayerElementConrolLoaded(object sender, RoutedEventArgs e)
        {
            if (EpisodePlayer.FindDescendant<AppBarButton>(x => x.Name is "PreviousTrackButton") is AppBarButton prevButton)
            {
                prevButton.Click -= PrevTrackButtonClick;
                prevButton.Click += PrevTrackButtonClick;

                if (data.IsEpisodeFirst)
                {
                    prevButton.IsEnabled = false;
                    EpisodePlayer.TransportControls.IsPreviousTrackButtonVisible = false;
                }
            }

            if (EpisodePlayer.FindDescendant<AppBarButton>(x => x.Name is "NextTrackButton") is AppBarButton nextButton)
            {
                nextButton.Click -= NextTrackButtonClick;
                nextButton.Click += NextTrackButtonClick;

                if (data.IsEpisodeLast)
                {
                    nextButton.IsEnabled = false;
                    EpisodePlayer.TransportControls.IsNextTrackButtonVisible = false;
                }
            }

            uint eNum = (uint)episodesMap[data.Episode.EpisodeNum];

            playbackList.MoveTo(eNum);
        }

        private void MediaPlayerElementConrolDoubleTapped(object sender, RoutedEventArgs e)
        {
            var app = Application.Current as App;

            if (_appWindow.Presenter.Kind == AppWindowPresenterKind.FullScreen)
            {
                _appWindow.SetPresenter(AppWindowPresenterKind.Default);
                EpisodePlayer.IsFullWindow = false;

                app?.MWindow.ShowInterface();
            }
            else
            {
                _appWindow.SetPresenter(AppWindowPresenterKind.FullScreen);
                EpisodePlayer.IsFullWindow = true;

                app?.MWindow.HideInterface();
            }
        }

        private void PrevTrackButtonClick(object sender, RoutedEventArgs e)
        {
            if (data.IsEpisodeFirst)
            {
                return;
            }

            var episode = data.Release.Player.List.Find(x => x.EpisodeNum == data.Episode.EpisodeNum - 1);

            if (episode == null)
            {
                return;
            }

            data.Episode = episode;

            playbackList.MovePrevious();

            if (EpisodePlayer.FindDescendant<AppBarButton>(x => x.Name is "NextTrackButton") is AppBarButton nextButton)
            {
                nextButton.IsEnabled = true;
                EpisodePlayer.TransportControls.IsNextTrackButtonVisible = true;
            }

            if (data.IsEpisodeFirst)
            {
                var prevButton = sender as AppBarButton;
                prevButton.IsEnabled = false;
                EpisodePlayer.TransportControls.IsPreviousTrackButtonVisible = false;
            }
        }

        private void NextTrackButtonClick(object sender, RoutedEventArgs e)
        {
            if (data.IsEpisodeLast)
            {
                return;
            }

            var episode = data.Release.Player.List.Find(x => x.EpisodeNum == data.Episode.EpisodeNum + 1);

            if (episode == null)
            {
                return;
            }

            data.Episode = episode;

            playbackList.MoveNext();

            if (EpisodePlayer.FindDescendant<AppBarButton>(x => x.Name is "PreviousTrackButton") is AppBarButton prevButton)
            {
                prevButton.IsEnabled = true;
                EpisodePlayer.TransportControls.IsPreviousTrackButtonVisible = true;
            }

            if (data.IsEpisodeLast)
            {
                var nextButton = sender as AppBarButton;
                nextButton.IsEnabled = false;
                EpisodePlayer.TransportControls.IsNextTrackButtonVisible = false;
            }
        }

        private AppWindow GetAppWindowForCurrentWindow()
        {
            var app = Application.Current as App;

            IntPtr hWnd = WindowNative.GetWindowHandle(app?.MWindow);
            WindowId myWndId = Win32Interop.GetWindowIdFromWindow(hWnd);
            return AppWindow.GetFromWindowId(myWndId);
        }
    }
}
