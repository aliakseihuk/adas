using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Xml.Serialization;
using Adas.Core.Algo;
using Adas.Core.Camera;
using Adas.Ui.Wpf.ViewModels;
using Emgu.CV.Structure;
using Microsoft.Win32;

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
            _dispatcherTimer = new DispatcherTimer(DispatcherPriority.Background);
            _dispatcherTimer.Tick += ProcessImages;
        }

        private AdasController Controller { get; set; }

        private AdasModel Model
        {
            get { return Controller.Model; }
        }
        
        private void ProcessImages(object sender, EventArgs e)
        {
            if (Controller.CameraIsEnabled())
            {
                var image = Controller.GetCalibratedStereoImage();
                SourceImage.ViewModel.Image = image;

                var temp = new StereoImage<Gray, byte>();

                if (ResultImage.ViewModel.ShowLeft)
                {
                    var model = (StereoSgbmModel) _viewModel.SgbmModel.Clone();
                    model.Image1 = image.LeftImage.Convert<Gray, byte>();
                    model.Image2 = image.RightImage.Convert<Gray, byte>();
                    var map = Stereo.Compute(model);

                    temp.LeftImage = map;
                    temp.RightImage = map;
                    
                }
                if (ResultImage.ViewModel.ShowRight)
                {
                    var model = (OpticalFlowModel)_viewModel.FlowModel.Clone();
                    model.Image1 = image.LeftImage.Convert<Gray, byte>();
                    model.Image2 = image.RightImage.Convert<Gray, byte>();
                    var map = OpticalFlow.Compute(model);
                    temp.RightImage = map.Convert<Gray, byte>();
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
        }

        private void ActionClick(object sender, RoutedEventArgs e)
        {
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
    }
}
