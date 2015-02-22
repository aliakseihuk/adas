using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Adas.Core;
using Adas.Core.Algo;
using Adas.Core.Camera;
using Emgu.CV;
using Emgu.CV.Structure;

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
            Model.StereoCamera.Init();
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

        public StereoImage<Bgr, byte> GetCalibratedStereoImage()
        {
            var stereoImage = Model.StereoCamera.GetStereoImage();
            if (Model.CalibrationModel.CalibrationResult != null)
                StereoCalibration.RemapStereoImage(stereoImage, Model.CalibrationModel.CalibrationResult);
            return stereoImage;
        }

        public void SaveImages(List<StereoImage<Bgr, byte>> images, string foldername)
        {
            if (!Directory.Exists("Images"))
                Directory.CreateDirectory("Images");
            Directory.CreateDirectory(Path.Combine("Images", foldername));
            var infos = new List<StereoImageFileInfo>();
            foreach (var image in images)
            {
                var info = new StereoImageFileInfo(image);
                infos.Add(info);
                var fullpath = Path.Combine(Directory.GetCurrentDirectory(), "Images", foldername, info.LeftImagePath);
                image.LeftImage.ToBitmap().Save(fullpath, ImageFormat.Png);
                fullpath = Path.Combine(Directory.GetCurrentDirectory(), "Images", foldername, info.RightImagePath);
                image.RightImage.ToBitmap().Save(fullpath, ImageFormat.Png);
            }
            var infosArray = infos.ToArray();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "Images", foldername, "sample.sti");
            SerializationHelper<StereoImageFileInfo[]>.XmlSerialize(infosArray, path);
        }
    }
}
