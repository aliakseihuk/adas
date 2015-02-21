using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Adas.Core.Camera;
using Emgu.CV;

namespace Adas.Ui.Wpf
{
    public class AdasController
    {
        public AdasController()
        {
            Model = new AdasModel();
            Model.Mode = SourceMode.Image;
        }

        public AdasModel Model { get; private set; }

        public void CreateCamera()
        {
            Model.StereoCamera = new StereoCamera();
            if (Model.StereoCamera.Init())
            {
            }
        }
        
        public bool CameraIsEnabled()
        {
            return Model.StereoCamera != null && Model.StereoCamera.IsEnabled;
        }

        public void SwapCameras()
        {
            if (CameraIsEnabled())
                Model.StereoCamera.SwapCameras();
        }

        public void SetCameraResolution(string resolutionString)
        {
            var resolution =
                (StereoCamera.Resolution) Enum.Parse(typeof (StereoCamera.Resolution), "r" + resolutionString, true);
            Model.StereoCamera.SetResolution(resolution);
        }
    }
}
