using System.Drawing;
using System.Linq;

namespace HaarPrecisionChecker
{
    public class Statistic
    {
        private int allOriginalCount_;
        private int allDetectedCount_;

        private int originalDetectedCount_;
        private int falseAlarmCount_;

        public long Time { get; set; }

        public void ProcessSigns(Rectangle[] original, Rectangle[] detected)
        {
            allOriginalCount_ += original.Length;
            allDetectedCount_ += detected.Length;

            originalDetectedCount_ += original.Count(o => detected.Any(d => d.IntersectsWith(o)));
            falseAlarmCount_ += detected.Count(d => !original.Any(o => o.IntersectsWith(d)));
        }

        public double GetPrecision()
        {
            return ((double)originalDetectedCount_) / allOriginalCount_;
        }

        public double GetMistake()
        {
            return ((double)falseAlarmCount_) / allDetectedCount_;
        }
    }
}
