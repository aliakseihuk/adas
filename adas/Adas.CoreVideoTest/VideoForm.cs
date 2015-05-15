using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Adas.Core.Algo.Hough;
using Emgu.CV;
using Emgu.CV.Structure;

namespace Adas.CoreVideoTest
{
    public partial class VideoForm : Form
    {
        private readonly Capture capture_;
        private readonly CacheLineContainer cache_ = new CacheLineContainer();

        public VideoForm()
        {
            InitializeComponent();
            capture_ = new Capture("d:\\4lera\\testvideo2.avi");
            KeyDown += OnKeyDown;
        }

        private void OnKeyDown(object sender, KeyEventArgs keyEventArgs)
        {
            if (keyEventArgs.KeyCode == Keys.Space)
            {
                UpdateImage(null, null);
            }
            if (keyEventArgs.KeyCode == Keys.Enter)
            {
                Application.Idle += UpdateImage;
            }
        }

        private void UpdateImage(object sender, EventArgs eventArgs)
        {
            if (capture_.Grab())
            {
                var frame = capture_.QueryFrame();
                ProcessHoughTest(frame);
                pictureBox1.Image = frame.Bitmap;
                Thread.Sleep(100);
            }
            else
            {
                Application.Idle -= UpdateImage;
            }
        }
        private void ProcessHoughTest(Image<Bgr, byte> image)
        {
            const int leftMargin = 0;
            const int upMargin = 300;
            const int downMargin = 200;
            var size = image.Size;

            image.ROI = new Rectangle(leftMargin, upMargin, size.Width - leftMargin * 2,
                size.Height - upMargin - downMargin);

            HoughLines.PreprocessImage(image);

            var result = HoughLines.Compute(image);
            cache_.AddResult(result);
            result = cache_.GetCachedResult();

            result.MoveRoiResult(leftMargin, upMargin);
            image.ROI = Rectangle.Empty;
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
                //    image.Draw(element, green, 3);
                }
                image.Draw(dash.AsSolid, red, 3);
            }
        }
    }
}
