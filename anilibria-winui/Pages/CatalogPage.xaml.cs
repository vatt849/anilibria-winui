using anilibria.Common;
using anilibria.Exceptions;
using anilibria.Models;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Collections.Generic;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace anilibria.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CatalogPage : Page
    {
        public IncrementalObservableCollection<Release> ReleasesData { get; set; }

        private readonly Anilibria apiClient = new();

        public CatalogPage()
        {
            InitializeComponent();

            InitializeData();
        }

        const int ITEMS_PER_PAGE = 10;

        private async void InitializeData()
        {
            try
            {
                var data = await apiClient.GetReleases(ITEMS_PER_PAGE, 1);

                ReleasesData = new()
                {
                    TotalResults = data.Pagination.TotalItems,
                    PageNumber = 1,
                    LoadCallback = async () =>
                    {
                        List<Release> list = new();
                        int total = 0;

                        try
                        {
                            var data = await apiClient.GetReleases(ITEMS_PER_PAGE, ++ReleasesData.PageNumber);

                            list = data.List;
                            total = data.Pagination.TotalItems;
                        }
                        catch (ApiException ex)
                        {
                            ReleasesData.PageNumber--;

                            ErrorInfo.Title = "Api error";
                            ErrorInfo.Message = string.Format("{0} ({1})", ex.Message, ex.Code);
                            ErrorInfo.IsOpen = true;

                            CatalogList.Visibility = Visibility.Collapsed;
                        }

                        return (list, total);
                    }
                };

                foreach (var r in data.List)
                {
                    ReleasesData.Add(r);
                }

                Bindings.Update();
                CatalogList.Visibility = Visibility.Visible;
            }
            catch (ApiException ex)
            {
                ErrorInfo.Title = "Api error";
                ErrorInfo.Message = string.Format("{0} ({1})", ex.Message, ex.Code);
                ErrorInfo.IsOpen = true;

                CatalogList.Visibility = Visibility.Collapsed;
            }
        }

        private void ErrorInfo_Closing(InfoBar sender, InfoBarClosingEventArgs args)
        {
            if (args.Reason == InfoBarCloseReason.CloseButton)
            {
                _ = ReleasesData.LoadMoreItemsAsync(0);
            }
        }
    }
}
