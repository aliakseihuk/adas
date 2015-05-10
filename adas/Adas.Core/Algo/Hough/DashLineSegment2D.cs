using Emgu.CV.Structure;

namespace Adas.Core.Algo.Hough
{
    public class DashLineSegment2D
    {
        private LineSegment2D[] elements_;

        public LineSegment2D[] Elements {
            get { return elements_; }
            set
            {
                elements_ = value;
                AsSolid = SegmentHelper.MergeSegments(elements_);
            }
        }

        public LineSegment2D AsSolid { get; private set; }
    }
}