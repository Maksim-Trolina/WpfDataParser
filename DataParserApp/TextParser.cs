using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows.Media;

namespace DataParserApp
{
    internal class TextParser : ProgressBarUpdater, ITextParser
    {
        readonly IReadOnlyCollection<string> separators = new string[1] { ";" };
        public IEnumerable<Relation> ConvertToRelations(string text, Action<double> updateSplashScreenContent, BackgroundWorker worker, ref double progressValue)
        {
            int progressMaxValue = 100;
            var rows = SpliteByRows(text);

            LinkedList<Relation> relations = new();

            if (rows.Length == 0)
                return relations;

            double progressStep = (progressMaxValue - progressValue) / (rows.Length - 1);

            for (int i = 1; i < rows.Length; i++)
            {
                if (worker.CancellationPending)
                    return null;

                var relation = ConvertToObject(rows[i]);
                relations.AddLast(relation);

                UpdateProgressBar(ref progressValue, progressStep, updateSplashScreenContent);
            }

            return relations;
        }

        Relation ConvertToObject(string row)
        {
            var fields = ConvertToFields(row);
            var relation = new Relation
            {
                Date = Convert.ToDateTime(fields[0]),
                ObjectA = fields[1],
                TypeA = fields[2],
                ObjectB = fields[3],
                TypeB = fields[4],
                Direction = (Direction)Enum.Parse(typeof(Direction), fields[5], true),
                Color = (Color)ColorConverter.ConvertFromString(fields[6]),
                Intensity = double.Parse(fields[7]),
                LatitudeA = double.Parse(fields[8], CultureInfo.InvariantCulture.NumberFormat),
                LongitudeA = double.Parse(fields[9], CultureInfo.InvariantCulture.NumberFormat),
                LatitudeB = double.Parse(fields[10], CultureInfo.InvariantCulture.NumberFormat),
                LongitudeB = double.Parse(fields[11], CultureInfo.InvariantCulture.NumberFormat)
            };

            return relation;
        }
        string[] SpliteByRows(string text) => text.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
        string[] SpliteByFields(string row) => row.Split(separators.ToArray(), StringSplitOptions.RemoveEmptyEntries);
        string[] ConvertToFields(string row) => SpliteByFields(row).Where(x => x != "\r").ToArray();

    }
}
