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

//Szablon elementu Pusta strona jest udokumentowany na stronie https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x415

namespace PodpisBio
{
    /// <summary>
    /// Pusta strona, która może być używana samodzielnie lub do której można nawigować wewnątrz ramki.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        public int strokesCount; //Liczba przyciśnięć
        public Stopwatch timer; //Obiekt zajmujący się czasem ogólnoaplikacji
        public List<Single> pressures; //lista sił nacisku punktów; nadmiarowe info zawarte w storke.GetInkPoints() 
        public List<long> times; //lista czasów naciśnięć poszczególnych punktów

        public List<int> pressureChanges; 
        /*lista zmian sił nacisku
         * -1 -- nacisk maleje
         *  0 -- nacisk się nie zmienia
         * +1 -- nacisk rośnie
         */


        public MainPage()
        {
            //Start the clock!
            timer = new Stopwatch();
            timer.Start();

            strokesCount = 0; //Liczba przyciśnięć na 0

            pressures = new List<Single>();
            times = new List<long>();
            pressureChanges = new List<int>();

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


        private void Core_PointerMoving(CoreInkIndependentInputSource sender, PointerEventArgs args)
        {
            if (args.CurrentPoint.Properties.Pressure > 0.9)
            {
                updateInfoAsync("Adam rysuje X: " + args.CurrentPoint.Position.X + ", Y: " + args.CurrentPoint.Position.Y + ", z mocą: IT'S OVER 9000");
            }
            else
            {
                updateInfoAsync("Adam rysuje X: " + args.CurrentPoint.Position.X + ", Y: " + args.CurrentPoint.Position.Y + ", z mocą: " + args.CurrentPoint.Properties.Pressure + ", " + args.CurrentPoint.Properties.Twist);
            }


        }

        private int calcPressureChange(Single currentPress, Single previousPress)
        {
            var difference = currentPress - previousPress;
            if (difference == 0)
                return 0;
            if (difference > 0)
                return 1;
            return -1;
        }

        private void Core_PointerReleasing(CoreInkIndependentInputSource sender, PointerEventArgs args)
        {
            Debug.WriteLine("Adam puścił");
        }

        private void Core_PointerPressing(CoreInkIndependentInputSource sender, PointerEventArgs args)
        {
            strokesCount = strokesCount + 1;

            var pressure = args.CurrentPoint.Properties.Pressure;
            Single previousPressure;
            if (pressures.Count() > 0)
                previousPressure = pressures[pressures.Count() - 1];
            else
                previousPressure = 0;
           var pressureChange = calcPressureChange(pressure, previousPressure);

           pressureChanges.Add(pressureChange);
           pressures.Add(pressure);
            
            times.Add(timer.ElapsedMilliseconds);
            updateInfoInLabel(strokesCountLabel, "Ilość naciśnięć: " + strokesCount);
            updateInfoInLabel(timeLastPressedLabel, "Czas ostatniego naciśnięcia w ms:  " + timer.ElapsedMilliseconds);
            updateInfoInLabel(pressureLastPressedLabel, "Siła ostatniego naciśnięcia: " + pressure);
            updateInfoInLabel(pressureChangeLabel, "Zmiana siły nacisku: " + pressureChange);
            Debug.WriteLine("Adam wcisnął " + strokesCount + " razy" + "ostatni raz " + timer.ElapsedMilliseconds);
        }

        //Funkcja aktualizacji tekstu Label, podaj nazwę obiektu, tekst
        private async System.Threading.Tasks.Task updateInfoInLabel(TextBlock givenLabel, string text)
        {
            //Updates informations asynchronously
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                givenLabel.Text = text;
            });
        }

        private async System.Threading.Tasks.Task updateInfoAsync(String value)
        {
            //Updates informations asynchronously
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                label1.Text = value;
            });
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var strokes = inkCanvas1.InkPresenter.StrokeContainer.GetStrokes();

            Debug.WriteLine(strokes.Count);
            foreach (var stroke in strokes)
            {
                Debug.WriteLine("Stroke: " + stroke.Id + ", point count: " + stroke.GetInkPoints().Count + ", duration: " + stroke.StrokeDuration.Value.Seconds + " s " + stroke.StrokeDuration.Value.Milliseconds + " ms");
                foreach (var point in stroke.GetInkPoints())
                {
                    Debug.WriteLine("x: " + point.Position.X + ", y: " + point.Position.Y + ", pressure: " + point.Pressure + ", timestamp: " + point.Timestamp);

                }
            }

            inkCanvas1.InkPresenter.StrokeContainer.Clear();
        }

        private void TextBlock_SelectionChanged(System.Object sender, RoutedEventArgs e)
        {

        }
    }
}