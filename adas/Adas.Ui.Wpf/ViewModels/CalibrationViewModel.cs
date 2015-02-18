using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Adas.Core.Camera;

namespace Adas.Ui.Wpf.ViewModels
{
    public class CalibrationViewModel
    {
        public CalibrationViewModel()
        {
            Settings = new CalibrationSettings();
            Delay = 1000;
        }

        public CalibrationViewModel(CalibrationSettings settings)
        {
            Settings = settings;
            Delay = 1000;
        }

        public CalibrationSettings Settings { get; private set; }

        public int Delay { get; set; }
    }
}
