using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
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
        private readonly AdasModel _model; 
        private readonly CalibrationViewModel _viewModel;

        public CalibrationWindow(AdasModel model)
        {
            InitializeComponent();

            _model = model;
            
            if (_model.CalibrationResult != null)
            {
                var result = (CalibrationStereoResult)_model.CalibrationResult.Clone();
                _viewModel = new CalibrationViewModel(result) { Mode = CalibrationMode.ShowCalibrated };
            }
            else
            {
                _viewModel = new CalibrationViewModel {Mode = CalibrationMode.ShowNotCalibrated};
            }
            DataContext = _viewModel;

            var dispatcherTimer = new DispatcherTimer(DispatcherPriority.Background);
            dispatcherTimer.Tick += ProcessImages;
            dispatcherTimer.Start();
        }

        

        private void CalibrateClick(object sender, RoutedEventArgs e)
        {
            var button = (Button) sender;
            switch (_viewModel.Mode)
            {
                case CalibrationMode.ShowNotCalibrated: //start to collect samples
                    _viewModel.Mode = CalibrationMode.GettingSamples;
                    PropertiesGrid.IsEnabled = false;
                    button.Content = "Restart";
                    break;

                case CalibrationMode.GettingSamples: //stop collect samples
                    _viewModel.Mode = CalibrationMode.ShowNotCalibrated;
                    PropertiesGrid.IsEnabled = true;
                    _viewModel.Samples.Clear();
                    button.Content = "Getting samples";
                    break;

                case CalibrationMode.ReadyCalibrating: //calibrate samples async
                    button.Content = "Calibrating...";
                    button.IsEnabled = false;

                    var task = new Task(() =>
                    {
                        var settings = (CalibrationSettings)_viewModel.Settings.Clone();
                        _viewModel.CalibrationResult = StereoCalibration.Calibrate(settings, _viewModel.Samples.Select(_ => _.Corners).ToArray());
                        _viewModel.Samples.Clear();
                        _viewModel.Mode = CalibrationMode.ShowCalibrated;
                        button.Content = "Restart";
                        button.IsEnabled = true;
                        PropertiesGrid.IsEnabled = true;
                    });
                    task.Start();
                    break;
                
                case CalibrationMode.ShowCalibrated: //reset result and start to collect samples again
                    _viewModel.CalibrationResult = null;
                    _viewModel.Mode = CalibrationMode.ShowNotCalibrated;
                    CalibrateClick(sender, e);
                    break;
            }
        }

        private void ProcessImages(object sender, EventArgs e)
        {
            if (_model.StereoCamera.IsEnabled)
            {
                var stereoImage = _model.StereoCamera.GetStereoImage();
                var corners = StereoCalibration.FindChessboardCorners(stereoImage, _viewModel.Settings.PatternSize);
                if (corners == null) return;

                StereoCalibration.DrawChessboardCorners(stereoImage, corners);

                if (_viewModel.Mode == CalibrationMode.ShowCalibrated)
                {
                    ShowCalibratedResult(stereoImage);
                    return;
                }

                bool showSample = false;
                if (_viewModel.Mode == CalibrationMode.GettingSamples)
                {
                    showSample = true;
                    if (_viewModel.AllowSaveCorners)
                    {
                        _viewModel.Samples.Add(new CalibratationSample(stereoImage, corners));
                        _viewModel.InvalidateSamples = true;
                        if (_viewModel.Settings.Count == _viewModel.Samples.Count)
                            _viewModel.Mode = CalibrationMode.ReadyCalibrating;
                    }
                }
                showSample |= _viewModel.Mode == CalibrationMode.ReadyCalibrating;
                showSample &= _viewModel.InvalidateSamples;
                if(showSample)
                    ShowNextSampleResult();
            }
        }

        private void ShowNextSampleResult()
        {
            var lastSample = _viewModel.Samples.Last();
            LeftResultImageHolder.Source = lastSample.StereoImage.LeftImage.ToBitmap().ToBitmapSource();
            RightResultImageHolder.Source = lastSample.StereoImage.RightImage.ToBitmap().ToBitmapSource();
            _viewModel.InvalidateSamples = false;
        }

        private void ShowCalibratedResult(StereoImage<Bgr, byte> stereoImage)
        {
            StereoCalibration.RemapStereoImage(stereoImage, _viewModel.CalibrationResult);
            LeftResultImageHolder.Source = stereoImage.LeftImage.ToBitmap().ToBitmapSource();
            RightResultImageHolder.Source = stereoImage.RightImage.ToBitmap().ToBitmapSource();
        }
    }
}
