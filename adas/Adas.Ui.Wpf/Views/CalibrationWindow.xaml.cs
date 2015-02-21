using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Xml.Serialization;
using Adas.Core.Camera;
using Adas.Ui.Wpf.ViewModels;
using Emgu.CV;
using Emgu.CV.Structure;
using Microsoft.Win32;

namespace Adas.Ui.Wpf.Views
{
    /// <summary>
    /// Interaction logic for CalibrationWindow.xaml
    /// </summary>
    public partial class CalibrationWindow
    {
        private readonly AdasModel _model;
        private readonly AdasController _controller;
        private readonly CalibrationViewModel _viewModel;

        public CalibrationWindow(AdasController controller)
        {
            InitializeComponent();

            Controller = controller;
            
            if (_model.CalibrationModel.CalibrationResult != null)
            {
                var result = (CalibrationStereoResult)_model.CalibrationModel.CalibrationResult.Clone();
                _viewModel = new CalibrationViewModel(result);
                SetMode(CalibrationMode.ShowCalibrated);
            }
            else
            {
                _viewModel = new CalibrationViewModel();
                SetMode(CalibrationMode.ShowNotCalibrated);
            }
            DataContext = _viewModel;

            var dispatcherTimer = new DispatcherTimer(DispatcherPriority.Background);
            dispatcherTimer.Tick += ProcessImages;
            dispatcherTimer.Start();
        }

        public AdasController Controller { get; private set; }
        public AdasModel Model
        {
            get { return Controller.Model; }
        }


        private void CalibrateClick(object sender, RoutedEventArgs e)
        {
            var button = (Button) sender;
            switch (_viewModel.Mode)
            {
                case CalibrationMode.ShowNotCalibrated: //start to collect samples
                    SetMode(CalibrationMode.CollectingSamples);
                    break;

                case CalibrationMode.CollectingSamples: //stop collect samples
                    SetMode(CalibrationMode.ShowNotCalibrated);
                    break;
                
                case CalibrationMode.ShowCalibrated: //reset result and start to collect samples again
                    //_viewModel.CalibrationResult = null;
                    //_viewModel.Mode = CalibrationMode.ShowNotCalibrated;
                    //CalibrateClick(sender, e);
                    break;
            }
        }

        private void LoadSamplesFromFile(object sender, RoutedEventArgs e)
        {
            _flyout.IsOpen = true;
            return;
            var openFileDialog = new OpenFileDialog()
            {
                Title = "Get left images",
                InitialDirectory = Directory.GetCurrentDirectory(),
                Multiselect = true
            };
            if (openFileDialog.ShowDialog() != true) return;
            var leftfiles = openFileDialog.FileNames;
            openFileDialog.Title = "Get right images";
            if (openFileDialog.ShowDialog() != true) return;
            var rightfiles = openFileDialog.FileNames;
            if (leftfiles.Length == 0 || leftfiles.Length != rightfiles.Length) return;
            //for (var i = 0; i < leftfiles.Length; ++i)
            //{
            //    var stereoImage = new StereoImage<Bgr, byte>
            //    {
            //        LeftImage = new Image<Bgr, byte>(leftfiles[i]),
            //        RightImage = new Image<Bgr, byte>(rightfiles[i])
            //    };
            //    var corners = StereoCalibration.FindChessboardCorners(stereoImage, _viewModel.Settings.PatternSize);
            //    if (corners == null) continue;
            //    StereoCalibration.DrawChessboardCorners(stereoImage, corners);
            //    _viewModel.Samples.Add(new CalibratationSample(stereoImage, corners));
            //}
            //_viewModel.CalibrationResult = null;
            //_viewModel.Settings.Count = leftfiles.Length;
            //_viewModel.InvalidateSamples = true;
            //_viewModel.Mode = CalibrationMode.ReadyCalibrating;
        }

        private void ProcessImages(object sender, EventArgs e)
        {
            if (_controller.CameraIsEnabled())
            {
                var stereoImage = _model.StereoCamera.GetStereoImage();
                ShowImage(SourceImage, stereoImage);

                if (_viewModel.Mode == CalibrationMode.ShowCalibrated)
                {
                    ShowCalibratedResult(stereoImage);
                    return;
                }

                //var corners = StereoCalibration.FindChessboardCorners(stereoImage, _viewModel.Settings.PatternSize);
                //if (corners == null) return;

                //StereoCalibration.DrawChessboardCorners(stereoImage, corners);

                

                //bool showSample = false;
                //if (_viewModel.Mode == CalibrationMode.CollectingSamples)
                //{
                //    showSample = true;
                //    if (_viewModel.AllowSaveCorners)
                //    {
                //        _viewModel.Samples.Add(new CalibratationSample(stereoImage, corners));
                //        _viewModel.InvalidateSamples = true;
                //        if (_viewModel.Settings.Count == _viewModel.Samples.Count)
                //        {
                //            SetMode(CalibrationMode.ReadyCalibrating);
                //            return;
                //        }
                //    }
                //}
                //showSample |= _viewModel.Mode == CalibrationMode.ReadyCalibrating;
                //showSample &= _viewModel.InvalidateSamples;
                //if(showSample)
                //    ShowNextSampleResult();
            }
        }



        private void ShowNextSampleResult()
        {
            var lastSample = _viewModel.Samples.Last();
            ShowImage(ResultImage, lastSample.StereoImage);
            _viewModel.InvalidateSamples = false;
        }

        private void ShowCalibratedResult(StereoImage<Bgr, byte> stereoImage)
        {
            //StereoCalibration.RemapStereoImage(stereoImage, _viewModel.CalibrationResult);
            //ShowImage(ResultImage, stereoImage);
        }

        public void ShowImage(StereoImageControl control, StereoImage<Bgr, byte> image)
        {
            control.ViewModel.Image = image;
            control.Refresh();
        }

        private void SaveClick(object sender, RoutedEventArgs e)
        {

            var savedialog = new SaveFileDialog();
            savedialog.InitialDirectory = Directory.GetCurrentDirectory();
            savedialog.AddExtension = true;
            savedialog.DefaultExt = "calib";
            savedialog.DefaultExt = "Calibration file (*.calib)|*.calib|All files (*.*)|*.*";
            if (savedialog.ShowDialog() == true)
            {
                //var serializer = new XmlSerializer(typeof(CalibrationStereoResult));
                //using (var writer = new StreamWriter(savedialog.FileName, false))
                //{
                //    serializer.Serialize(writer, _viewModel.CalibrationResult);
                //    writer.Close();
                //}
            }
        }

        private void SetMode(CalibrationMode nextMode)
        {
            _viewModel.Mode = nextMode;
            switch (nextMode)
            {
                case CalibrationMode.ShowNotCalibrated:

                    //_viewModel.CalibrationResult = null;
                    //PropertiesGrid.IsEnabled = true;
                    //ActionButton.Content = "Collect Samples";
                    //SaveButton.IsEnabled = false;
                    break;

                case CalibrationMode.CollectingSamples:
                    PropertiesGrid.IsEnabled = false;
                    _viewModel.Samples.Clear();
                    ActionButton.Content = "Restart";
                    break;

                case CalibrationMode.ReadyCalibrating:
                    
                    ActionButton.Content = "Calibrating...";
                    ActionButton.IsEnabled = false;
                    
                    //var settings = (CalibrationSettings)_viewModel.Settings.Clone();
                    //_viewModel.CalibrationResult = StereoCalibration.Calibrate(settings, _viewModel.Samples.Select(_ => _.Corners).ToArray());
                    //_viewModel.Samples.Clear();
                    //_viewModel.Mode = CalibrationMode.ShowCalibrated;

                    SetMode(CalibrationMode.ShowCalibrated);
                    break;

                case CalibrationMode.ShowCalibrated:
                    ActionButton.Content = "Restart";
                    ActionButton.IsEnabled = true;
                    PropertiesGrid.IsEnabled = false;
                    SaveButton.IsEnabled = true;
                    break;
            }
        }
    }
}
