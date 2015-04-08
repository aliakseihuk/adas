using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Adas.Core;
using Adas.Core.Camera;
using Adas.Ui.Wpf.ViewModels;
using Emgu.CV;
using Emgu.CV.Structure;
using Microsoft.Win32;

namespace Adas.Ui.Wpf.Views.Calibration
{
    /// <summary>
    ///     Interaction logic for CalibrationWindow.xaml
    /// </summary>
    public partial class CalibrationWindow
    {
        private readonly CalibrationViewModel _viewModel;

        public CalibrationWindow(AdasController controller)
        {
            InitializeComponent();

            Controller = controller;

            _viewModel = Model.Calibrated
                ? new CalibrationViewModel(Model.CalibrationModel.CalibrationResult.Settings)
                : new CalibrationViewModel();
            _viewModel.CalibrationResult = Model.CalibrationModel.CalibrationResult;

            if (Model.Mode == SourceMode.Camera)
            {
                InitCameraMode();
            }
            else
            {
                InitImageMode();
            }

            DataContext = _viewModel;
        }

        public AdasController Controller { get; private set; }

        public AdasModel Model
        {
            get { return Controller.Model; }
        }


        private void ActionClick(object sender, RoutedEventArgs e)
        {
            if (Model.Mode == SourceMode.Image)
            {
                switch (_viewModel.Mode)
                {
                    case CalibrationMode.CollectingSamples:
                        SetImageWindowMode(CalibrationMode.ReadyCalibrating);
                        break;
                    case CalibrationMode.ReadyCalibrating:
                        SetImageWindowMode(CalibrationMode.Calibrating);
                        break;
                    case CalibrationMode.ShowCalibrated:
                        SetImageWindowMode(CalibrationMode.CollectingSamples);
                        break;
                }
                RefreshImage();
            }
            else
            {
                switch (_viewModel.Mode)
                {
                    case CalibrationMode.ShowNotCalibrated:
                        SetCameraWindowMode(CalibrationMode.CollectingSamples);
                        break;
                    case CalibrationMode.CollectingSamples:
                        SetCameraWindowMode(CalibrationMode.ShowNotCalibrated);
                        break;
                    case CalibrationMode.ReadyCalibrating:
                        SetCameraWindowMode(CalibrationMode.Calibrating);
                        break;
                    case CalibrationMode.ShowCalibrated:
                        SetCameraWindowMode(CalibrationMode.ShowNotCalibrated);
                        break;
                }
            }
        }

        private StereoImage<Bgr, byte> RemapImage(StereoImage<Bgr, byte> stereoImage)
        {
            StereoImage<Bgr, byte> copy = stereoImage.Copy();
            StereoCalibration.RemapStereoImage(copy, _viewModel.CalibrationResult);
            return copy;
        }

        private void Calibrate()
        {
            CalibrationCorners[] samples =
                _viewModel.Samples.Where(_ => _.IsCornersInitialized).Select(_ => _.Corners).ToArray();
            var settings = new CalibrationSettings
            {
                ChessboardHeight = _viewModel.ChessboardHeight,
                ChessboardWidth = _viewModel.ChessboardWidth,
                CellHeight = _viewModel.CellHeight,
                CellWidth = _viewModel.CellWidth,
                Count = _viewModel.Samples.Count(_ => _.IsCornersInitialized),
                ImageSize = _viewModel.Samples[0].StereoImage.LeftImage.Size
            };
            _viewModel.CalibrationResult = StereoCalibration.Calibrate(settings, samples);
        }

        private void StartClick(object sender, RoutedEventArgs e)
        {
            Controller.Model.CalibrationModel.CalibrationResult = _viewModel.CalibrationResult;
            var mainwindow = new MainWindow(Controller);
            mainwindow.Show();
            Close();
        }

        private void SaveClick(object sender, RoutedEventArgs e)
        {
            var savedialog = new SaveFileDialog
            {
                InitialDirectory = Directory.GetCurrentDirectory(),
                AddExtension = true,
                DefaultExt = "calib",
                Filter = "Calibration file (*.calib)|*.calib|All files (*.*)|*.*"
            };

            if (savedialog.ShowDialog() == true)
            {
                SerializationHelper<CalibrationStereoResult>.XmlSerialize(_viewModel.CalibrationResult,
                    savedialog.FileName);
            }
        }

        private void CopySettings()
        {
            CalibrationStereoResult result = _viewModel.CalibrationResult;

            _viewModel.ChessboardHeight = result.Settings.ChessboardHeight;
            _viewModel.ChessboardWidth = result.Settings.ChessboardWidth;
            _viewModel.CellHeight = result.Settings.CellHeight;
            _viewModel.CellWidth = result.Settings.CellWidth;
            _viewModel.Count = result.Settings.Count;
        }

        private void DrawImage(StereoImage<Bgr, byte> image)
        {
            SourceImage.ViewModel.Image = image;
            if (ResultImage.Visibility == Visibility.Visible)
            {
                StereoImage<Bgr, byte> calibratedImage = RemapImage(image);
                ResultImage.ViewModel.Image = calibratedImage;
            }
        }

        private void ShowResultSection(bool show)
        {
            if (show)
            {
                Grid.SetRowSpan(SourceImage, 1);
                ResultImage.Visibility = Visibility.Visible;
                SaveButton.Visibility = Visibility.Visible;
                StartButton.Visibility = Visibility.Visible;
            }
            else
            {
                Grid.SetRowSpan(SourceImage, 2);
                ResultImage.Visibility = Visibility.Collapsed;
                SaveButton.Visibility = Visibility.Collapsed;
                StartButton.Visibility = Visibility.Collapsed;
            }
        }
    }
}