using System.Collections.Generic;
using System.Drawing;

namespace HaarPrecisionChecker
{
    class TestImage
    {
        public string Name { get; private set; }
        public List<Rectangle> SignsAreas { get; private set; }

        public TestImage(string name)
        {
            Name = name;
            SignsAreas = new List<Rectangle>();
        }
    }
}
