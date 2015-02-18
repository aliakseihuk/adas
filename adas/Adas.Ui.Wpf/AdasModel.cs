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
            StereoCamera = new StereoCamera();
        }

        public StereoCamera StereoCamera { get; set; }
        public CalibrationStereoResult CalibrationResult { get; set; }
    }
}
