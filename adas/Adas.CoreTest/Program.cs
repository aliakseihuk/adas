using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms.VisualStyles;
using Adas.Core.Algo.Hough;
using Adas.Core.Algo.Window;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.UI;

namespace Adas.CoreTest
{
    public class Program
    {
        private static void Main(string[] args)
        {
            //var image = new Image<Bgr, byte>("Images/image1.png");
            //

            ProcessCarDetectionTest();
        }

        public static void ProcessCarDetectionTest()
        {
            var image = new Image<Gray, byte>("Images/disparity.png");

            var left = new LineSegment2D(new Point(61, 221), new Point(121, 175));
            var right = new LineSegment2D(new Point(253, 221), new Point(209, 175));

            ////*****draw lines*****
            //var red = new Bgr(Color.Red);
            //image.Draw(left, red, 1);
            //image.Draw(right, red, 1);

            var original = CalculateHistogram(image);
            var cutted = new float[256];
            Array.Copy(original, cutted, 256);

            var windows = WindowFlow.Compute(image.Size, new[] {left, right});
            var allValues = new List<float[]>();

            int counter = 0;
            foreach (var window in windows)
            {
                counter++;
                image.ROI = window;
                var values = CalculateHistogram(image);
                for (var i = 0; i < original.Length; ++i)
                {
                    if (original[i] > 0)
                        values[i] = (original[i] - values[i])/original[i];
                    else
                        values[i] = 1.0f;
                }
                if (AnalyzeHistogram(values))
                {
                    Console.WriteLine(counter);
                }

                allValues.Add(values);
            }

            //PrintData(allValues);


            //ImageViewer.Show(image);

        }

        public static float[] CalculateHistogram(Image<Gray, byte> image)
        {
            var histogram = new DenseHistogram(256, new RangeF(0.0f, 255.0f));

            histogram.Calculate(new[] {image}, true, null);
            var values = new float[256];
            histogram.MatND.ManagedArray.CopyTo(values, 0);
            return values;
        }

        public static bool AnalyzeHistogram(float[] values)
        {
            const double eps = 0.05;
            const int minObjectSize = 2;
            const int maxObjectrSize = 20;
            var lastMax = values.Length;
            var functionSize = 0;
            for (var i = values.Length-1; i >= 0; --i)
            {
                if (Math.Abs(1.0 - values[i]) < eps)
                {
                    if (functionSize > minObjectSize)
                    {
                        return true;
                    }
                    lastMax = i;
                }
                else
                {
                    if (functionSize > maxObjectrSize)
                        return false;
                    functionSize = lastMax - i;
                }
            }
            return false;
        }





        public static void PrintData(List<float[]> values)
        {
            using (var writer = new StreamWriter(new FileStream("output.txt", FileMode.OpenOrCreate, FileAccess.Write)))
            {
                foreach (var v in values)
                {
                    var str = string.Join(" ", v);
                    writer.WriteLine(str);
                }
            }
        }
    }
}