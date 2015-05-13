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
using Emgu.CV;

namespace Adas.CoreVideoTest
{
    public partial class VideoForm : Form
    {
        private readonly Capture capture_;

        public VideoForm()
        {
            InitializeComponent();
            capture_ = new Capture("d:\\4lera\\testvideo2.avi");
            Application.Idle += UpdateImage;
        }

        private void UpdateImage(object sender, EventArgs eventArgs)
        {
            if (capture_.Grab())
            {
                var frame = capture_.QueryFrame();
                CoreTest.Program.ProcessHoughTest(frame);
                pictureBox1.Image = frame.Bitmap;
                Thread.Sleep(80);
            }
            else
            {
                Application.Idle -= UpdateImage;
            }
        }
    }
}
