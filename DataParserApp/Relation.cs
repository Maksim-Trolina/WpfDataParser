using System;
using System.Windows.Media;

namespace DataParserApp
{
    public enum Direction
    {
        In = 0,
        Out = 1
    }
    public class Relation
    {
        public DateTime Date { get; set; }
        public string ObjectA { get; set; }
        public string TypeA { get; set; }
        public string ObjectB { get; set; }
        public string TypeB { get; set; }
        public Direction Direction { get; set; }
        public Color Color { get; set; }
        public double Intensity { get; set; }
        public double LatitudeA { get; set; }
        public double LongitudeA { get; set; }
        public double LatitudeB { get; set; }
        public double LongitudeB { get; set; }
    }
}
