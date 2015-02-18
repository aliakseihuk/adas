using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
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
using System.Windows.Shapes;
using System.Windows.Threading;
using Adas.Core.Camera;
using Adas.Ui.Wpf.ViewModels;

namespace Adas.Ui.Wpf.Views
{
    /// <summary>
    /// Interaction logic for CalibrationWindow.xaml
    /// </summary>
    public partial class CalibrationWindow : Window
    {
        private readonly StereoCamera _stereoCamera;
        private CalibrationMode _mode = CalibrationMode.Waiting;
        private readonly DispatcherTimer _background;
        private readonly Stopwatch _stopwatch = new Stopwatch();
        private readonly List<PointF[]> _imagesLeftCorners = new List<PointF[]>();
        private readonly List<PointF[]> _imagesRightCorners = new List<PointF[]>();
        private CalibrationStereoResult _result;

        
        public CalibrationWindow(StereoCamera camera)
        {
            _stereoCamera = camera;
            InitializeComponent();
            ViewModel = new CalibrationViewModel();
            DataContext = ViewModel;

            _background = new DispatcherTimer(DispatcherPriority.Background);
            _background.Tick += ProcessFrame;
        }

        public CalibrationViewModel ViewModel { get; set; }

        private void CalibrateClick(object sender, RoutedEventArgs e)
        {
            var button = (Button) sender;
            switch (_mode)
            {
                case CalibrationMode.Waiting:
                    PropertiesGrid.IsEnabled = false;
                    button.Content = "Stop";
                    _mode = CalibrationMode.GettingSamples;
                    _background.Start();
                    break;
                case CalibrationMode.GettingSamples:
                    PropertiesGrid.IsEnabled = true;
                    button.Content = "Getting Samples";
                    _background.Stop();
                    _mode = CalibrationMode.Waiting;
                    break;
                case CalibrationMode.Calibrating:
                    _result = StereoCalibration.Calibrate(ViewModel.Settings, _imagesLeftCorners.ToArray(),
                        _imagesRightCorners.ToArray());
                    button.Content = "Show";
                    _mode = CalibrationMode.Showing;
                    break;
                case CalibrationMode.Showing:
                    _background.Start();
                    break;
            }
        }

        private void ProcessFrame(object sender, EventArgs e)
        {
            if (_stereoCamera.IsEnabled)
            {
                var stereoImage = _stereoCamera.GetStereoImage();

                if (_mode == CalibrationMode.GettingSamples)
                {
                    PointF[] leftCorners;
                    PointF[] rightCorners;
                    if (StereoCalibration.FindChessboardCorners(stereoImage, ViewModel.Settings.PatternSize,
                        out leftCorners, out rightCorners))
                    {
                        StereoCalibration.DrawChessboardCorners(stereoImage, leftCorners, rightCorners);
                        if (!_stopwatch.IsRunning || _stopwatch.ElapsedMilliseconds > ViewModel.Delay)
                        {
                            _imagesLeftCorners.Add(leftCorners);
                            _imagesRightCorners.Add(rightCorners);
                            _stopwatch.Restart();
                        }
                    }

                    LeftImageHolder.Source = stereoImage.LeftImage.ToBitmap().ToBitmapSource();
                    RightImageHolder.Source = stereoImage.RightImage.ToBitmap().ToBitmapSource();
                    if (_imagesLeftCorners.Count == ViewModel.Settings.Count)
                    {
                        _mode = CalibrationMode.Calibrating;
                        ActionButton.Content = "Calibrate";
                        _background.Stop();
                        _stopwatch.Stop();
                    }
                }
                if (_mode == CalibrationMode.Showing)
                {
                    LeftImageHolder.Source = stereoImage.LeftImage.ToBitmap().ToBitmapSource();
                    RightImageHolder.Source = stereoImage.RightImage.ToBitmap().ToBitmapSource();
                    StereoCalibration.RemapStereoImage(stereoImage, _result);
                    LeftResultImageHolder.Source = stereoImage.LeftImage.ToBitmap().ToBitmapSource();
                    RightResultImageHolder.Source = stereoImage.RightImage.ToBitmap().ToBitmapSource();
                }
            }
        }
    }

    public enum CalibrationMode
    {
        GettingSamples,
        Calibrating,
        Waiting,
        Showing,
    }
}
