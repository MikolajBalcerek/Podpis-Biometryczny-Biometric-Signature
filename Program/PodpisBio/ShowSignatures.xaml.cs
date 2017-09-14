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
    /// <summary>
    /// Pusta strona, która może być używana samodzielnie lub do której można nawigować wewnątrz ramki.
    /// </summary>
    public sealed partial class ShowSignatures : Page
    {
        private Signature signature;
        ObservableCollection<String> plotOptions = new ObservableCollection<String>();

        public ShowSignatures()
        {
            this.InitializeComponent();

            String[] options = { "Podpis", "Oś X", "Oś Y", "Siła", "TiltX", "TiltY", "Szybkość", "Szybkość_X", "Szybkość_Y",
                "Przyspieszenie", "Przyspieszenie_X", "Przyspieszenie_Y" };
            foreach (var x in options)
                plotOptions.Add(x);
            plotCombobox.SelectedValue = "Podpis";
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            this.signature  = (Signature)e.Parameter;
            //foreach (var point in signature.getStrokes()[0].getPoints())
            //{
            //    Debug.WriteLine("Adam otworzył punkt na drugiej stronie " + point.getX() + " " + point.getY());
            //}
        }


        private void drawPoints(PointCollection points)
        {
            var polyline = new Polyline();
            polyline.Stroke = new SolidColorBrush(Windows.UI.Colors.Black);
            polyline.StrokeThickness = 1;
            polyline.Points = points;
            canvas1.Children.Add(polyline);
        }



        private void ShowPlot_Click(object sender, RoutedEventArgs e)
        {
            canvas1.Children.Clear();
            Debug.WriteLine("Wartość comboboxa to " + plotCombobox.SelectedItem.ToString());
            var option = plotCombobox.SelectedItem.ToString();
            if (option == "Podpis" || option == "")
                drawSignature();
            else
            {
                var ptsToDraw = new PointCollection();
                var times = getNormalisedTimes();
                IEnumerable<float> feature;
                switch (option)
                {
                    case "Oś X": feature = from x in this.signature.getAllPoints() select x.getX();
                        break;
                    case "Oś Y": feature = from x in this.signature.getAllPoints() select x.getY();
                        break;
                    case "Siła": feature = from x in this.signature.getAllPoints() select x.getPressure();
                        break;
                    case "TiltX": feature = from x in this.signature.getAllPoints() select x.getTiltX();
                        break;
                    case "TiltY": feature = from x in this.signature.getAllPoints() select x.getTiltY();
                        break;

                    default: feature = from x in this.signature.getAllPoints() select x.getX();
                        break;
                }
                var normalised = normaliseFeature(feature);
                for (int i = 0; i < normalised.Count; i++)
                {
                    ptsToDraw.Add(new Windows.Foundation.Point(times[i], normalised[i]));
                    Debug.WriteLine("Adam rysuje ficzera " + option + " " + times[i] + " " + normalised[i]);
                }
                drawPoints(ptsToDraw);
            }
            
        }

        private List<double> getNormalisedTimes()
        {
            var times = from x in this.signature.getAllPoints() select x.getTime();
            var normalised = new List<double>();
            var width = canvas1.ActualWidth;
            foreach (var time in times)
                normalised.Add(2 * width * (time - times.First()) / (times.Last() - times.First()));
            //0 division?
            return normalised;
        }

        private List<double> normaliseFeature(IEnumerable<float> samples)
        {
            var normalised = new List<double>();
            var height = canvas1.ActualHeight;
            foreach (var sample in samples)
                normalised.Add(2 * height * (sample - samples.First()) / (samples.Max() - samples.Min()) + 1.5 * height);
            //is 0 division possible here
            return normalised;
        }
        private void drawX()
        {
            var ptsToDraw = new PointCollection();
            var times = getNormalisedTimes();
            var xs = from x in this.signature.getAllPoints() select x.getX();
            var normalised = normaliseFeature(xs);
            for(int i=0; i < normalised.Count; i++)
            {
                ptsToDraw.Add(new Windows.Foundation.Point(times[i], normalised[i]));
                Debug.WriteLine("X Adama:  " + times[i] + " " + normalised[i]);
            }
            drawPoints(ptsToDraw);
        }
        private void drawSignature()
        {
            Debug.WriteLine("Adam rysuje podpis.");
            foreach (var stroke in signature.getStrokes())
            {
                var points = new PointCollection();
                foreach (var point in stroke.getPoints())
                {
                    points.Add(new Windows.Foundation.Point(point.getX(), point.getY()));
                    Debug.WriteLine("Punkt Adama: " + point.getX() + " " + point.getY());
                }
                drawPoints(points);
            }
        }
    }
}