using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Emgu.CV;
using Emgu.CV.Structure;

namespace HaarPrecisionChecker
{
    class TestImage
    {
        public string Name { get; private set; }
        public List<Rectangle> SignsAreas { get; private set; }
        public List<Rectangle> DetectedAreas { get; private set; }

        public Image<Bgr, Byte> Image { get; set; }
        public Image<Gray, Byte> GrayImage { get; private set; }

        public TestImage(string name)
        {
            Name = name;
            SignsAreas = new List<Rectangle>();
            DetectedAreas = new List<Rectangle>();
        }

        public void LoadImages(string basepath)
        {
            var imagepath = Path.Combine(basepath, Name);
            Image = new Image<Bgr, Byte>(imagepath);
            GrayImage = Image.Convert<Gray, Byte>();
        }
    }
}
