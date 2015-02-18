using Adas.Core.Algo;

namespace Adas.Ui.Wpf.ViewModels
{
    public class MainViewModel
    {
        public MainViewModel()
        {
            SgbmModel = new StereoSgbmModel();
            FlowModel = new OpticalFlowModel();
        }

        public StereoSgbmModel SgbmModel { get; set; }
        public OpticalFlowModel FlowModel { get; set; }

    }
}
