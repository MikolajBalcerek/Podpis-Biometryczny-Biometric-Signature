using PodpisBio.Src.Author;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

//Szablon elementu Pusta strona jest udokumentowany na stronie https://go.microsoft.com/fwlink/?LinkId=234238

namespace PodpisBio.Src
{
    /// <summary>
    /// Pusta strona, która może być używana samodzielnie lub do której można nawigować wewnątrz ramki.
    /// </summary>
    public sealed partial class VerifyAuthorPage : Page
    {
        private AuthorController authorController;
        public VerifyAuthorPage()
        {
            this.InitializeComponent();
            inkCanvas1.InkPresenter.InputDeviceTypes = CoreInputDeviceTypes.Mouse | CoreInputDeviceTypes.Pen;

            //inicjalizacja wielkości pola do rysowania
            this.initRealSizeInkCanvas(130, 50);
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //base.OnNavigatedTo(e);
            this.authorController = (AuthorController)e.Parameter;

            //Zaktualizowanie listy autorów
            this.updateAuthorCombobox();
        }

        private void initRealSizeInkCanvas(double mmWidth, double mmHeight)
        {
            RealScreenSizeCalculator calc = new RealScreenSizeCalculator();
            int width = (int)calc.convertToPixels(mmWidth);
            int height = (int)calc.convertToPixels(mmHeight);
            inkCanvasHolder.Height = height;
            inkCanvasHolder.Width = width;
            inkCanvasHolder.MinWidth = width;
            StackPanel1.MinWidth = width + height + 10;
            VerifyButton.Height = VerifyButton.Width = height;
            guideLine.X1 = 0.05 * width;
            guideLine.X2 = 0.95 * width;
            guideLine.Y1 = guideLine.Y2 = 0.7 * height;
        }

        private void VerifyButton_Click(object sender, RoutedEventArgs e)
        {
            inkCanvas1.InkPresenter.StrokeContainer.Clear();
        }

        private void updateAuthorCombobox()
        {
            var authorNames = authorController.getAuthorsNames();

            if (authorNames.Any())
            {
                foreach (var authorName in this.authorController.getAuthorsNames())
                {
                    this.authorCombobox.Items.Add(authorName);
                }
                this.authorCombobox.SelectedIndex = 0;
            }
            
            
        }
    }
}
