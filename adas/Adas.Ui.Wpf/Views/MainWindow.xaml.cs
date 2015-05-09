using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Xml.Serialization;
using Adas.Core;
using Adas.Core.Algo;
using Adas.Core.Camera;
using Adas.Ui.Wpf.ViewModels;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Microsoft.Win32;
using OpticalFlow = Adas.Core.Algo.OpticalFlow;

namespace Adas.Ui.Wpf.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly MainViewModel _viewModel;
        private readonly DispatcherTimer _dispatcherTimer;

        public MainWindow(AdasController controller)
        {
            Controller = controller;
            InitializeComponent();

            _viewModel = new MainViewModel();
            
            DataContext = _viewModel;
            _dispatcherTimer = new DispatcherTimer(DispatcherPriority.ApplicationIdle);
            _dispatcherTimer.Tick += ProcessImages;
        }

        private AdasController Controller { get; set; }

        private AdasModel Model
        {
            get { return Controller.Model; }
        }
        
        private void ProcessImages(object sender, EventArgs e)
        {
            StereoImage<Bgr, byte> image = null;
            if (Model.Mode == SourceMode.Camera)
            {
                if (Controller.CameraIsEnabled())
                {
                    //image = Controller.GeatedStereoImage();
                }
            }
            else
            {
                if (Model.Image != null)
                {
                    image = Model.Image;
                }
            }

            if (image == null)
                return;

            SourceImage.ViewModel.Image = image;
            var temp = new StereoImage<Bgr, byte>();

            if (ResultImage.ViewModel.ShowLeft)
            {
                var model = (StereoSgbmModel) _viewModel.SgbmModel.Clone();
                model.Image1 = image.LeftImage.Convert<Gray, byte>();
                model.Image2 = image.RightImage.Convert<Gray, byte>();
                var map = Stereo.Compute(model);
                var test = map.Convert<Bgr, byte>();
                CvInvoke.ApplyColorMap(test, test, ColorMapType.Rainbow);
                temp.LeftImage = test;
                test = test.SmoothGaussian(15);
                temp.RightImage = test;
            }
            if (ResultImage.ViewModel.ShowRight)
            {
                var model = (OpticalFlowModel)_viewModel.FlowModel.Clone();
                model.Image1 = image.LeftImage.Convert<Gray, byte>();
                model.Image2 = image.RightImage.Convert<Gray, byte>();
                var map = OpticalFlow.Compute(model);
                temp.RightImage = map.Convert<Bgr, byte>();
                if (temp.LeftImage == null)
                    temp.LeftImage = temp.RightImage;
            }
            ResultImage.ViewModel.Image = temp.Convert<Bgr, byte>();

            if (_viewModel.SaveImage)
            {
                if (_viewModel.AllowGrabImage)
                {
                    _viewModel.Images.Add(image);
                    ImageListBox.Items.Refresh();
                }
                if (_viewModel.AllowSaveImages)
                {
                    Controller.SaveImages(_viewModel.Images, DateTime.Now.Ticks.ToString());
                    _viewModel.Images.Clear();
                    ImageListBox.Items.Refresh();
                }
            }
        }

        private void ActionClick(object sender, RoutedEventArgs e)
        {
            if(!_dispatcherTimer.IsEnabled)
                _dispatcherTimer.Start();
            //if (!_model.StereoCamera.IsEnabled) return;

            //if (!_viewModel.IsRunCamera)
            //{
            //    ActionButton.Content = "Stop Camera";
            //    _dispatcherTimer.Start();
            //}
            //else
            //{
            //    ActionButton.Content = "Run Camera";
            //    _dispatcherTimer.Stop();
            //}

            //_viewModel.IsRunCamera = !_viewModel.IsRunCamera;
        }

        private void OpenClick(object sender, RoutedEventArgs e)
        {
            var infos = Controller.LoadStereoImageFileInfos();
            if (infos != null)
            {
                Model.Image = StereoImage<Bgr, byte>.Load(infos.First());
            }
        }
    }
}
