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
    public sealed partial class MainPage : Page
    {
        private int strokesCount; //Liczba przyciśnięć tylko do poglądu, Signature ma swój
        public Stopwatch timer; //Obiekt zajmujący się czasem ogólnoaplikacji
        public List<Single> pressures; //lista sił nacisku punktów; nadmiarowe info zawarte w storke.GetInkPoints() 
        public List<long> times; //lista czasów naciśnięć poszczególnych punktów
        //TODO: MK wywal stąd te niepotrzebne listy
        public List<int> pressureChanges;
        /*lista zmian sił nacisku
         * -1 -- nacisk maleje
         *  0 -- nacisk się nie zmienia
         * +1 -- nacisk rośnie
         */
        SignatureController signatureController;
        AuthorController authorController;
        public MainPage()
        {
            //Start the clock!
            timer = new Stopwatch();
            timer.Start();


            pressures = new List<Single>();
            times = new List<long>();
            pressureChanges = new List<int>();

            signatureController = new SignatureController();
            authorController = new AuthorController();
            this.InitializeComponent();
            this.initializePenHandlers();

            //ściągnięcie listy autorów żeby wyświetliło default
            updateAuthorCombobox();
            authorCombobox.SelectedIndex = 0;

            
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
            var pressure = args.CurrentPoint.Properties.Pressure;
            Single previousPressure;
            if (pressures.Count() > 0)
                previousPressure = pressures[pressures.Count() - 1];
            else
                previousPressure = 0;
            var pressureChange = calcPressureChange(pressure, previousPressure);

            pressureChanges.Add(pressureChange);
            pressures.Add(pressure);
            //TODO: MK - wywal stąd te obliczenia, pobieraj dane z naszych klas
            updateInfoInLabel(label1, "Adam rysuje X: " + args.CurrentPoint.Position.X + ", Y: " + args.CurrentPoint.Position.Y + ", z mocą: " + args.CurrentPoint.Properties.Pressure + ", " + args.CurrentPoint.Properties.Twist);
            updateInfoInLabel(pressureLastPressedLabel, "Siła ostatniego naciśnięcia: " + pressure);
            updateInfoInLabel(pressureChangeLabel, "Zmiana siły nacisku: " + pressureChange);
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

        //Event handler dla rysowania
        private void Core_PointerReleasing(CoreInkIndependentInputSource sender, PointerEventArgs args)
        {
            Debug.WriteLine("Adam puścił");
        }

        private void Core_PointerPressing(CoreInkIndependentInputSource sender, PointerEventArgs args)
        {



            strokesCount = strokesCount + 1;
            
            times.Add(timer.ElapsedMilliseconds);
            updateInfoInLabel(strokesCountLabel, "Ilość naciśnięć: " + strokesCount);
            updateInfoInLabel(timeLastPressedLabel, "Czas ostatniego naciśnięcia w ms:  " + timer.ElapsedMilliseconds);
            Debug.WriteLine("Adam wcisnął " + strokesCount + " razy" + "ostatni raz " + timer.ElapsedMilliseconds);
        }

        //Funkcja aktualizacji tekstu Label, podaj nazwę obiektu, tekst
        private async void updateInfoInLabel(TextBlock givenLabel, string text)
        {
            //Updates informations asynchronously
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                givenLabel.Text = text;
            });
        }

        
        //Updates window text label
        private async void updateInfoAsync(String value)
        {
            //Updates informations asynchronously
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                label1.Text = value;
            });
        }

        //Action for button click
        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            

            Clear_Screen_Add_Strokes();
        }

        private void Clear_Screen_Add_Strokes()
        {
            strokesCount = 0; //tylko do wyświetlania, Signature class ma realcount
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
            //IsChecked zwraca typ 'bool?', może posiadać wartość null stąd dodatkowy if tutaj sprawdzający czy nie zwraca nulla
            bool isOriginal = false;
            if (isOriginalCheckBox.IsChecked.HasValue)
            {
                isOriginal = isOriginalCheckBox.IsChecked.Value;
            }
            try
            {
                authorController.getAuthor(authorCombobox.SelectedItem.ToString()).addSignature(signatureController.addSignature(strokes, isOriginal));
            }
            catch (System.NullReferenceException)
            {
                authorController.getAuthor("Default").addSignature(signatureController.addSignature(strokes, isOriginal));
                //nie było podanego autora, autor domyślny
               
            }
        }



        private void SaveToFile_Click(object sender, RoutedEventArgs e)
        {
            FileController saver = new FileController();
            saver.save(inkCanvas1.InkPresenter.StrokeContainer);
        }

        private void AddAuthor_Click(object sender, RoutedEventArgs e)
        {
            if (authorInputBox.Text.Equals(""))
            {
                Debug.WriteLine("Author name cannot be empty");
                authorInputBox.Background = new SolidColorBrush(Color.FromArgb(180, 255, 0, 0));
            }
            else if(authorController.contains(authorInputBox.Text))
            {
                Debug.WriteLine("Author already exists");
                authorInputBox.Background = new SolidColorBrush(Color.FromArgb(180, 255, 0, 0));
            }
            else
            {
                authorController.addAuthor(authorInputBox.Text);
                authorInputBox.Text = "";
                authorInputBox.Background = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
                updateAuthorCombobox();
            } 
        }

        private void updateAuthorCombobox()
        {
            authorCombobox.Items.Clear();

            foreach(var authorName in authorController.getAuthorsNames())
            {
                authorCombobox.Items.Add(authorName);
            }
            displaySignatures();
        }

        private void displaySignatures()
        {
            Page page = new Page();
            
        }

        private async void Button_ClickAsync(object sender, RoutedEventArgs e)
        {
            //drawAuthorSignature();
            CoreApplicationView newView = CoreApplication.CreateNewView();
            int newViewId = 0;
            await newView.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                Frame frame = new Frame();
                frame.Navigate(typeof(ShowSignatures), null);
                Window.Current.Content = frame;
                // You have to activate the window in order to show it later.
                Window.Current.Activate();

                newViewId = ApplicationView.GetForCurrentView().Id;
            });
            bool viewShown = await ApplicationViewSwitcher.TryShowAsStandaloneAsync(newViewId);
        }

        private void drawAuthorSignature()
        {
            var polyline1 = new Polyline();
            polyline1.Stroke = new SolidColorBrush(Windows.UI.Colors.Black);
            polyline1.StrokeThickness = 4;

            var points = new PointCollection();

            Debug.WriteLine(authorCombobox.SelectedItem);

            var author = authorController.getAuthor(authorCombobox.SelectedItem.ToString());

            var signature = author.getSignature();

            foreach(var stroke in signature.getStrokes())
            {
                foreach(var point in stroke.getPoints())
                {
                    points.Add(new Windows.Foundation.Point(point.getX(), point.getY()));
                }
            }

            
            polyline1.Points = points;

            canvas1.Children.Add(polyline1);

        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            var strokes = inkCanvas1.InkPresenter.StrokeContainer.GetStrokes();
            addSignature(strokes);
            Clear_Screen_Add_Strokes();
        }
    }
}