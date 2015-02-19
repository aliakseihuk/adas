using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Adas.Core.Camera;
using Adas.Ui.Wpf.Annotations;
using Adas.Ui.Wpf.Views;
using Emgu.CV.Structure;

namespace Adas.Ui.Wpf.ViewModels
{
    public class CalibrationViewModel : INotifyPropertyChanged
    {
        private readonly Stopwatch stopwatch_ = new Stopwatch();

        public CalibrationViewModel() : this(null)
        {
        }

        public CalibrationViewModel(CalibrationStereoResult result)
        {
            CalibrationResult = result;
            Settings = result != null ? (CalibrationSettings) result.Settings.Clone() : new CalibrationSettings();
            Samples = new List<CalibratationSample>();
            Delay = 1000;
        }

        public CalibrationMode Mode { get; set; }
        public CalibrationSettings Settings { get; set; }
        public CalibrationStereoResult CalibrationResult { get; set; }
        
        public int Delay { get; set; }
        public List<CalibratationSample> Samples { get; private set; }
        public bool InvalidateSamples { get; set; }

        public bool AllowSaveCorners
        {
            get
            {
                var allow = !stopwatch_.IsRunning || stopwatch_.ElapsedMilliseconds > Delay;
                if(allow)
                    stopwatch_.Restart();
                return allow;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class CalibratationSample
    {
        public CalibratationSample(StereoImage<Bgr, byte> image, CalibrationCorners corners)
        {
            StereoImage = image;
            Corners = corners;
        }

        public StereoImage<Bgr, byte> StereoImage { get; private set; }
        public CalibrationCorners Corners { get; private set; }
    }

    public enum CalibrationMode
    {
        CollectingSamples,
        ReadyCalibrating,
        ShowNotCalibrated,
        ShowCalibrated,
    }
}
