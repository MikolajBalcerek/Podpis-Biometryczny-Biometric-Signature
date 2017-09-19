using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using PodpisBio.Src;
using Windows.Foundation.Metadata;
using Windows.UI.ViewManagement;
using Windows.UI;
using Windows.ApplicationModel.Core;

//Szablon elementu Pusta strona jest udokumentowany na stronie https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x415

namespace PodpisBio
{
    /// <summary>
    /// Pusta strona, która może być używana samodzielnie lub do której można nawigować wewnątrz ramki.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            this.setNavbarColor();
            this.goToDefaultPage(this, null);
        }

        //Ustawia kolor paska tytułowego w oknie aplikacji
        private void setNavbarColor()
        {
            if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.ApplicationView"))
            {
                var titleBar = ApplicationView.GetForCurrentView().TitleBar;
                if (titleBar != null)
                {
                    titleBar.ButtonBackgroundColor = Color.FromArgb(255, 133, 22, 22);
                    titleBar.ButtonForegroundColor = Colors.White;
                    titleBar.BackgroundColor = Color.FromArgb(255, 153, 22, 22);
                    titleBar.ForegroundColor = Colors.White;
                }
            }
        }

        private void goToDefaultPage(object sender, RoutedEventArgs e)
        {
            this.MainPageDisplayFrame.Navigate(typeof(DefaultPage));
        }

        private void Wyszukiwanie_Click(object sender, RoutedEventArgs e)
        {
            this.MainPageDisplayFrame.Navigate(typeof(FindAuthorPage));
        }

        private void Weryfikowanie_Click(object sender, RoutedEventArgs e)
        {
            this.MainPageDisplayFrame.Navigate(typeof(VerifyAuthorPage));
        }

        private void Podpisy_Click(object sender, RoutedEventArgs e)
        {
            this.MainPageDisplayFrame.Navigate(typeof(SignaturePage));
        }

        private void Wykresy_Click(object sender, RoutedEventArgs e)
        {
            this.MainPageDisplayFrame.Navigate(typeof(ChartsPage));
        }

        private void Ustawienia_Click(object sender, RoutedEventArgs e)
        {
            this.MainPageDisplayFrame.Navigate(typeof(SettingsPage));
        }
    }
}