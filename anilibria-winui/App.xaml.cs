using Microsoft.UI.Xaml;
using Windows.System;
using Windows.UI.Core;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace anilibria
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when the application is launched.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            m_window = new MainWindow();
            m_window.Activate();

            //m_window.Dispatcher.AcceleratorKeyActivated += CoreDispatcher_AcceleratorKeyActivated;

            // Add support for system back requests. 
            //SystemNavigationManager.GetForCurrentView().BackRequested += System_BackRequested;

            // Add support for mouse navigation buttons. 
            //Window.Current.CoreWindow.PointerPressed += CoreWindow_PointerPressed;
        }

        private Window m_window;
        public MainWindow MWindow => m_window as MainWindow;

        // Invoked on every keystroke, including system keys such as Alt key combinations.
        // Used to detect keyboard navigation between pages even when the page itself
        // doesn't have focus.
        private void CoreDispatcher_AcceleratorKeyActivated(CoreDispatcher sender, AcceleratorKeyEventArgs e)
        {
            // When Alt+Left are pressed navigate back.
            // When Alt+Right are pressed navigate forward.
            if (e.EventType == CoreAcceleratorKeyEventType.SystemKeyDown
                && (e.VirtualKey == VirtualKey.Left || e.VirtualKey == VirtualKey.Right)
                && e.KeyStatus.IsMenuKeyDown == true
                && !e.Handled)
            {
                if (e.VirtualKey == VirtualKey.Left)
                {
                    e.Handled = MWindow.TryGoBack();
                }
                else if (e.VirtualKey == VirtualKey.Right)
                {
                    e.Handled = MWindow.TryGoForward();
                }
            }
        }

        // Handle system back requests.
        private void System_BackRequested(object sender, BackRequestedEventArgs e)
        {
            if (!e.Handled)
            {
                e.Handled = MWindow.TryGoBack();
            }
        }

        // Handle mouse back button.
        private void CoreWindow_PointerPressed(CoreWindow sender, PointerEventArgs e)
        {
            // For this event, e.Handled arrives as 'true'.
            if (e.CurrentPoint.Properties.IsXButton1Pressed)
            {
                e.Handled = !MWindow.TryGoBack();
            }
            else if (e.CurrentPoint.Properties.IsXButton2Pressed)
            {
                e.Handled = !MWindow.TryGoForward();
            }
        }
    }
}
