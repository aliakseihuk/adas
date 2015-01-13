using System.Drawing;
using System.Linq;

namespace HaarPrecisionChecker
{
    public class Statistic
    {
        private readonly double factor_;
        private readonly int minNeighbours_;

        private int allDetectedCount_;
        private int allOriginalCount_;

        private int falseAlarmCount_;
        private int originalDetectedCount_;

        public Statistic(double factor, int minNeighbour)
        {
            factor_ = factor;
            minNeighbours_ = minNeighbour;
        }

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
            return ((double) originalDetectedCount_)/allOriginalCount_;
        }

        public double GetMistake()
        {
            return ((double) falseAlarmCount_)/allDetectedCount_;
        }

        public override string ToString()
        {
            return string.Format("F:{0:N2} | MN:{1:N0} | P:{2:N2}% | M:{3:N2}% | T:{4:N0}", factor_, minNeighbours_,
                GetPrecision()*100, GetMistake()*100, Time);
        }
    }
}