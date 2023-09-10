using anilibria.Pages;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Linq;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace anilibria
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            ExtendsContentIntoTitleBar = true;
            SetTitleBar(AppTitleBar);
        }

        private void MainNav_BackRequested(NavigationView sender,
                                   NavigationViewBackRequestedEventArgs args)
        {
            TryGoBack();
        }

        private bool TryGoBack()
        {
            if (!ContentFrame.CanGoBack)
                return false;

            // Don't go back if the nav pane is overlayed.
            if (MainNav.IsPaneOpen &&
                (MainNav.DisplayMode == NavigationViewDisplayMode.Compact ||
                 MainNav.DisplayMode == NavigationViewDisplayMode.Minimal))
                return false;

            ContentFrame.GoBack();
            return true;
        }

        private void MainNav_Loaded(object sender, RoutedEventArgs e)
        {
            // Add handler for ContentFrame navigation.
            ContentFrame.Navigated += On_Navigated;

            // MainNav doesn't load any page by default, so load home page.
            MainNav.SelectedItem = MainNav.MenuItems[0];
            // If navigation occurs on SelectionChanged, this isn't needed.
            // Because we use ItemInvoked to navigate, we need to call Navigate
            // here to load the home page.
            MainNav_Navigate(typeof(ReleasesPage), new EntranceNavigationTransitionInfo());
        }

        private void MainNav_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked == true)
            {
                MainNav_Navigate(typeof(SettingsPage), args.RecommendedNavigationTransitionInfo);
            }
            else if (args.InvokedItemContainer != null)
            {
                Type navPageType = Type.GetType(args.InvokedItemContainer.Tag.ToString());
                MainNav_Navigate(navPageType, args.RecommendedNavigationTransitionInfo);
            }
        }

        private void MainNav_Navigate(Type navPageType, NavigationTransitionInfo transitionInfo)
        {
            // Get the page type before navigation so you can prevent duplicate
            // entries in the backstack.
            Type preNavPageType = ContentFrame.CurrentSourcePageType;

            // Only navigate if the selected page isn't currently loaded.
            if (navPageType is not null && !Type.Equals(preNavPageType, navPageType))
            {
                ContentFrame.Navigate(navPageType, null, transitionInfo);
            }
        }

        private void On_Navigated(object sender, NavigationEventArgs e)
        {
            MainNav.IsBackEnabled = ContentFrame.CanGoBack;

            if (ContentFrame.SourcePageType == typeof(SettingsPage))
            {
                // SettingsItem is not part of MainNav.MenuItems, and doesn't have a Tag.
                MainNav.SelectedItem = (NavigationViewItem)MainNav.SettingsItem;
                MainNav.Header = "Параметры";
            }
            else if (ContentFrame.SourcePageType != null)
            {
                // Select the nav view item that corresponds to the page being navigated to.
                MainNav.SelectedItem = MainNav.MenuItems
                            .OfType<NavigationViewItem>()
                            .FirstOrDefault(i => i.Tag.Equals(ContentFrame.SourcePageType.FullName.ToString()), null) ?? MainNav.FooterMenuItems
                            .OfType<NavigationViewItem>()
                            .FirstOrDefault(i => i.Tag.Equals(ContentFrame.SourcePageType.FullName.ToString()), null);

                MainNav.Header =
                    ((NavigationViewItem)MainNav.SelectedItem)?.Content?.ToString();

            }
        }
    }
}
