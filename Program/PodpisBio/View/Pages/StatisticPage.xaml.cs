using PodpisBio.Src.Author;
using PodpisBio.Src.FinalScore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
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
    public sealed partial class StatisticPage : Page
    {
        private AuthorController authorController;
        private SignatureController signatureController;
        public StatisticPage()
        {
            this.InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //base.OnNavigatedTo(e);
            this.authorController = (AuthorController)e.Parameter;
            this.signatureController = authorController.signatureController;

            //Zaktualizowanie listy autorów
            this.updateAuthorCombobox();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            this.resultList.Items.Clear();

            var originalAll = getOriginalSignaturesFromAuthor();
            var fake = getFakeSignaturesFromAuthor();
            var weights = getAuthorWeights();

            List<Signature> originalTest = new List<Signature>();
            List<Signature> originalBasic = new List<Signature>();

            //Oddzielenie oryginalnych podpisow bazowych od testowych do sprawdzenia (basicCount ustala sie w klasie Weight)
            int temp = 0;
            foreach(Signature s in originalAll)
            {
                if (temp < weights.getBasicCount())
                {
                    originalBasic.Add(s);
                }
                else
                {
                    originalTest.Add(s);
                }

                temp++;
            }

            SignVerification signVerification = new SignVerification();

            //int hmm = 1;
            //Weryfikacja podpisow branych jako baza do liczenia wag i weryfikacji (musi zwracac besta = 1)
            foreach (Signature s in originalBasic)
            {
                //Debug.WriteLine("Basic "+hmm);
                //hmm++;
                var finalScores = signVerification.init(s, originalBasic, weights);

                StringBuilder result = new StringBuilder();
                StringBuilder resultTemp = new StringBuilder();
                double best = 0;
                result.Append("Basic: ");
                foreach (var score in finalScores)
                {
                    resultTemp.Append(Math.Round(score, 3) + " ");
                    if (score > best) { best = Math.Round(score, 3); }
                }
                result.Append("Best = " + best + " | ");
                result.Append(resultTemp);
                Debug.WriteLine(result);
                this.resultList.Items.Add(result);
            }

            //hmm = 1;
            //Weryfikacja oryginalnych podpisow ktore sa w bazie danych (bez tych branych jako baza do wag i weryfikacji)
            foreach (Signature s in originalTest)
            {
                //Debug.WriteLine("Test "+hmm);
                //hmm++;
                var finalScores = signVerification.init(s, originalBasic, weights);

                StringBuilder result = new StringBuilder();
                StringBuilder resultTemp = new StringBuilder();
                double best = 0;
                result.Append("Original: ");
                foreach (var score in finalScores)
                {
                    resultTemp.Append(Math.Round(score,3) +" ");
                    if(score > best) { best = Math.Round(score, 3); }
                }
                result.Append("Best = " + best +" | ");
                result.Append(resultTemp);
                Debug.WriteLine(result);
                this.resultList.Items.Add(result);
            }
            //hmm = 1;
            //Weryfikacja podrobionych podpisow ktore sa w bazie danych
            foreach (Signature s in fake)
            {
                //Debug.WriteLine("Fake "+hmm);
                //hmm++;
                var finalScores = signVerification.init(s, originalBasic, weights);

                StringBuilder result = new StringBuilder();
                StringBuilder resultTemp = new StringBuilder();
                double best = 0;
                result.Append("Fake: ");
                foreach (var score in finalScores)
                {
                    resultTemp.Append(Math.Round(score, 3) + " ");
                    if (score > best) { best = Math.Round(score, 3); }
                }
                result.Append("Best = " + best + " | ");
                result.Append(resultTemp);
                Debug.WriteLine(result);
                this.resultList.Items.Add(result);
            }
        }

        private void DTWsButton_Click(Object sender, RoutedEventArgs e)
        {
            this.resultList.Items.Clear();

            var originalSignatures = getOriginalSignaturesFromAuthor();
            var fakeSignatures = getFakeSignaturesFromAuthor();
            DynamicTimeWrapping dtw = new DynamicTimeWrapping();

            //foreach (var s in originalSignatures)
            //    System.Diagnostics.Debug.WriteLine("org ma " + s.getAllOriginalPoints().Count + " punktów.");

            //foreach (var s in fakeSignatures)
            //    System.Diagnostics.Debug.WriteLine("fake ma " + s.getAllOriginalPoints().Count + " punktów.");
            System.Diagnostics.Debug.WriteLine("\n\nOryginalne podpisy\n");
            for (int i = 0; i < originalSignatures.Count; i++)
                for (int j = 0; j < i; j++)
                {
                    var result = dtw.calcSimilarity(originalSignatures[i], originalSignatures[j]);
                    System.Diagnostics.Debug.WriteLine("DTW dla orygnalnych to " + result);
                }
            System.Diagnostics.Debug.WriteLine("\nFałszywe podpisy\n");
            foreach (var orgSgn in originalSignatures)
                foreach (var fakeSgn in fakeSignatures)
                    System.Diagnostics.Debug.WriteLine("DTW dla fejków to " + dtw.calcSimilarity(orgSgn, fakeSgn));
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

        private List<Signature> getOriginalSignaturesFromAuthor()
        {
            List<Signature> signatureList = new List<Signature>();
            if (this.authorCombobox.Items.Any())
            {
                signatureList = authorController.getAuthor(this.authorCombobox.SelectedItem.ToString()).getOriginalSignatures();
            }
            return signatureList;
        }

        private List<Signature> getFakeSignaturesFromAuthor()
        {
            List<Signature> signatureList = new List<Signature>();
            if (this.authorCombobox.Items.Any())
            {
                signatureList = authorController.getAuthor(this.authorCombobox.SelectedItem.ToString()).getFakeSignatures();
            }
            return signatureList;
        }

        private Weight getAuthorWeights()
        {
            return authorController.getAuthor(this.authorCombobox.SelectedItem.ToString()).getWeight();
        }
    }
}
