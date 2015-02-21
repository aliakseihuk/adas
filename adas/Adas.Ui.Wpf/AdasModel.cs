using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Adas.Core.Camera;

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

        public SourceMode Mode { get; set; }
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
