using System;

namespace DataParserApp
{
    public class ProgressBarUpdater
    {
        protected void UpdateProgressBar(ref double currentProgressValue, double progressStep, Action<double> updateSplashScreenContent)
        {
            int progressMaxValue = 100;
            currentProgressValue = Math.Min(currentProgressValue + progressStep, progressMaxValue);
            updateSplashScreenContent(currentProgressValue);
        }
    }
}
