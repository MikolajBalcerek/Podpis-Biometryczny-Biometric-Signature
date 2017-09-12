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

//Szablon elementu Pusta strona jest udokumentowany na stronie https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x415

namespace PodpisBio
{
    /// <summary>
    /// Pusta strona, która może być używana samodzielnie lub do której można nawigować wewnątrz ramki.
    /// </summary>
    public sealed partial class ShowSignatures : Page
    {

        public ShowSignatures()
        {
            this.InitializeComponent();
            DrawSomething();
        }

        private void DrawSomething()
        {
            var path1 = new Windows.UI.Xaml.Shapes.Path();
            path1.Fill = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 204, 204, 255));
            path1.Stroke = new SolidColorBrush(Windows.UI.Colors.Black);
            path1.StrokeThickness = 1;

            var geometryGroup1 = new GeometryGroup();
            var rectangleGeometry1 = new RectangleGeometry();
            rectangleGeometry1.Rect = new Rect(50, 5, 100, 10);
            var rectangleGeometry2 = new RectangleGeometry();
            rectangleGeometry2.Rect = new Rect(5, 5, 95, 180);
            geometryGroup1.Children.Add(rectangleGeometry1);
            geometryGroup1.Children.Add(rectangleGeometry2);

            var ellipseGeometry1 = new EllipseGeometry();
            ellipseGeometry1.Center = new Windows.Foundation.Point(100, 100);
            ellipseGeometry1.RadiusX = 20;
            ellipseGeometry1.RadiusY = 30;
            geometryGroup1.Children.Add(ellipseGeometry1);

            var pathGeometry1 = new PathGeometry();
            var pathFigureCollection1 = new PathFigureCollection();
            var pathFigure1 = new PathFigure();
            pathFigure1.IsClosed = true;
            pathFigure1.StartPoint = new Windows.Foundation.Point(50, 50);
            pathFigureCollection1.Add(pathFigure1);
            pathGeometry1.Figures = pathFigureCollection1;

            var pathSegmentCollection1 = new PathSegmentCollection();
            var pathSegment1 = new BezierSegment();
            pathSegment1.Point1 = new Windows.Foundation.Point(75, 300);
            pathSegment1.Point2 = new Windows.Foundation.Point(125, 100);
            pathSegment1.Point3 = new Windows.Foundation.Point(150, 50);
            pathSegmentCollection1.Add(pathSegment1);

            var pathSegment2 = new BezierSegment();
            pathSegment2.Point1 = new Windows.Foundation.Point(125, 300);
            pathSegment2.Point2 = new Windows.Foundation.Point(75, 100);
            pathSegment2.Point3 = new Windows.Foundation.Point(50, 50);
            pathSegmentCollection1.Add(pathSegment2);
            pathFigure1.Segments = pathSegmentCollection1;

            geometryGroup1.Children.Add(pathGeometry1);
            path1.Data = geometryGroup1;

            canvas1.Children.Add(path1);
        }
    }
}