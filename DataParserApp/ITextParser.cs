using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace DataParserApp
{
    internal interface ITextParser
    {
        IEnumerable<Relation> ConvertToRelations(string text, Action<double> updateSplashScreenContent, BackgroundWorker worker, ref double progressValue);
    }
}
