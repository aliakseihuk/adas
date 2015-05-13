using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV.UI;

namespace Adas.CoreTest
{
    class LaneVideoTest
    {
        public void Test()
        {
            var capture = new Emgu.CV.Capture("D:\\videofile.mp4");
            while (capture.Grab())
            {
                var image = capture.QueryFrame();
                Program.ProcessHoughTest(image);
                ImageViewer.Show(image);
                for (var i = 0; i < 100; ++i)
                {
                    capture.Grab();
                }
            }
        }
    }
}
