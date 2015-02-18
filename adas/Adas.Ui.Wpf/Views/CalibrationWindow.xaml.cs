using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Adas.Core.Camera;
using Adas.Ui.Wpf.ViewModels;
using Emgu.CV.Structure;

namespace Adas.Ui.Wpf.Views
{
    /// <summary>
    /// Interaction logic for CalibrationWindow.xaml
    /// </summary>
    public partial class CalibrationWindow
    {
        public CalibrationWindow(AdasModel model)
        {
            InitializeComponent();

            Model = model;
            
            if (Model.CalibrationResult != null)
            {
                var result = (CalibrationStereoResult)Model.CalibrationResult.Clone();
                ViewModel = new CalibrationViewModel(result) { Mode = CalibrationMode.ShowCalibrated };
            }
            else
            {
                ViewModel = new CalibrationViewModel {Mode = CalibrationMode.ShowNotCalibrated};
            }
            DataContext = ViewModel;

            DispatcherTimer.Start();
        }

        public CalibrationViewModel ViewModel { get; set; }

        private void CalibrateClick(object sender, RoutedEventArgs e)
        {
            var button = (Button) sender;
            switch (ViewModel.Mode)
            {
                case CalibrationMode.ShowNotCalibrated: //start to collect samples
                    ViewModel.Mode = CalibrationMode.GettingSamples;
                    PropertiesGrid.IsEnabled = false;
                    button.Content = "Restart";
                    break;

                case CalibrationMode.GettingSamples: //stop collect samples
                    ViewModel.Mode = CalibrationMode.ShowNotCalibrated;
                    PropertiesGrid.IsEnabled = true;
                    ViewModel.Samples.Clear();
                    button.Content = "Getting samples";
                    break;

                case CalibrationMode.ReadyCalibrating: //calibrate samples async
                    button.Content = "Calibrating...";
                    button.IsEnabled = false;

                    var task = new Task(() =>
                    {
                        var settings = (CalibrationSettings)ViewModel.Settings.Clone();
                        ViewModel.CalibrationResult = StereoCalibration.Calibrate(settings, ViewModel.Samples.Select(_ => _.Corners).ToArray());
                        ViewModel.Samples.Clear();
                        ViewModel.Mode = CalibrationMode.ShowCalibrated;
                        button.Content = "Restart";
                        button.IsEnabled = true;
                        PropertiesGrid.IsEnabled = true;
                    });
                    task.Start();
                    break;
                
                case CalibrationMode.ShowCalibrated: //reset result and start to collect samples again
                    ViewModel.CalibrationResult = null;
                    ViewModel.Mode = CalibrationMode.ShowNotCalibrated;
                    CalibrateClick(sender, e);
                    break;
            }
        }

        protected override void ProcessImages(object sender, EventArgs e)
        {
            if (Model.StereoCamera.IsEnabled)
            {
                var stereoImage = Model.StereoCamera.GetStereoImage();
                var corners = StereoCalibration.FindChessboardCorners(stereoImage, ViewModel.Settings.PatternSize);
                if (corners == null) return;

                StereoCalibration.DrawChessboardCorners(stereoImage, corners);

                if (ViewModel.Mode == CalibrationMode.ShowCalibrated)
                {
                    ShowCalibratedResult(stereoImage);
                    return;
                }

                bool showSample = false;
                if (ViewModel.Mode == CalibrationMode.GettingSamples)
                {
                    showSample = true;
                    if (ViewModel.AllowSaveCorners)
                    {
                        ViewModel.Samples.Add(new CalibratationSample(stereoImage, corners));
                        ViewModel.InvalidateSamples = true;
                        if (ViewModel.Settings.Count == ViewModel.Samples.Count)
                            ViewModel.Mode = CalibrationMode.ReadyCalibrating;
                    }
                }
                showSample |= ViewModel.Mode == CalibrationMode.ReadyCalibrating;
                showSample &= ViewModel.InvalidateSamples;
                if(showSample)
                    ShowNextSampleResult();
            }
        }

        private void ShowNextSampleResult()
        {
            var lastSample = ViewModel.Samples.Last();
            LeftResultImageHolder.Source = lastSample.StereoImage.LeftImage.ToBitmap().ToBitmapSource();
            RightResultImageHolder.Source = lastSample.StereoImage.RightImage.ToBitmap().ToBitmapSource();
            ViewModel.InvalidateSamples = false;
        }

        private void ShowCalibratedResult(StereoImage<Bgr, byte> stereoImage)
        {
            StereoCalibration.RemapStereoImage(stereoImage, ViewModel.CalibrationResult);
            LeftResultImageHolder.Source = stereoImage.LeftImage.ToBitmap().ToBitmapSource();
            RightResultImageHolder.Source = stereoImage.RightImage.ToBitmap().ToBitmapSource();
        }
    }
}
