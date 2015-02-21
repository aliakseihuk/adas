using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Adas.Ui.Wpf.Views.Setup
{
    /// <summary>
    ///     Interaction logic for SetupCameraParametersControl.xaml
    /// </summary>
    public partial class SetupCameraParametersControl
    {
        private readonly ISetupWindow _window;

        public SetupCameraParametersControl(ISetupWindow setupWindow)
        {
            _window = setupWindow;
            InitializeComponent();
            CameraView.ViewModel.ShowCustomize = false;

            var dispatcherTimer = new DispatcherTimer(DispatcherPriority.Background);
            dispatcherTimer.Tick += ProcessImages;
            dispatcherTimer.Start();
        }

        public AdasModel Model
        {
            get { return _window.Controller.Model; }
        }

        public AdasController Controller
        {
            get { return _window.Controller; }
        }

        private void SwapClick(object sender, RoutedEventArgs e)
        {
            _window.Controller.SwapCameras();
        }

        private void NextClick(object sender, RoutedEventArgs e)
        {
            _window.GoNextStage();
        }

        private void ResolutionChecked(object sender, RoutedEventArgs e)
        {
            var content = ((RadioButton) sender).Content;
            if (content != null)
            {
                Controller.SetCameraResolution(content.ToString());
            }
        }

        private void ProcessImages(object sender, EventArgs e)
        {
            if (Controller.CameraIsEnabled())
            {
                var image = Model.StereoCamera.GetStereoImage();
                CameraView.ViewModel.Image = image;
            }
        }
    }
}