using anilibria.Common;
using anilibria.Models;
using Microsoft.UI.Xaml.Controls;
using System.Collections.ObjectModel;
using System.Text.Json;
using System.Threading.Tasks;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace anilibria.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ReleasesPage : Page
    {
        public ObservableCollection<Release> releasesData;

        public ReleasesPage()
        {
            InitializeComponent();

            releasesData = new();

            InitializeData();
        }

        private async Task InitializeData()
        {
            var svc = new HttpService();
            string data = await svc.GetAsync(@"https://api.anilibria.tv/v3/title/updates?limit=20&filter=id,code,names,posters,genres,description");

            UpdatesResponse resp = JsonSerializer.Deserialize<UpdatesResponse>(data);

            foreach (var r in resp.List)
            {
                releasesData.Add(r);
            }
        }
    }
}
