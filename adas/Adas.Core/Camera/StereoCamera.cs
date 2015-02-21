using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;
using DirectShowLib;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

namespace Adas.Core.Camera
{
    public class StereoCamera
    {
        public enum Resolution
        {
            R320X240,
            R640X480,
            R800X600,
            R1280X720
        }

        private readonly Capture[] _captures = new Capture[2];

        public bool IsEnabled { get; private set; }

        public bool Init()
        {
            DsDevice[] systemCameras = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);
            int initialized = 0;
            for (int i = 0; i < systemCameras.Length; ++i)
            {
                if (systemCameras[i].Name.Contains("Logitech"))
                {
                    _captures[initialized] = CreateCapture(i);
                    ++initialized;
                }

                if (initialized > 1)
                {
                    IsEnabled = true;
                    break;
                }
            }
            return IsEnabled;
        }
        
        public StereoImage<Bgr, byte> GetStereoImage()
        {
            if (!IsEnabled) return null;
            return new StereoImage<Bgr, byte>
            {
                Name = DateTime.Now.ToString("o"),
                LeftImage = GetFrame(0),
                RightImage = GetFrame(1)
            };
        }

        public void SwapCameras()
        {
            Capture temp = _captures[1];
            _captures[1] = _captures[0];
            _captures[0] = temp;
        }

        public void SetResolution(Resolution resolution)
        {
            if (!IsEnabled) return;
            SetResolution(_captures[0], resolution);
            SetResolution(_captures[1], resolution);
        }

        private void SetResolution(Capture capture, Resolution resolution)
        {
            int width = 0;
            int height = 0;
            switch (resolution)
            {
                case Resolution.R320X240:
                    width = 320;
                    height = 240;
                    break;
                case Resolution.R640X480:
                    width = 640;
                    height = 480;
                    break;
                case Resolution.R800X600:
                    width = 800;
                    height = 600;
                    break;
                case Resolution.R1280X720:
                    width = 1280;
                    height = 720;
                    break;
            }
            capture.SetCaptureProperty(CAP_PROP.CV_CAP_PROP_FRAME_WIDTH, width);
            capture.SetCaptureProperty(CAP_PROP.CV_CAP_PROP_FRAME_HEIGHT, height);
        }

        private Capture CreateCapture(int id)
        {
            var capture = new Capture(id);
            SetResolution(capture, Resolution.R640X480);
            return capture;
        }

        private Image<Bgr, byte> GetFrame(int capture)
        {
            return _captures[capture].QueryFrame();
        }
    }
}