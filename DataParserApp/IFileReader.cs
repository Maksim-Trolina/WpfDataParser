using System;
using System.ComponentModel;


namespace DataParserApp
{
    internal interface IFileReader
    {
        string ReadText(string path, Action<double> updateSplashScreenContent, BackgroundWorker worker, ref double progressValue);
    }
}
