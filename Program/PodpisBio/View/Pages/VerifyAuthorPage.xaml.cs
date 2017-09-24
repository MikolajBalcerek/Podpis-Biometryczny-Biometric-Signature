using PodpisBio.Src.Author;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Input.Inking;
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
        private SignatureController signatureController;
        public VerifyAuthorPage()
        {
            this.InitializeComponent();
            inkCanvas1.InkPresenter.InputDeviceTypes = CoreInputDeviceTypes.Mouse | CoreInputDeviceTypes.Pen;

            //inicjalizacja wielkości pola do rysowania
            this.initRealSizeInkCanvas(110, 40);
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //base.OnNavigatedTo(e);
            this.authorController = (AuthorController)e.Parameter;
            this.signatureController = authorController.signatureController;

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
            VerifyButton.Height = ClearButton.Height = height / 2;
            VerifyButton.Width = ClearButton.Width = height;
            guideLine.X1 = 0.05 * width;
            guideLine.X2 = 0.95 * width;
            guideLine.Y1 = guideLine.Y2 = 0.7 * height;
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            inkCanvas1.InkPresenter.StrokeContainer.Clear();
        }

        private void VerifyButton_Click(object sender, RoutedEventArgs e)
        {
            //zainicjalizowana sygnatura z inkCanvas
            var signature = getSignatureFromInkCanvas();
            //zainicjalizowane ORYGINALNE sygnatury od wybranego autora z comboBoxa
            var authorOriginalSignatures = getSignaturesFromAuthorCombobox();


            //MIEJSCE NA METODĘ DO POROWNYWANIA PODPISOW

            //MIEJSCE NA METODĘ DO POROWNYWANIA PODPISOW


            //wyświetlenie rezultatu
            String result = "REZULTAT";
            this.resultText.Text = "Wynik weryfikacji: " + result;
        }

        private void updateAuthorCombobox()
        {
            var authorNames = authorController.getAuthorsNames();

            if (authorNames.Any())
            {
                this.authorCombobox.Items.Clear();
                foreach (var authorName in this.authorController.getAuthorsNames())
                {
                    this.authorCombobox.Items.Add(authorName);
                }
                this.authorCombobox.SelectedIndex = 0;
            }
        }

        //zwraca zainicjalizowaną sygnaturę od inkCanvas
        private Signature getSignatureFromInkCanvas()
        {
            var inkStrokes = new List<InkStroke>(inkCanvas1.InkPresenter.StrokeContainer.GetStrokes());

            return signatureController.buildInitializedSignature(inkStrokes);
        }

        private List<Signature> getSignaturesFromAuthorCombobox()
        {
            List<Signature> signatureList = new List<Signature>();
            if (this.authorCombobox.Items.Any())
            {
                signatureList = authorController.getAuthor(this.authorCombobox.SelectedItem.ToString()).getOriginalSignatures();
            }
            return signatureList;
        }
    }
}
