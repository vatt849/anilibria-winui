using anilibria.Common;
using anilibria.Exceptions;
using anilibria.Models;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Collections.ObjectModel;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace anilibria.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ReleasesPage : Page
    {
        public ObservableCollection<Release> ReleasesData;
        private readonly Anilibria apiClient = new();

        public ReleasesPage()
        {
            InitializeComponent();

            ReleasesData = new();

            InitializeData();
        }

        private async void InitializeData()
        {
            try
            {
                var resp = await apiClient.GetReleases(5);

                foreach (var r in resp.List)
                {
                    ReleasesData.Add(r);
                }
                ReleasesView.Visibility = Visibility.Visible;
            }
            catch (ApiException ex)
            {
                ErrorInfo.Title = "Api error";
                ErrorInfo.Message = string.Format("{0} ({1})", ex.Message, ex.Code);
                ErrorInfo.IsOpen = true;
                ReleasesView.Visibility = Visibility.Collapsed;
            }
        }

        private void ErrorInfo_Closing(InfoBar sender, InfoBarClosingEventArgs args)
        {
            if (args.Reason == InfoBarCloseReason.CloseButton)
            {
                InitializeData();
            }
        }

        private void GoSeeBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void GoReleaseBtn_Click(object sender, RoutedEventArgs e)
        {
            var r = (Release)ReleasesView.SelectedItem;
            if (r != null)
            {
                var app = Application.Current as App;

                app?.MWindow.NavigateToTitlePage(r);
            }
        }
    }
}
