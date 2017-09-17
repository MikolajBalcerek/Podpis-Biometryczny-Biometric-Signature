using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Input.Inking.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Diagnostics;
using PodpisBio.Src;
using PodpisBio.Src.Author;
using System.Threading.Tasks;
using System.Text;
using Windows.UI.Input.Inking;
using Windows.ApplicationModel.Core;
using Windows.UI.ViewManagement;
using Windows.UI;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml.Media.Imaging;
using System.Collections.ObjectModel;

//Szablon elementu Pusta strona jest udokumentowany na stronie https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x415

namespace PodpisBio
{
    public sealed partial class ShowSignatures : Page
    {
        private Signature signature;
        private AuthorController autController = new AuthorController();
        ObservableCollection<String> plotOptions = new ObservableCollection<String>();
        ObservableCollection<String> authorNames = new ObservableCollection<String>();
        ObservableCollection<int> signatureIndexes = new ObservableCollection<int>();

    
        public ShowSignatures()
        {
            this.InitializeComponent();


            String[] options = { "Podpis", "Oś X", "Oś Y", "Siła", "TiltX", "TiltY",
                "Szybkość", "Szybkość_X", "Szybkość_Y",
                "Przyspieszenie", "Przyspieszenie_X", "Przyspieszenie_Y" };
            foreach (var x in options)
                plotOptions.Add(x);

            //plotCombobox.SelectedIndex = 0;
            // TODO spraw, by poniższa linijka działała
            plotCombobox.SelectedItem = "Podpis";
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Debug.WriteLine("Adam odebrał eventa");
            base.OnNavigatedTo(e);
            this.autController = (AuthorController)e.Parameter;
            Debug.WriteLine("Imiona autorów Adama to: ");
            foreach(var name in autController.getAuthorsNames())
                Debug.WriteLine("\t " + name);
            foreach(var autor in autController.getAuthorsNames())
                this.authorNames.Add(autor);
        }

        private void autComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Debug.WriteLine("Adam zmienia autora do rysowania.");
            this.signature = null;

            string authorName = autCombobox.SelectedItem.ToString();
            var author = this.autController.getAuthor(authorName);
            sigCombobox.SelectedItem = null;
            signatureIndexes.Clear();
            for (int i = 0; i < author.getSignaturesNumber(); i++)
                this.signatureIndexes.Add(i);
            for (int i = 0; i < author.getSignaturesNumber(); i++)
                Debug.WriteLine(this.signatureIndexes[i]);
        }

        private void sigComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sigCombobox.SelectedItem == null)
                return;
            string authorName = autCombobox.SelectedItem.ToString();
            var author = this.autController.getAuthor(authorName);
            var index = sigCombobox.SelectedIndex;
            this.signature = author.getSignature(index);
        }

        private void drawPoints(PointCollection points)
        {
            var polyline = new Polyline();
            polyline.Stroke = new SolidColorBrush(Windows.UI.Colors.Black);
            polyline.StrokeThickness = 1;
            polyline.Points = points;
            canvas1.Children.Add(polyline);
        }

        private async void NullSignatureDialog()
        {
            ContentDialog dialog = new ContentDialog
            {
                Title = "Nie wybrano podpisu lub cechy.",
                Content = "Aby narysować wykres podpisu, musisz wybrać autora podpisu,jego numer oraz cechę do rysowania.",
                CloseButtonText = "Zamknij",
                DefaultButton = ContentDialogButton.Close
            };
            ContentDialogResult result = await dialog.ShowAsync();
        }

        private void ShowPlot_Click(object sender, RoutedEventArgs e)
        {
            canvas1.Children.Clear();
            if(this.signature == null || plotCombobox.SelectedItem == null)
            {
                NullSignatureDialog();
                return;
            }
            Debug.WriteLine("Wartość plotComboboxa to " + plotCombobox.SelectedItem.ToString());
            var option = plotCombobox.SelectedItem.ToString();
            if (option == "Podpis")
                drawSignature();
            else
                drawFeature(option);
        }

        private void drawFeature(string option)
        {
            var ptsToDraw = new PointCollection();
            var times = getNormalisedTimes();
            var points = this.signature.getAllOriginalPoints();
            var derivatives = this.signature.getOriginalDerivatives();
            IEnumerable<float> feature;
            switch (option)
            {
                case "Oś X":
                    feature = from x in points select x.getX();
                    break;
                case "Oś Y":
                    feature = from x in points select x.getY();
                    break;
                case "Siła":
                    feature = from x in points select x.getPressure();
                    break;
                case "TiltX":
                    feature = from x in points select x.getTiltX();
                    break;
                case "TiltY":
                    feature = from x in points select x.getTiltY();
                    break;
                case "Szybkość":
                    feature = from x in derivatives select x.Velocity;
                    break;
                case "Szybkość_X":
                    feature = from x in derivatives select x.VelocityX;
                    break;
                case "Szybkość_Y":
                    feature = from x in derivatives select x.VelocityY;
                    break;
                case "Przyspieszenie":
                    feature = from x in derivatives select x.Acc;
                    break;
                case "Przyspieszenie_X":
                    feature = from x in derivatives select x.AccX;
                    break;
                case "Przyspieszenie_Y":
                    feature = from x in derivatives select x.AccY;
                    break;

                default:
                    return;
            }
            var normalised = normaliseFeature(feature);
            for (int i = 0; i < normalised.Count; i++)
                ptsToDraw.Add(new Windows.Foundation.Point(times[i], normalised[i]));
            drawPoints(ptsToDraw);
        }

        private List<double> getNormalisedTimes()
        {
            var times = from x in this.signature.getAllOriginalPoints() select x.getTime();
            var normalised = new List<double>();
            var width = canvas1.ActualWidth;
            foreach (var time in times)
                normalised.Add(3 * width * (time - times.First()) / (times.Last() - times.First()));
            //0 division?
            return normalised;
        }

        private List<double> normaliseFeature(IEnumerable<float> samples)
        {
            var normalised = new List<double>();
            var height = canvas1.ActualHeight;
            foreach (var sample in samples)
                normalised.Add(height * (sample) / (samples.Max() - samples.Min()) + height);
            //is 0 division possible here
            return normalised;
        }

        private void drawSignature()
        {
            Debug.WriteLine("Adam rysuje podpis.");
            foreach (var stroke in signature.getStrokesOriginal())
            {
                var points = new PointCollection();
                foreach (var point in stroke.getPoints())
                    points.Add(new Windows.Foundation.Point(point.getX(), point.getY()));
                drawPoints(points);
            }
        }
    }
}