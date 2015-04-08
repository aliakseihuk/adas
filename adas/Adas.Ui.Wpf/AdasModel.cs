using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Adas.Core.Camera;
using Emgu.CV.Structure;

namespace Adas.Ui.Wpf
{
    public class AdasModel
    {
        public AdasModel()
        {
            CalibrationModel = new CalibrationModel();
        }

        public StereoCamera StereoCamera { get; set; }
        public CalibrationModel CalibrationModel { get; set; }
        public StereoImage<Bgr, byte> Image { get; set; }

        public SourceMode Mode { get; set; }

        public bool Calibrated
        {
            get { return CalibrationModel.CalibrationResult != null; }
        }

        //todo: find more beautiful place for it
        public bool SkipCalibration { get; set; }
    }

    public class CalibrationModel
    {
        public CalibrationStereoResult CalibrationResult { get; set; }
        public StereoImageFileInfo[] CalibrationSamplesInfo { get; set; }
    }

    public enum SourceMode
    {
        Camera,
        Image
    }
}
