using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Adas.Core.Algo.Hough;
using Emgu.CV;
using Emgu.CV.Structure;

namespace Adas.Core.Algo.Window
{
    public class WindowFlow
    {
        public static List<Rectangle> Compute(Size imageSize, LineSegment2D[] lines)
        {
            const double scaleStep = 0.5;
            const double minScale = 0.1;
            const double ratio = 1.5;

            var allLines = lines.Select(RotateLineSegment2D).ToArray();

            var left = default(LineSegment2D);
            var right = default(LineSegment2D);
            var leftPoint = new Point(int.MinValue, 0);
            var rightPoint = new Point(int.MaxValue, 0);

            var viewPoint = imageSize.Width/2;

            foreach (var line in allLines)
            {
                var start = FindStartingPoint(line, imageSize.Width, imageSize.Height, false);
                if (start.X >= leftPoint.X && start.X <= viewPoint)
                {
                    left = line;
                    leftPoint = start;
                }
                else if (start.X <= rightPoint.X && start.X > viewPoint)
                {
                    right = line;
                    rightPoint = start;
                }
            }

            var maxY = Math.Min(FindStartingPoint(left, imageSize.Width, imageSize.Height, true).Y,
                FindStartingPoint(right, imageSize.Width, imageSize.Height, true).Y);

            leftPoint = FindStartingPoint(left, imageSize.Width, maxY, false);
            rightPoint = FindStartingPoint(right, imageSize.Width, maxY, false);

            var intersection = Intersection(left, right);


            var windowSize = rightPoint.X - leftPoint.X;
            var windowMiddlePoint = new Point(leftPoint.X + windowSize/2, leftPoint.Y);
            var windowDirection =
                new PointF(-(left.Direction.X + right.Direction.X*left.Direction.Y/right.Direction.Y)/2,
                    -left.Direction.Y);
            var middleDistance =
                (float) Math.Sqrt(windowDirection.X*windowDirection.X + windowDirection.Y*windowDirection.Y);
            windowDirection = new PointF(windowDirection.X/middleDistance, windowDirection.Y/middleDistance);

            //calculate step count
            var stepCount = 0;
            var temp = minScale;
            do
            {
                temp = temp/scaleStep;
                ++stepCount;
            } while (temp < 1);

            var coefficient = DistanceHelper.Distance(windowMiddlePoint, intersection);

            var scale = 1.0;
            var windows = new List<Rectangle>();
            for (var i = 0; i < stepCount; ++i)
            {
                var windowWidth = windowSize*scale;
                var windowHeight = windowWidth/ratio;
                var length = (1.0 - scale)*coefficient;
                var position = new Point((int) (windowMiddlePoint.X + windowDirection.X*length),
                    (int) (windowMiddlePoint.Y + windowDirection.Y*length));
                var windowLeft = position.X - windowWidth/2;
                var windowDown = position.Y - windowHeight;
                windows.Add(new Rectangle((int) windowLeft, (int) windowDown, (int) windowWidth, (int) windowHeight));
                scale *= scaleStep;
            }

            return windows;
        }

        public static void DrawWindows(Image<Bgr, byte> image, List<Rectangle> windows)
        {
            foreach (var window in windows)
            {
                image.Draw(window, new Bgr(Color.Blue), 2);
            }
        }

        private static LineSegment2D RotateLineSegment2D(LineSegment2D segment)
        {
            if (segment.P1.Y < segment.P2.Y)
            {
                segment = new LineSegment2D(segment.P2, segment.P1);
            }
            return segment;
        }

        private static Point FindStartingPoint(LineSegment2D segment, int maxX, int maxY, bool crop)
        {
            double distance = maxY - segment.P1.Y;
            var x = (int) (distance/segment.Direction.Y*segment.Direction.X + segment.P1.X);
            if (!crop)
                return new Point(x, maxY);
            if (x >= 0 && x < maxX)
            {
                return new Point(x, maxY);
            }
            if (segment.Direction.X > 0)
            {
                distance = maxX - segment.P1.X;
                var y = (int) (distance/segment.Direction.X*segment.Direction.Y + segment.P1.Y);
                if (y >= 0 && y < maxY)
                {
                    return new Point(maxX, y);
                }
                return new Point(maxX, maxY);
            }
            else
            {
                distance = segment.P1.X;
                var y = (int) (-distance/segment.Direction.X*segment.Direction.Y + segment.P1.Y);
                if (y >= 0 && y < maxY)
                {
                    return new Point(0, y);
                }
                return new Point(0, maxY);
            }
        }

        private static Point Intersection(LineSegment2D segment1, LineSegment2D segment2)
        {
            var d1 = segment1.Direction;
            var d2 = segment2.Direction;
            var y = (d1.Y*d2.Y*(segment1.P1.X - segment2.P1.X) - d1.X*d2.Y*segment1.P1.Y + d1.Y*d2.X*segment2.P1.Y)/
                    (d1.Y*d2.X - d2.Y*d1.X);
            var x = d1.X*(y - segment1.P1.Y)/d1.Y + segment1.P1.X;
            return new Point((int) x, (int) y);
        }
    }
}