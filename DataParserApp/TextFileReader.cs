using System;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace DataParserApp
{
    internal class TextFileReader : ProgressBarUpdater, IFileReader
    {
        public string ReadText(string path, Action<double> updateSplashScreenContent, BackgroundWorker worker, ref double progressValue)
        {
            StringBuilder sourceText = new StringBuilder();
            double progressStep = 0.1;
            try
            {
                using (StreamReader sr = new StreamReader(path))
                {
                    while (!sr.EndOfStream)
                    {
                        if (worker.CancellationPending)
                            return string.Empty;

                        string line = sr.ReadLine();
                        sourceText.AppendLine(line);
                        UpdateProgressBar(ref progressValue, progressStep, updateSplashScreenContent);
                    }
                }
            }
            catch { }

            return sourceText.ToString();
        }
    }
}
