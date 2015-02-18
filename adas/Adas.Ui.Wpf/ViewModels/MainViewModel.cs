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


        public bool IsRunCamera { get; set; }
        public StereoSgbmModel SgbmModel { get; set; }
        public OpticalFlowModel FlowModel { get; set; }
    }
}
