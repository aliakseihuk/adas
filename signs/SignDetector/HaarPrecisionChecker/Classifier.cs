using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Emgu.CV;
using Emgu.CV.Structure;

namespace HaarPrecisionChecker
{
    class Classifier: IDisposable
    {
        public const string CascadPath = @"..\..\cascad\haarcascade\cascade.xml";

        private readonly CascadeClassifier internalClassifier_;

        public Classifier()
        {
            internalClassifier_ = new CascadeClassifier(CascadPath);
            ScaleFactor = 1.1;
            MinNeighbours = 6;
            MinSize = new Size(10, 10);
            MaxSize = Size.Empty;
        }

        public double ScaleFactor { get; set; }
        public int MinNeighbours { get; set; }
        public Size MinSize { get; set; }
        public Size MaxSize { get; set; }

        public Rectangle[] Detect(Image<Gray, byte> image)
        {
            return internalClassifier_.DetectMultiScale(image, ScaleFactor, MinNeighbours, MinSize, MaxSize);
        }

        public void Dispose()
        {
            internalClassifier_.Dispose();
        }
    }
}
