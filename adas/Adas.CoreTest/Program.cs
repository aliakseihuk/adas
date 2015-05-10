using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Adas.Core.Algo.Hough;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.UI;

namespace Adas.CoreTest
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var image = new Image<Bgr, byte>("Images/image1.png");
            var houghResult = ProcessHoughTest(image);
            ProcessWindowTest(image, houghResult);
            ImageViewer.Show(image);
        }

        public static HoughResult ProcessHoughTest(Image<Bgr, byte> image)
        {
            const int leftMargin = 150;
            const int upMargin = 350;
            var size = image.Size;
            
            image.ROI = new Rectangle(leftMargin, upMargin, size.Width - leftMargin*2, size.Height - upMargin);

            var result = HoughLines.Compute(image);

            image.ROI = Rectangle.Empty;
            result.MoveRoiResult(new Size(leftMargin, upMargin));
            
            var red = new Bgr(Color.Red);
            var green = new Bgr(Color.Green);
            foreach (var line in result.SolidLines)
            {
                image.Draw(line, red, 3);
            }

            foreach (var dash in result.DashLines)
            {
                //foreach (var element in dash.Elements)
                //{
                //    image.Draw(element, green, 3);
                //}

                image.Draw(dash.AsSolid, green, 3);
            }
            return result;
        }

        public static List<Rectangle> ProcessWindowTest(Image<Bgr, byte> image, HoughResult result)
        {
            const double scaleStep = 0.5;
            const double minScale = 0.1;
            const double ratio = 1.5;

            //need to sort road lanes and select the nearest left and right lanes only

            var left = result.DashLines.First().AsSolid;
            var right = result.SolidLines.First();

            left = RotateLineSegment2D(left);
            right = RotateLineSegment2D(right);


            //need to select right points
            //var nearLeft = left.P1;
            //var nearRight = right.P1;

            //var farLeft = left.P2;
            //var farRight = right.P2;
            //var nearLeft = new Point(204, 608);
            //var nearRight = new Point(905, 608);

            //var farLeft = new Point(538, 390);
            //var farRight = new Point(695, 390);
            var nearLeft = FindStartingPoint(left, image.Size);
            var nearRight = FindStartingPoint(right, image.Size);

            var farLeft = new Point(538, 390);
            var farRight = new Point(695, 390);

            //sync Y coord there...


            //


            var nearDistance = nearRight.X - nearLeft.X;
            var farDistance = farRight.X - farLeft.X;

            var nearMiddle = new Point(nearLeft.X + nearDistance/2, nearLeft.Y);
            var farMiddle = new Point(farLeft.X + farDistance/2, farLeft.Y);

            //var direction = new PointF((left.Direction.X + right.Direction.X),
            //    (left.Direction.Y + right.Direction.Y) / 2);
            var direction = new Point(farMiddle.X - nearMiddle.X, farMiddle.Y - nearMiddle.Y);
            var middleDistance = (float)Math.Sqrt(direction.X*direction.X + direction.Y*direction.Y);
            var normalizeDirection = new PointF(direction.X/middleDistance, direction.Y/middleDistance);
            middleDistance = (float)DistanceHelper.Distance(nearMiddle, farMiddle);
            //calculate step count
            var stepCount = 0;
            var temp = minScale;
            do
            {
                temp = temp/scaleStep;
                ++stepCount;
            } while (temp < 1);

            var coefficient = nearDistance*middleDistance/(nearDistance - farDistance);
            //var coefficient = nearDistance/(right.Direction.X - left.Direction.X);

            //var coefficient = nearDistance / (nearDistance + left.Direction.X - right.Direction.X);
            var scale = 1.0;
            var windows = new List<Rectangle>();
            for (var i = 0; i < stepCount; ++i)
            {
                var windowWidth = nearDistance*scale;
                var windowHeight = windowWidth/ratio;
                var length = (1.0 - scale)*coefficient;
                var position = new Point((int) (nearMiddle.X + normalizeDirection.X*length),
                    (int) (nearMiddle.Y + normalizeDirection.Y*length));
                var windowLeft = position.X - windowWidth/2;
                var windowDown = position.Y - windowHeight;
                windows.Add(new Rectangle((int) windowLeft, (int) windowDown, (int) windowWidth, (int) windowHeight));
                scale *= scaleStep;
            }  

            foreach (var window in windows)
            {
                image.Draw(window, new Bgr(Color.Blue), 2);
            }

            return windows;
        }

        public static LineSegment2D RotateLineSegment2D(LineSegment2D segment)
        {
            if (segment.P1.Y < segment.P2.Y)
            {
                segment = new LineSegment2D(segment.P2, segment.P1);
            }
            return segment;
        }

        public static Point FindStartingPoint(LineSegment2D segment, Size imageSize)
        {
            var maxY = imageSize.Height - 1;
            double distance = maxY - segment.P1.Y;
            var x = (int) (distance/segment.Direction.Y*segment.Direction.X + segment.P1.X);
            if (x >= 0 && x < imageSize.Width)
            {
                return new Point(x, maxY);
            }
            if (segment.Direction.X > 0)
            {
                distance = segment.P1.X;
                var y = (int) (distance/segment.Direction.X*segment.Direction.Y + segment.P1.Y);
                if (y >= 0 && y < imageSize.Height)
                {
                    return new Point(0, y);
                }
                return new Point(0, maxY);
            }
            else
            {
                var maxX = imageSize.Width - 1;
                distance = maxX - segment.P1.X;
                var y = (int) (-distance/segment.Direction.X*segment.Direction.Y + segment.P1.Y);
                if (y >= 0 && y < imageSize.Height)
                {
                    return new Point(maxX, y);
                }
                return new Point(maxX, maxY);
            }
        }
    }
}