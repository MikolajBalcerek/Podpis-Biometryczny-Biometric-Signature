using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using PodpisBio.Src;

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
    }
}