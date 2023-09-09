using anilibria.Models;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace anilibria.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ReleasesPage : Page
    {
        public List<Release> staticReleaseData;

        public ReleasesPage()
        {
            InitializeComponent();
            InitializeData();
        }

        private void InitializeData()
        {
            staticReleaseData = new();

            var rnd = new Random();
            List<Release> tempList = new List<Release>(
                                      Enumerable.Range(0, 1000).Select(k =>
                                        new Release
                                        {
                                            Title = "Title " + k.ToString(),
                                            ImageLocation = @"F:\UserData\Pictures\5c53eb75-a102-427b-9451-28ade1f7ddd1.png"
                                        }));

            foreach (Release r in tempList)
            {
                staticReleaseData.Add(r);
            }
        }
    }
}
