﻿using System;
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
using Windows.Graphics.Display;
using PodpisBio.Src.Service;
using Windows.Foundation.Metadata;

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

        SignatureController signatureController;
        AuthorController authorController;
        public MainPage()
        {
            //Start the clock!
            timer = new Stopwatch();
            timer.Start();

            signatureController = new SignatureController();
            authorController = new AuthorController(signatureController);
            this.InitializeComponent();
            this.initializePenHandlers();

            //inicjalizacja wielkości pola do rysowania
            this.initRealSizeInkCanvas(130, 50);

            this.setNavbarColor();

            //ściągnięcie listy autorów żeby wyświetliło default
            this.updateAuthorCombobox();
            //this.authorCombobox.SelectedIndex = 0;       
        }

        private void initRealSizeInkCanvas(double mmWidth, double mmHeight)
        {
            RealScreenSizeCalculator calc = new RealScreenSizeCalculator();
            int width = (int) calc.convertToPixels(mmWidth);
            int height = (int)calc.convertToPixels(mmHeight);
            inkCanvasHolder.Height = height;
            inkCanvasHolder.Width = width;
            guideLine.X1 = 0.05 * width;
            guideLine.X2 = 0.95 * width;
            guideLine.Y1 = guideLine.Y2 = 0.7 * height;
        }

        private void setNavbarColor()
        {
            if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.ApplicationView"))
            {
                var titleBar = ApplicationView.GetForCurrentView().TitleBar;
                if (titleBar != null)
                {
                    titleBar.ButtonBackgroundColor = Color.FromArgb(255, 63, 81, 181);
                    titleBar.ButtonForegroundColor = Colors.White;
                    titleBar.BackgroundColor = Color.FromArgb(255, 63, 81, 181);
                    titleBar.ForegroundColor = Colors.White;
                }
            }
        }

        private void initializePenHandlers()
        {
            CoreInkIndependentInputSource core = CoreInkIndependentInputSource.Create(inkCanvas1.InkPresenter);

            //InkDrawingAttributes myAttributes = core.InkPresenter.CopyDefaultDrawingAttributes();
            //myAttributes.Color = Windows.UI.Colors.Blue;
            //myAttributes.PenTip = PenTipShape.Rectangle;
            //myAttributes.PenTipTransform = System.Numerics.Matrix3x2.CreateRotation((float)Math.PI / 4);
            //myAttributes.Size = new Size(2, 5);
            //core.InkPresenter.UpdateDefaultDrawingAttributes(myAttributes);

            core.PointerPressing += Core_PointerPressing;
            core.PointerReleasing += Core_PointerReleasing;
            core.PointerMoving += Core_PointerMoving;

            inkCanvas1.InkPresenter.InputDeviceTypes = CoreInputDeviceTypes.Mouse | CoreInputDeviceTypes.Pen;
        }

        //Event handler dla rysowania
        private void Core_PointerMoving(CoreInkIndependentInputSource sender, PointerEventArgs args)
        {
            updateInfoInLabel(label1, "Adam rysuje X: " + args.CurrentPoint.Position.X + ", Y: " + args.CurrentPoint.Position.Y + ", z mocą: " + args.CurrentPoint.Properties.Pressure + ", " + args.CurrentPoint.Properties.Twist);
        }

        //Event handler dla rysowania
        private void Core_PointerReleasing(CoreInkIndependentInputSource sender, PointerEventArgs args)
        {
            //Debug.WriteLine("Adam puścił");
        }

        private void Core_PointerPressing(CoreInkIndependentInputSource sender, PointerEventArgs args)
        {
            if (args.CurrentPoint.Properties.IsEraser)
            {
                Clear_Screen_Add_Strokes();
            }

            strokesCount = strokesCount + 1;
            
            updateInfoInLabel(strokesCountLabel, "Ilość naciśnięć: " + strokesCount);
            updateInfoInLabel(timeLastPressedLabel, "Czas ostatniego naciśnięcia w ms:  " + timer.ElapsedMilliseconds);
            //Debug.WriteLine("Adam wcisnął " + strokesCount + " razy" + "ostatni raz " + timer.ElapsedMilliseconds);
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

        //Action for button click
        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            Clear_Screen_Add_Strokes();
        }

        private async void Clear_Screen_Add_Strokes()
        {
            //Clears inkCanvas
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                inkCanvas1.InkPresenter.StrokeContainer.Clear();
            });

            strokesCount = 0; //tylko do wyświetlania, Signature class ma realcount
        }


        //Add signature
        private void addSignature()
        {
            List<InkStroke> strokes = new List<InkStroke>(inkCanvas1.InkPresenter.StrokeContainer.GetStrokes());

            if (strokes.Count == 0)
            {
                DisplayNoSignaturesDialog();
                return;
            }
            Debug.WriteLine("Adam dodaje podpis.");
            bool isOriginal = false;
            //IsChecked zwraca typ 'bool?', może posiadać wartość null stąd dodatkowy if tutaj sprawdzający czy nie zwraca nulla
            if (isOriginalCheckBox.IsChecked.HasValue) { isOriginal = isOriginalCheckBox.IsChecked.Value; }
            else { throw new Exception("isOriginal inputbox nie posiada wartości (jest wyłączony?)"); }

            try
            {
                var author = authorController.getAuthor(authorCombobox.SelectedItem.ToString());
                if(signatureController.addSignature(strokes, author, isOriginal) == null)
                {
                    DisplayWarningMessage("Błąd", "Próba dodania podpisu zakończona niepowodzeniem");
                }
                
            }
            catch (System.NullReferenceException)
            {
                DisplayWarningMessage("Błąd", "Próba dodania podpisu zakończona niepowodzeniem");
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
            else if(authorController.isContaining(authorInputBox.Text))
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

            authorCombobox.SelectedIndex = authorCombobox.Items.Count - 1;
        }

        private async void Button_ClickAsync(object sender, RoutedEventArgs e)
         //GUZIK WYKRESY
        {
            if (authorController.Empty())
            {
                DisplayNoSignaturesDialog();
                return;
            }
            CoreApplicationView newView = CoreApplication.CreateNewView();
            int newViewId = 0;
            await newView.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                Frame frame = new Frame();
                var signatures = this.signatureController.getSignatures();
                frame.Navigate(typeof(ShowSignatures), authorController);
                Window.Current.Content = frame;
                // You have to activate the window in order to show it later.
                Window.Current.Activate();

                newViewId = ApplicationView.GetForCurrentView().Id;
            });
            bool viewShown = await ApplicationViewSwitcher.TryShowAsStandaloneAsync(newViewId);
        }

        private async void DisplayNoSignaturesDialog()
        {
            ContentDialog dialog = new ContentDialog
            {
                Title = "Brak podpisu.",
                Content = "By wykonać akcję dodaj podpis.",
                CloseButtonText = "Zamknij",
                DefaultButton = ContentDialogButton.Close
            };

            ContentDialogResult result = await dialog.ShowAsync();
        }

        private async void DisplayWarningMessage(String title, String content)
        {
            ContentDialog dialog = new ContentDialog
            {
                Title = title,
                Content = content,
                CloseButtonText = "Zamknij",
                DefaultButton = ContentDialogButton.Close
            };

            ContentDialogResult result = await dialog.ShowAsync();
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                addSignature();
                Clear_Screen_Add_Strokes();
            }
            catch(ArgumentOutOfRangeException){ Debug.WriteLine("Nie można zapisać pustego podpisu!"); }

            
        }

        private void displayAuthorSignature()
        {
            this.showSignature.Children.Clear();
            if (!this.isOriginalCheckBox.IsChecked.Value)
            {
                if (!authorCombobox.Items.Count.Equals(0))
                {
                    var signature = authorController.getAuthor(authorCombobox.SelectedItem.ToString()).getSignature(0);
                    var ptsToDraw = new PointCollection();
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

        private void drawPoints(PointCollection points)
        {
            var polyline = new Polyline();
            polyline.Stroke = new SolidColorBrush(Windows.UI.Colors.Black);
            polyline.StrokeThickness = 1;
            polyline.Points = points;
            this.showSignature.Children.Add(polyline);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            displayAuthorSignature();
        }
    }
}