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

//Szablon elementu Pusta strona jest udokumentowany na stronie https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x415

namespace PodpisBio
{
    /// <summary>
    /// Pusta strona, która może być używana samodzielnie lub do której można nawigować wewnątrz ramki.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        SignatureController signatureController;
        AuthorController authorController;

        public MainPage()
        {
            signatureController = new SignatureController();
            authorController = new AuthorController();

            this.InitializeComponent();
            this.initializePenHandlers();
        }

        private void initializePenHandlers()
        {
            CoreInkIndependentInputSource core = CoreInkIndependentInputSource.Create(inkCanvas1.InkPresenter);
            core.PointerPressing += Core_PointerPressing;
            core.PointerReleasing += Core_PointerReleasing;
            core.PointerMoving += Core_PointerMoving;

            inkCanvas1.InkPresenter.InputDeviceTypes = CoreInputDeviceTypes.Mouse | CoreInputDeviceTypes.Pen;
        }

        //Event handler dla rysowania
        private void Core_PointerMoving(CoreInkIndependentInputSource sender, PointerEventArgs args)
        {
            if(args.CurrentPoint.Properties.Pressure > 0.9)
            {
                updateInfoAsync("Adam rysuje X: " + args.CurrentPoint.Position.X + ", Y: " + args.CurrentPoint.Position.Y + ", z mocą: IT'S OVER 9000");
            }
            else
            {
                updateInfoAsync("Adam rysuje X: " + args.CurrentPoint.Position.X + ", Y: " + args.CurrentPoint.Position.Y + ", z mocą: " + args.CurrentPoint.Properties.Pressure +", " + args.CurrentPoint.Properties.Twist);
            }

            
        }

        //Event handler dla rysowania
        private void Core_PointerReleasing(CoreInkIndependentInputSource sender, PointerEventArgs args)
        {
            Debug.WriteLine("Adam puścił");
        }

        private void Core_PointerPressing(CoreInkIndependentInputSource sender, PointerEventArgs args)
        {
            Debug.WriteLine("Adam wcisnął");
        }

        
        //Updates window text label
        private async System.Threading.Tasks.Task updateInfoAsync(String value)
        {
            //Updates informations asynchronously
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                label1.Text = value;
            });
        }

        //Action for button click
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var strokes = inkCanvas1.InkPresenter.StrokeContainer.GetStrokes();
            

            consoleStrokeInfo(strokes);
            addSignature(strokes);

            inkCanvas1.InkPresenter.StrokeContainer.Clear();


        }

        //Write Stroke info to debug window
        private void consoleStrokeInfo(IReadOnlyList<InkStroke> strokes)
        {
            foreach (var stroke in strokes)
            {
                Debug.WriteLine("Stroke: " + stroke.Id + ", point count: " + stroke.GetInkPoints().Count + ", duration: " + stroke.StrokeDuration.Value.Seconds + " s " + stroke.StrokeDuration.Value.Milliseconds + " ms");
                foreach (var point in stroke.GetInkPoints())
                {
                    Debug.WriteLine("x: " + point.Position.X + ", y: " + point.Position.Y + ", pressure: " + point.Pressure + ", timestamp: " + point.Timestamp);
                }

            }
        }    
        
        //Add signature
        private void addSignature(IReadOnlyList<InkStroke> strokes)
        {
            Signature signature = new Signature();
            foreach (var strokeTemp in strokes)
            {
                Stroke stroke = new Stroke();
                foreach (var pointTemp in strokeTemp.GetInkPoints())
                {
                    Src.Point point = new Src.Point((float) pointTemp.Position.X, (float) pointTemp.Position.Y, pointTemp.Pressure);
                    stroke.addPoint(point);
                    Debug.WriteLine(stroke.getPoints().Count);
                }
                signature.addStroke(stroke);   
            }
            signatureController.addSignature(signature);
        }
    }
}
