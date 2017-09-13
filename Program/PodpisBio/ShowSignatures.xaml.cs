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

            String[] options = { "Podpis", "X", "Y", "Pressure", "Szybkość", "Szybkość_X", "Szybkość_Y",
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

        private void clearCanvas()
        {
            canvas1.Children.Clear();
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
            Debug.WriteLine("Wartość comboboxa to " + plotCombobox.SelectedItem.ToString());
            clearCanvas();
            Debug.WriteLine("Adam rysuje podpis.");
            double thickness = 1;
            Color color = Colors.Black;

            foreach (var stroke in signature.getStrokes())
            {
                var points = new PointCollection();
                foreach (var point in stroke.getPoints())
                {
                    points.Add(new Windows.Foundation.Point(point.getX(), point.getY()));
                }
                drawPoints(points);
            }
        }
    }
}