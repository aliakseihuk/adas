using System;
using System.Windows;
using System.Windows.Threading;
using Adas.Core.Camera;
using Adas.Ui.Wpf.ViewModels;
using Emgu.CV.Structure;

namespace Adas.Ui.Wpf.Views.Calibration
{
    public partial class CalibrationWindow
    {
        private void InitCameraMode()
        {
            SetCameraWindowMode(_viewModel.CalibrationResult != null
                ? CalibrationMode.ShowCalibrated
                : CalibrationMode.ShowNotCalibrated);

            var dispatcherTimer = new DispatcherTimer(DispatcherPriority.Background);
            dispatcherTimer.Tick += ProcessImages;
            dispatcherTimer.Start();
        }

        private void SetCameraWindowMode(CalibrationMode mode)
        {
            _viewModel.Mode = mode;
            switch (mode)
            {
                case CalibrationMode.ShowNotCalibrated:
                    _viewModel.CalibrationResult = null;
                    _viewModel.Samples.Clear();
                    ActionButton.Content = "Find Corners";
                    ShowResultSection(false);
                    Flyout.IsOpen = true;
                    CollectedCount.Visibility = Visibility.Collapsed;
                    break;
                case CalibrationMode.CollectingSamples:
                    ActionButton.Content = "Stop";
                    CollectedCount.Visibility = Visibility.Visible;
                    break;
                case CalibrationMode.ReadyCalibrating:
                    ActionButton.Content = "Calibrate";
                    CollectedCount.Visibility = Visibility.Collapsed;
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

        private void ProcessImages(object sender, EventArgs e)
        {
            if (Controller.CameraIsEnabled())
            {
                StereoImage<Bgr, byte> stereoImage = Model.StereoCamera.GetStereoImage();

                if (_viewModel.Mode != CalibrationMode.ShowNotCalibrated)
                {
                    CalibrationCorners corners = StereoCalibration.FindChessboardCorners(stereoImage,
                        _viewModel.PatternSize);
                    if (corners != null)
                    {
                        StereoCalibration.DrawChessboardCorners(stereoImage, corners);
                    }

                    DrawImage(stereoImage);

                    if (_viewModel.Mode == CalibrationMode.CollectingSamples && corners != null)
                    {
                        CollectedCount.Content = string.Format("Collected: {0}/{1}", _viewModel.Samples.Count,
                            _viewModel.Count);
                        if (_viewModel.AllowSaveCorners)
                        {
                            _viewModel.Samples.Add(new CalibrationSample(stereoImage, corners));
                            _viewModel.InvalidateSamples = true;
                            if (_viewModel.Count == _viewModel.Samples.Count)
                            {
                                CollectedCount.Content = string.Empty;
                                SetCameraWindowMode(CalibrationMode.ReadyCalibrating);
                            }
                            SampleList.Items.Refresh();
                        }
                    }
                }
                else
                {
                    DrawImage(stereoImage);
                }
            }
        }
    }
}