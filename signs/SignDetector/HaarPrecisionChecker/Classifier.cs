using System;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;

namespace HaarPrecisionChecker
{
    class Classifier: IDisposable
    {
        public const string CascadPath = @"..\..\cascad\haarcascade\cascade.xml";

        private readonly CascadeClassifier _internalClassifier;

        public Classifier()
        {
            _internalClassifier = new CascadeClassifier(CascadPath);
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
            return _internalClassifier.DetectMultiScale(image, ScaleFactor, MinNeighbours, MinSize, MaxSize);
        }

        public void Dispose()
        {
            _internalClassifier.Dispose();
        }
    }
}
