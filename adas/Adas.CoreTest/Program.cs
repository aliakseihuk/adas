using System.Drawing;
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
            var size = image.Size;
            image.ROI = new Rectangle(150, 400, size.Width - 300, size.Height - 400);

            var pair = HoughLine.Compute(image);
            var solidlines = pair.Item1;
            var dashlines = pair.Item2;
            var red = new Bgr(Color.Red);
            var green = new Bgr(Color.Green);
            foreach (var line in solidlines)
            {
                image.Draw(line, red, 3);
            }

            foreach (var dash in dashlines)
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