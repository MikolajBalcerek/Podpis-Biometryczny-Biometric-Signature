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
    public sealed partial class FindAuthorPage : Page
    {
        private AuthorController authorController;
        private SignatureController signatureController;

        public FindAuthorPage()
        {
            this.InitializeComponent();
            inkCanvas1.InkPresenter.InputDeviceTypes = CoreInputDeviceTypes.Mouse | CoreInputDeviceTypes.Pen;

            //inicjalizacja wielkości pola do rysowania
            this.initRealSizeInkCanvas(110, 40);
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
            SearchButton.Height = SearchButton.Width = height;
            guideLine.X1 = 0.05 * width;
            guideLine.X2 = 0.95 * width;
            guideLine.Y1 = guideLine.Y2 = 0.7 * height;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //base.OnNavigatedTo(e);
            this.authorController = (AuthorController)e.Parameter;
            this.signatureController = authorController.signatureController;

        }

        private void VerifyButton_Click(object sender, RoutedEventArgs e)
        {
            var sgn = getSignatureFromInkCanvas();
            if (sgn.getAllOriginalPoints().Count() == 0)
                return;
            System.Diagnostics.Debug.WriteLine("Wyszukany autor to " + kNearestNeighbours(buildFeatures(sgn)));
            inkCanvas1.InkPresenter.StrokeContainer.Clear();
        }

        private Signature getSignatureFromInkCanvas()
        {
            var inkStrokes = new List<InkStroke>(inkCanvas1.InkPresenter.StrokeContainer.GetStrokes());

            return signatureController.buildInitializedSignature(inkStrokes);
        }

        private List<double> buildFeatures(Signature sgn)
        {
            var res = new List<double>();
            res.Add(sgn.getHeight());
            res.Add(sgn.getLengthM());
            var tsp = sgn.getTimeSizeProbe();
            res.Add(tsp.getTotalRatioAreaToTime());
            res.Add(tsp.getTotalDrawingTime());
            return res;
        }

        private double L1(List<double> xs, List<double> ys)
        {
            return xs.Select((x, i) => Math.Abs(x - ys[i])).Sum();
        }

        private string kNearestNeighbours(List<double> inputFeatures)
        {
            const int k = 10;
            return authorController.getAuthorsNames().Select(name => authorController.getAuthor(name)).
                Select(author => author.getOriginalSignatures().Select(sgn => Tuple.Create(author.getName(), sgn))).
                SelectMany(x => x).AsEnumerable().Select(x => Tuple.Create(x.Item1, buildFeatures(x.Item2))).
                Select(x => Tuple.Create(x.Item1, L1(inputFeatures, x.Item2))).OrderByDescending(x => x.Item2).
                Take(k).GroupBy(x => x.Item1).OrderByDescending(grp => grp.Count()).First().Select(x=>x.Item1).First();
        }
    }
}
