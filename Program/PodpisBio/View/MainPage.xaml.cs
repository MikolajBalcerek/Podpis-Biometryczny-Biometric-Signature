﻿using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using PodpisBio.Src;
using Windows.Foundation.Metadata;
using Windows.UI.ViewManagement;
using Windows.UI;
using Windows.ApplicationModel.Core;
using PodpisBio.Src.Author;
using Windows.UI.Xaml.Media;

//Szablon elementu Pusta strona jest udokumentowany na stronie https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x415

namespace PodpisBio
{
    /// <summary>
    /// Pusta strona, która może być używana samodzielnie lub do której można nawigować wewnątrz ramki.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private SignatureController signatureController;
        private AuthorController authorController;
        public MainPage()
        {
            this.InitializeComponent();
            this.setNavbarColor();
            this.signatureController = new SignatureController();
            this.authorController = new AuthorController(signatureController);
            this.goToDefaultPage(this, null);
            this.showTitleBar(true);
        }

        //Ustawia kolor paska tytułowego w oknie aplikacji
        private void setNavbarColor()
        {
            if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.ApplicationView"))
            {
                var titleBar = ApplicationView.GetForCurrentView().TitleBar;
                if (titleBar != null)
                {
                    var textColor = Colors.White;
                    var titlebarBackgroundColor = Color.FromArgb(255, 153, 22, 22);
                    var buttonBackgroundColor = Color.FromArgb(255, 133, 22, 22);

                    titleBar.ButtonBackgroundColor = buttonBackgroundColor;
                    titleBar.ButtonForegroundColor = textColor;
                    titleBar.BackgroundColor = titlebarBackgroundColor;
                    titleBar.ForegroundColor = textColor;
                    titleBar.InactiveBackgroundColor = titlebarBackgroundColor;
                    titleBar.InactiveForegroundColor = Colors.LightGray;
                    titleBar.ButtonInactiveBackgroundColor = buttonBackgroundColor;
                    titleBar.ButtonInactiveForegroundColor = textColor;
                }
            }
        }

        private void showTitleBar(bool show)
        {
            ApplicationViewTitleBar formattableTitleBar = ApplicationView.GetForCurrentView().TitleBar;
            CoreApplicationViewTitleBar coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            coreTitleBar.ExtendViewIntoTitleBar = !show;
        }

        private void goToDefaultPage(object sender, RoutedEventArgs e)
        {
            this.MainPageDisplayFrame.Navigate(typeof(DefaultPage));
        }

        private void Wyszukiwanie_Click(object sender, RoutedEventArgs e)
        {
            clearSelection();
            this.SearchButton.Background = new SolidColorBrush(Color.FromArgb(255, 153, 22, 22));
            this.MainPageDisplayFrame.Navigate(typeof(FindAuthorPage));
        }

        private void Weryfikowanie_Click(object sender, RoutedEventArgs e)
        {
            clearSelection();
            this.VerifyButton.Background = new SolidColorBrush(Color.FromArgb(255, 153, 22, 22));
            this.MainPageDisplayFrame.Navigate(typeof(VerifyAuthorPage), authorController);
        }

        private void Podpisy_Click(object sender, RoutedEventArgs e)
        {
            clearSelection();
            this.SignatureButton.Background = new SolidColorBrush(Color.FromArgb(255, 153, 22, 22));
            this.MainPageDisplayFrame.Navigate(typeof(SignaturePage), authorController);
        }

        private void Wykresy_Click(object sender, RoutedEventArgs e)
        {
            clearSelection();
            this.ChartButton.Background = new SolidColorBrush(Color.FromArgb(255, 153, 22, 22));
            this.MainPageDisplayFrame.Navigate(typeof(ChartsPage), authorController);
        }

        private void Ustawienia_Click(object sender, RoutedEventArgs e)
        {
            clearSelection();
            this.SettingsButton.Background = new SolidColorBrush(Color.FromArgb(255, 153, 22, 22));
            this.MainPageDisplayFrame.Navigate(typeof(SettingsPage));
        }

        private void Statystyki_Click(object sender, RoutedEventArgs e)
        {
            clearSelection();
            this.StatButton.Background = new SolidColorBrush(Color.FromArgb(255, 153, 22, 22));
            this.MainPageDisplayFrame.Navigate(typeof(StatisticPage), authorController);
        }

        private void clearSelection()
        {
            this.SearchButton.Background = new SolidColorBrush(Color.FromArgb(255, 50, 50, 50));
            this.VerifyButton.Background = new SolidColorBrush(Color.FromArgb(255, 50, 50, 50));
            this.SignatureButton.Background = new SolidColorBrush(Color.FromArgb(255, 50, 50, 50));
            this.ChartButton.Background = new SolidColorBrush(Color.FromArgb(255, 50, 50, 50));
            this.StatButton.Background = new SolidColorBrush(Color.FromArgb(255, 50, 50, 50));
            this.SettingsButton.Background = new SolidColorBrush(Color.FromArgb(255, 50, 50, 50));
        }
    }
}