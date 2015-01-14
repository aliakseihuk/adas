using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

namespace StereoCameraTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        [DllImport("gdi32")]
        static extern int DeleteObject(IntPtr o);

        private Capture cameraCapture1_;
        private Capture cameraCapture2_;
        private DispatcherTimer background_;

        public MainWindow()
        {
            InitializeComponent();
            cameraCapture1_ = new Capture(0);
            cameraCapture2_ = new Capture(1);

            background_ = new DispatcherTimer(DispatcherPriority.Background);
            background_.Tick += ProcessFrames;
            background_.Start();
        }

        void ProcessFrames(object sender, EventArgs e)
        {
            Image<Bgr, Byte> frame1 = cameraCapture1_.QueryFrame();
            Image<Bgr, Byte> frame2 = cameraCapture2_.QueryFrame();
            //frame1 = frame1.Resize(160, 120, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
            //frame2 = frame2.Resize(160, 120, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
            camera1Image_.Source = loadBitmap(frame1.Bitmap);
            camera2Image_.Source = loadBitmap(frame2.Bitmap);
            resultImage_.Source = loadBitmap(FindDisparity1(frame1, frame2).Bitmap);
        }

        public static BitmapSource loadBitmap(System.Drawing.Bitmap source)
        {
            IntPtr ip = source.GetHbitmap();
            BitmapSource bs;
            try
            {
                bs = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(ip,
                    IntPtr.Zero, Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions());
            }
            finally
            {
                DeleteObject(ip);
            }

            return bs;
        }

        private Image<Gray, short> FindDisparity1(Image<Bgr, Byte> image1, Image<Bgr, Byte> image2)
        {
            var disparity = new Image<Gray, short>(image1.Size);
            using (
                StereoSGBM stereoSolver = new StereoSGBM(-(int)minDispSlider_.Value, (int)numDispSlider_.Value, (int)sadWindowSizeSlider_.Value, (int)p1Slider_.Value, (int)p2Slider_.Value,
                    (int)disp12MaxDiffSlider_.Value, (int)preFilterCapSlider_.Value, (int)uniquenessRatioSlider_.Value, (int)speckleWindowSizeSlider_.Value, (int)speckleRangeSlider_.Value, StereoSGBM.Mode.SGBM)
                )
            {
                stereoSolver.FindStereoCorrespondence(image1.Convert<Gray, Byte>(), image2.Convert<Gray, Byte>(),
                    disparity);
            }
            return disparity;
        }
    }
}
