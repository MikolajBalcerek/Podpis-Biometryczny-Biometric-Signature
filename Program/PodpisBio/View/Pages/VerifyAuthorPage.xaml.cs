﻿using PodpisBio.Src.Author;
using PodpisBio.Src.FinalScore;
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
            VerifyButton.Height = ClearButton.Height = height / 2 -1;
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
            var signatureListAll = getOriginalSignaturesFromAuthor();
            //wagi  autora
            var weights = getAuthorWeights();

            List<Signature> signatureList = new List<Signature>();
            int i = 0;
            foreach (var s in signatureListAll)
            {
                if (i >= weights.getBasicCount()) { break; }
                signatureList.Add(s);
                i++;
            }

            //MIEJSCE NA METODĘ DO POROWNYWANIA PODPISOW
            SignVerification signVerification = new SignVerification();

            var finalScores = signVerification.init(signature, signatureList, weights);
            //MIEJSCE NA METODĘ DO POROWNYWANIA PODPISOW


            //wyczyszczenie ekranu
            inkCanvas1.InkPresenter.StrokeContainer.Clear();

            //wyświetlenie listy rezultatów
            this.resultList.Items.Clear();
            double bestResult = 0;
            foreach (var score in finalScores)
            {
                if(score > bestResult) { bestResult = score; }
                this.resultList.Items.Add(score);
            }

            //wyświetlenie rezultatu końcowego
            String result = "REZULTAT";
            this.resultText.Text = "Najlepszy rezultat: " + bestResult;
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

        private List<Signature> getOriginalSignaturesFromAuthor()
        {
            List<Signature> signatureList = new List<Signature>();
            if (this.authorCombobox.Items.Any())
            {
                signatureList = authorController.getAuthor(this.authorCombobox.SelectedItem.ToString()).getOriginalSignatures();
            }
            return signatureList;
        }

        private Weight getAuthorWeights()
        {
            return authorController.getAuthor(this.authorCombobox.SelectedItem.ToString()).getWeight();
        }
    }
}
