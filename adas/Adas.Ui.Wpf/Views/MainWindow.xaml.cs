﻿using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Xml.Serialization;
using Adas.Core.Camera;
using Adas.Ui.Wpf.ViewModels;
using Microsoft.Win32;

namespace Adas.Ui.Wpf.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly MainViewModel _viewModel;
        private readonly AdasModel _model;
        private readonly DispatcherTimer _dispatcherTimer;

        public MainWindow()
        {
            InitializeComponent();

            _model = new AdasModel();
            _viewModel = new MainViewModel();
            
            DataContext = _viewModel;
            _dispatcherTimer = new DispatcherTimer(DispatcherPriority.Background);
            _dispatcherTimer.Tick += ProcessImages;
        }
        
        private void ProcessImages(object sender, EventArgs e)
        {
            var stereoImage = _model.StereoCamera.GetStereoImage();
            SourceImage.ViewModel.Image = stereoImage;
            SourceImage.Refresh();
        }

        private void ActionClick(object sender, RoutedEventArgs e)
        {
            if (!_model.StereoCamera.IsEnabled) return;

            if (!_viewModel.IsRunCamera)
            {
                ActionButton.Content = "Stop Camera";
                _dispatcherTimer.Start();
            }
            else
            {
                ActionButton.Content = "Run Camera";
                _dispatcherTimer.Stop();
            }

            _viewModel.IsRunCamera = !_viewModel.IsRunCamera;
        }

        #region Camera Items

        private void CameraInitializeClick(object sender, RoutedEventArgs e)
        {
            var item = (MenuItem) sender;
            _model.StereoCamera.Init();
            if (_model.StereoCamera.IsEnabled)
            {
                item.Header = "Reinitialize";
                foreach (var childItem in CameraItem.Items.OfType<MenuItem>())
                {
                    childItem.IsEnabled = true;
                }
                SourceImage.ViewModel.Title1 = "Camera 1";
                SourceImage.ViewModel.Title2 = "Camera 2";
            }
        }

        private void CameraResolutionClick(object sender, RoutedEventArgs e)
        {
            var item = (MenuItem)sender;
            var resolution = (StereoCamera.Resolution) Enum.Parse(typeof (StereoCamera.Resolution), "r" + item.Header, true);
            _model.StereoCamera.SetResolution(resolution);
        }

        private void CameraSwapClick(object sender, RoutedEventArgs e)
        {
            _model.StereoCamera.SwapCameras();
        }

        private void CameraMakeCalibrationClick(object sender, RoutedEventArgs e)
        {
            //var calibrateWindow = new CalibrationWindow(_model);
            //calibrateWindow.ShowDialog();
        }

        private void CameraLoadCalibrationClick(object sender, RoutedEventArgs e)
        {
            var opendialog = new OpenFileDialog();
            opendialog.DefaultExt = "config";
            opendialog.Filter = "Calibration Files(*.calib)|*.calib|All files (*.*)|*.*";
            opendialog.InitialDirectory = Directory.GetCurrentDirectory();
            if (opendialog.ShowDialog() == true)
            {
                var mySerializer = new XmlSerializer(typeof (CalibrationStereoResult));
                using (var myFileStream = new FileStream(opendialog.FileName, FileMode.Open))
                {
                    _model.CalibrationModel.CalibrationResult = (CalibrationStereoResult)mySerializer.Deserialize(myFileStream);
                }
            }
        }

        #endregion //Camera Items

        
    }
}
