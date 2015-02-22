using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Documents;
using Adas.Core.Algo;
using Adas.Core.Camera;
using Emgu.CV.Structure;

namespace Adas.Ui.Wpf.ViewModels
{
    public class MainViewModel
    {
        public const int Delay = 5000;
        public const int SaveDelay = 30000;

        private readonly Stopwatch grabStopwatch_ = new Stopwatch();
        private readonly Stopwatch saveStopwatch_ = new Stopwatch();
        private bool saveImage_;

        public MainViewModel()
        {
            SgbmModel = new StereoSgbmModel();
            FlowModel = new OpticalFlowModel();
            Images = new List<StereoImage<Bgr, byte>>();
        }


        public bool IsRunCamera { get; set; }
        public StereoSgbmModel SgbmModel { get; set; }
        public OpticalFlowModel FlowModel { get; set; }

        public List<StereoImage<Bgr, byte>> Images { get; private set; }

        public bool SaveImage
        {
            get { return saveImage_; }
            set
            {
                if (saveImage_ == value) return;
                saveImage_ = value;
                if (saveImage_)
                {
                    grabStopwatch_.Start();
                    saveStopwatch_.Start();
                }
                else
                {
                    grabStopwatch_.Stop();
                    saveStopwatch_.Stop();
                }
            }
        }

        public bool AllowGrabImage
        {
            get
            {
                bool allow = !grabStopwatch_.IsRunning || grabStopwatch_.ElapsedMilliseconds > Delay;
                if (allow)
                    grabStopwatch_.Restart();
                return allow;
            }
        }

        public bool AllowSaveImages
        {
            get
            {
                bool allow = !saveStopwatch_.IsRunning || saveStopwatch_.ElapsedMilliseconds > SaveDelay;
                if (allow)
                    saveStopwatch_.Restart();
                return allow;
            }
        }
    }
}
