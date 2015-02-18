using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Adas.Core;
using Adas.Core.Algo;
using Adas.Core.Camera;
using Adas.Ui.Wpf.ViewModels;

namespace Adas.Ui.Wpf.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly StereoCamera _stereoCamera = new StereoCamera();
        private readonly DispatcherTimer _background;

        private bool _isRun;
        
        public MainWindow()
        {
            MainViewModel = new MainViewModel();
            ComputeModel = new ComputeModel();

            InitializeComponent();
            DataContext = MainViewModel;

            _background = new DispatcherTimer(DispatcherPriority.Background);
            _background.Tick += ProcessFrames;
        }

        public MainViewModel MainViewModel { get; set; }
        public ComputeModel ComputeModel { get; set; }

        private void ProcessFrames(object sender, EventArgs e)
        {
            if (_stereoCamera.IsEnabled)
            {
                var stereoImage = _stereoCamera.GetStereoImage();
                LeftImageHolder.Source = stereoImage.LeftImage.ToBitmap().ToBitmapSource();
                RightImageHolder.Source = stereoImage.RightImage.ToBitmap().ToBitmapSource();
            }
        }

        private void ActionClick(object sender, RoutedEventArgs e)
        {
            if (!_isRun)
            {
                if (!_stereoCamera.IsEnabled) return;
                ActionButton.Content = "Stop";
                _background.Start();
            }
            else
            {
                ActionButton.Content = "Run";
                _background.Stop();
            }
            _isRun = !_isRun;
            RefreshButton.IsEnabled = _isRun;
        }

        private void RefreshClick(object sender, RoutedEventArgs e)
        {
            ComputeModel.SgbmModel = (StereoSgbmModel)MainViewModel.SgbmModel.Clone();
            ComputeModel.FlowModer = (OpticalFlowModel)MainViewModel.FlowModel.Clone();
        }

        #region Camera Items

        private void CameraInitClick(object sender, RoutedEventArgs e)
        {
            var item = (MenuItem) sender;
            _stereoCamera.Init();
            if (_stereoCamera.IsEnabled)
            {
                item.Header = "Reinitialize";
                foreach (var childItem in CameraItem.Items.OfType<MenuItem>())
                {
                    childItem.IsEnabled = true;
                }
            }
        }

        private void CameraResolutionClick(object sender, RoutedEventArgs e)
        {
            var item = (MenuItem)sender;
            var resolution = (Resolution) Enum.Parse(typeof (Resolution), "r" + item.Header, true);
            _stereoCamera.SetResolution(resolution);
        }

        private void CameraSwapClick(object sender, RoutedEventArgs e)
        {
            _stereoCamera.SwapCameras();
        }

        private void CameraMakeCalibrationClick(object sender, RoutedEventArgs e)
        {
            var calibrateWindow = new CalibrationWindow(_stereoCamera);
            calibrateWindow.ShowDialog();
        }

        private void CameraLoadCalibrationClick(object sender, RoutedEventArgs e)
        {

        }

        #endregion //Camera Items

        
    }
}
