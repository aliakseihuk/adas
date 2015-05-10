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
            var gray = ProcessHoughTest(image);
            ImageViewer.Show(gray);
        }

        public static Image<Bgr, byte> ProcessHoughTest(Image<Bgr, byte> image)
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
                foreach (var element in dash.Elements)
                {
                    image.Draw(element, green, 3);
                }
            }
            return image;
        }
    }
}