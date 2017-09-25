using PodpisBio.Src.Author;
using PodpisBio.Src.FinalScore;
using System;
using System.Collections.Generic;
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

            int temp = 0;
            foreach(Signature s in originalAll)
            {
                if (temp < 5)
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

            foreach (Signature s in originalBasic)
            {
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

                this.resultList.Items.Add(result);
            }

            foreach (Signature s in originalTest)
            {
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

                this.resultList.Items.Add(result);
            }

            foreach (Signature s in fake)
            {
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

                this.resultList.Items.Add(result);
            }
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
