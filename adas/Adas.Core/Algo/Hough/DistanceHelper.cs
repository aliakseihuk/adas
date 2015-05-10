using System;
using System.Drawing;
using Emgu.CV.Structure;

namespace Adas.Core.Algo.Hough
{
    public class DistanceHelper
    {
        //Compute the distance from A to B
        public static double Distance(Point point1, Point point2)
        {
            return Math.Sqrt(Math.Pow(point1.X - point2.X, 2) + Math.Pow(point1.Y - point2.Y, 2));
        }

        //Compute the distance from AB segment to C
        public static double SegmentToPointDistance2D(LineSegment2D segment, Point point)
        {
            return ElementToPointDistance2D(segment.P1, segment.P2, point, true);
        }

        //Compute the distance from AB to C
        //if isSegment is true, AB is a segment, not a line.
        public static double LineToPointDistance2D(Point point1, Point point2, Point point)
        {
            return ElementToPointDistance2D(point1, point2, point, false);
        }

        //Compute the distance from AB to C
        //if isSegment is true, AB is a segment, not a line.
        private static double ElementToPointDistance2D(Point point1, Point point2, Point point, bool isSegment)
        {
            var dist = CrossProduct(point1, point2, point)/Distance(point1, point2);
            var dot1 = DotProduct(point1, point2, point);
            if (isSegment)
            {
                if (dot1 > 0)
                    return Distance(point2, point);

                var dot2 = DotProduct(point2, point1, point);
                if (dot2 > 0)
                    return Distance(point1, point);
            }
            return Math.Abs(dist);
        }

        //Compute the dot product AB . AC
        private static double DotProduct(Point pointA, Point pointB, Point pointC)
        {
            var ab = new Point();
            var bc = new Point();
            ab.X = pointB.X - pointA.X;
            ab.Y = pointB.Y - pointA.Y;
            bc.X = pointC.X - pointB.X;
            bc.Y = pointC.Y - pointB.Y;
            double dot = ab.X*bc.X + ab.Y*bc.Y;

            return dot;
        }

        //Compute the cross product AB x AC
        private static double CrossProduct(Point pointA, Point pointB, Point pointC)
        {
            var ab = new Point();
            var ac = new Point();
            ab.X = pointB.X - pointA.X;
            ab.Y = pointB.Y - pointA.Y;
            ac.X = pointC.X - pointA.X;
            ac.Y = pointC.Y - pointA.Y;
            return ab.X*ac.Y - ab.Y*ac.X;
        }
    }
}