using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Adas.Core.Camera;
using Adas.Ui.Wpf.ViewModels;

namespace Adas.Ui.Wpf.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : StereoWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            Model = new AdasModel();
            ViewModel = new MainViewModel();
            
            DataContext = ViewModel;
        }

        public MainViewModel ViewModel { get; set; }

        protected override void ProcessImages(object sender, EventArgs e)
        {
            var stereoImage = Model.StereoCamera.GetStereoImage();
            LeftImageHolder.Source = stereoImage.LeftImage.ToBitmap().ToBitmapSource();
            RightImageHolder.Source = stereoImage.RightImage.ToBitmap().ToBitmapSource();
        }

        private void ActionClick(object sender, RoutedEventArgs e)
        {
            if (!Model.StereoCamera.IsEnabled) return;

            if (!ViewModel.IsRunCamera)
            {
                ActionButton.Content = "Stop Camera";
                DispatcherTimer.Start();
            }
            else
            {
                ActionButton.Content = "Run Camera";
                DispatcherTimer.Stop();
            }

            ViewModel.IsRunCamera = !ViewModel.IsRunCamera;
        }

        #region Camera Items

        private void CameraInitializeClick(object sender, RoutedEventArgs e)
        {
            var item = (MenuItem) sender;
            Model.StereoCamera.Init();
            if (Model.StereoCamera.IsEnabled)
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
            Model.StereoCamera.SetResolution(resolution);
        }

        private void CameraSwapClick(object sender, RoutedEventArgs e)
        {
            Model.StereoCamera.SwapCameras();
        }

        private void CameraMakeCalibrationClick(object sender, RoutedEventArgs e)
        {
            var calibrateWindow = new CalibrationWindow(Model);
            calibrateWindow.ShowDialog();
        }

        private void CameraLoadCalibrationClick(object sender, RoutedEventArgs e)
        {

        }

        #endregion //Camera Items

        
    }
}
