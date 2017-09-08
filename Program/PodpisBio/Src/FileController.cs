using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.UI.Input.Inking;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using System.Diagnostics;
using Windows.UI.Xaml.Controls;

namespace PodpisBio.Src
{
    class FileController
    {
        private async void writeToFileAsync(String rawCsv)
        {
            var savePicker = new Windows.Storage.Pickers.FileSavePicker();
            savePicker.SuggestedStartLocation =
                Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
            // Dropdown of file types the user can save the file as
            savePicker.FileTypeChoices.Add("Plain Text", new List<string>() { ".csv" });
            // Default file name if the user does not type one in or select a file to replace
            savePicker.SuggestedFileName = "Nowy csv";

            Windows.Storage.StorageFile file = await savePicker.PickSaveFileAsync();
            if (file != null)
            {
                // Prevent updates to the remote version of the file until
                // we finish making changes and call CompleteUpdatesAsync.
                Windows.Storage.CachedFileManager.DeferUpdates(file);
                // write to file
                await Windows.Storage.FileIO.WriteTextAsync(file, rawCsv);
                // Let Windows know that we're finished changing the file so
                // the other app can update the remote version of the file.
                // Completing updates may require Windows to ask for user input.
                Windows.Storage.Provider.FileUpdateStatus status =
                    await Windows.Storage.CachedFileManager.CompleteUpdatesAsync(file);
                if (status == Windows.Storage.Provider.FileUpdateStatus.Complete)
                {
                    Debug.WriteLine("File " + file.Name + " was saved.");
                }
                else
                {
                    Debug.WriteLine("File " + file.Name + " was NOT saved.");
                }
            }
            else
            {
                Debug.WriteLine("Cancelled");
            }
        }

        private void createCSV(IReadOnlyList<InkStroke> strokes)
        {
            var csv = new StringBuilder();
            var rawCsv = "";
            foreach (var stroke in strokes)
            {
                foreach (var point in stroke.GetInkPoints())
                {
                    var newLine = string.Format(point.Position.X.ToString() + ";" + point.Position.Y.ToString() + ";" + point.Pressure.ToString() + ";" + point.Timestamp.ToString());

                    rawCsv = rawCsv + newLine + Environment.NewLine;

                }

                rawCsv = rawCsv + Environment.NewLine;
            }
            writeToFileAsync(rawCsv);
        }
  
        private async void saveGIF(InkStrokeContainer strokeContainer)
        {
            var strokes = strokeContainer.GetStrokes();

            if (strokes.Count > 0)
            {
                Windows.Storage.Pickers.FileSavePicker savePicker =
                    new Windows.Storage.Pickers.FileSavePicker();
                savePicker.SuggestedStartLocation =
                    Windows.Storage.Pickers.PickerLocationId.Desktop;
                savePicker.FileTypeChoices.Add(
                    "Obrazek",
                    new List<string>() { ".gif" });
                savePicker.DefaultFileExtension = ".gif";
                savePicker.SuggestedFileName = "Nowy gif";

                Windows.Storage.StorageFile file =
                    await savePicker.PickSaveFileAsync();
                // When chosen, picker returns a reference to the selected file.
                if (file != null)
                {
                    Windows.Storage.CachedFileManager.DeferUpdates(file);
                    IRandomAccessStream stream = await file.OpenAsync(Windows.Storage.FileAccessMode.ReadWrite);
                    using (IOutputStream outputStream = stream.GetOutputStreamAt(0))
                    {
                        await strokeContainer.SaveAsync(outputStream);
                        await outputStream.FlushAsync();
                    }
                    stream.Dispose();

                    Windows.Storage.Provider.FileUpdateStatus status =
                        await Windows.Storage.CachedFileManager.CompleteUpdatesAsync(file);

                    if (status == Windows.Storage.Provider.FileUpdateStatus.Complete)
                    {
                        Debug.WriteLine("File " + file.Name + " was saved.");
                    }
                    else
                    {
                        Debug.WriteLine("File " + file.Name + " was NOT saved.");
                    }
                }
                else
                {
                    Debug.WriteLine("Cancelled");
                }
            }
        }

        public void save(InkStrokeContainer strokeContainer)
        {
            this.createCSV(strokeContainer.GetStrokes());
            this.saveGIF(strokeContainer);
        }

    }
}
