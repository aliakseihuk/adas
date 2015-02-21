using System.Windows;
using System.Windows.Controls;
using Adas.Core.Camera;
using Adas.Ui.Wpf.ViewModels;

namespace Adas.Ui.Wpf.Views.Calibration
{
    public partial class CalibrationWindow
    {
        private void InitImageMode()
        {
            CountLabel.Visibility = Visibility.Collapsed;
            CountBox.Visibility = Visibility.Collapsed;
            DelayLabel.Visibility = Visibility.Collapsed;
            DelayBox.Visibility = Visibility.Collapsed;

            if (_viewModel.CalibrationResult != null)
            {
                LoadImages();
                FindChessboardCornersOnSamples();
                SetImageWindowMode(CalibrationMode.ShowCalibrated);
            }
            else
            {
                SetImageWindowMode(CalibrationMode.CollectingSamples);
            }
        }

        private void LoadImages()
        {
            _viewModel.Samples.Clear();
            var infos = Model.CalibrationModel.CalibrationSamplesInfo;
            foreach (var info in infos)
            {
                var sample = new CalibrationSample(info);
                _viewModel.Samples.Add(sample);
            }
            SampleList.Items.Refresh();
        }

        private void SetImageWindowMode(CalibrationMode mode)
        {
            _viewModel.Mode = mode;
            switch (mode)
            {
                case CalibrationMode.CollectingSamples:
                    _viewModel.CalibrationResult = null;
                    ActionButton.Content = "Find Corners";
                    Flyout.IsOpen = true;
                    ShowResultSection(false);
                    LoadImages();
                    break;
                case CalibrationMode.ReadyCalibrating:
                    FindChessboardCornersOnSamples();
                    ActionButton.Content = "Calibrate";
                    break;
                case CalibrationMode.Calibrating:
                    Calibrate();
                    SetImageWindowMode(CalibrationMode.ShowCalibrated);
                    break;
                case CalibrationMode.ShowCalibrated:
                    ActionButton.Content = "Recalibrate";
                    CopySettings();
                    ShowResultSection(true);
                    break;       
            }
        }

        private void SampleListSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshImage();
        }

        private void FindChessboardCornersOnSamples()
        {
            foreach (var sample in _viewModel.Samples)
            {
                var corners = StereoCalibration.FindChessboardCorners(sample.StereoImage, _viewModel.PatternSize);
                if (corners == null) continue;
                StereoCalibration.DrawChessboardCorners(sample.StereoImage, corners);
                sample.Corners = corners;
                sample.IsCornersInitialized = true;
            }
        }

        private void RefreshImage()
        {
            var sample = SampleList.SelectedItem as CalibrationSample;
            if (sample != null)
            {
                DrawImage(sample.StereoImage);
            }
        }
    }
}
