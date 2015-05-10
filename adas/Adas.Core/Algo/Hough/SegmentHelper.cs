using System;
using System.Drawing;
using System.Linq;
using Emgu.CV.Structure;

namespace Adas.Core.Algo.Hough
{
    internal class SegmentHelper
    {
        public static bool OneLineClose(LineSegment2D segment1, LineSegment2D segment2)
        {
            const double eps = 7.0;
            var d1 = DistanceHelper.LineToPointDistance2D(segment1.P1, segment1.P2, segment2.P1);
            var d2 = DistanceHelper.LineToPointDistance2D(segment1.P1, segment1.P2, segment2.P2);
            return d1 < eps || d2 < eps;
        }

        public static bool DistanceClose(LineSegment2D segment1, LineSegment2D segment2)
        {
            const double eps = 5.0;
            var d1 = DistanceHelper.SegmentToPointDistance2D(segment1, segment2.P1);
            var d2 = DistanceHelper.SegmentToPointDistance2D(segment1, segment2.P2);
            return d1 < eps || d2 < eps;
        }

        public static bool DirectionClose(PointF direction1, PointF direction2)
        {
            const double eps = 0.2;
            return Math.Abs(direction1.X) - Math.Abs(direction2.X) < eps &&
                   Math.Abs(direction1.Y) - Math.Abs(direction2.Y) < eps &&
                   ((Math.Sign(direction1.X) == Math.Sign(direction2.X) &&
                     Math.Sign(direction1.Y) == Math.Sign(direction2.Y)) ||
                    (Math.Sign(direction1.X) != Math.Sign(direction2.X) &&
                     Math.Sign(direction1.Y) != Math.Sign(direction2.Y)));
        }

        public static LineSegment2D MergeSegments(LineSegment2D[] segments)
        {
            var points = segments.Select(s => s.P1).Union(segments.Select(s => s.P2)).ToArray();

            var a = (double) points.Length*points.Sum(p => p.X*p.Y) - points.Sum(p => p.X)*points.Sum(p => p.Y);
            a /= points.Length*points.Sum(p => p.X*p.X) - Math.Pow(points.Sum(p => p.X), 2);
            var b = (points.Sum(p => p.Y) - a*points.Sum(p => p.X))/points.Length;

            var y1 = points.Min(p => p.Y);
            var y2 = points.Max(p => p.Y);
            var x1 = (int) ((y1 - b)/a);
            var x2 = (int) ((y2 - b)/a);

            return new LineSegment2D(new Point(x1, y1), new Point(x2, y2));
        }
    }
}