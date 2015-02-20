using System.IO;
using System.Windows;
using Adas.Core;
using Adas.Core.Camera;
using Microsoft.Win32;

namespace Adas.Ui.Wpf.Views.Setup
{
    /// <summary>
    ///     Interaction logic for SetupCalibrationControl.xaml
    /// </summary>
    public partial class SetupCalibrationControl
    {
        #region Private Fields

        private readonly ISetupWindow window_;

        #endregion

        #region Constructors

        public SetupCalibrationControl(ISetupWindow setupWindow)
        {
            window_ = setupWindow;
            InitializeComponent();

            SetCalibrateButtonAppearance(Model.Mode == SourceMode.Camera);
        }

        #endregion

        #region Public Properties

        public AdasModel Model
        {
            get { return window_.Controller.Model; }
        }

        #endregion

        #region Non-Public Methods/Events/Indexers

        private void CalibrateClick(object sender, RoutedEventArgs e)
        {
            window_.GoNextStage();
        }

        private void LoadFromFileClick(object sender, RoutedEventArgs e)
        {
            var opendialog = new OpenFileDialog
            {
                DefaultExt = "config",
                Filter = "Calibration Files(*.calib)|*.calib|All files (*.*)|*.*",
                InitialDirectory = Directory.GetCurrentDirectory(),
            };

            if (opendialog.ShowDialog() == true)
            {
                Model.CalibrationModel.CalibrationResult =
                    SerializationHelper<CalibrationStereoResult>.XmlDeserialize(opendialog.FileName);
            }
        }

        private void LoadSamplesClick(object sender, RoutedEventArgs e)
        {
            var opendialog = new OpenFileDialog
            {
                DefaultExt = "sti",
                Filter = "Stereo images Files(*.sti)|*.sti|All files (*.*)|*.*",
                InitialDirectory = Directory.GetCurrentDirectory(),
            };

            if (opendialog.ShowDialog() == true)
            {
                Model.CalibrationModel.CalibrationSamples =
                    SerializationHelper<StereoImageFileInfo[]>.XmlDeserialize(opendialog.FileName);
                SetCalibrateButtonAppearance(true);
            }
        }

        private void SetCalibrateButtonAppearance(bool showCalibrate)
        {
            if (showCalibrate)
            {
                _calibrateButton.Visibility = Visibility.Visible;
                _loadSamplesButton.Visibility = Visibility.Collapsed;
                _loadCalibButton.Visibility = Visibility.Visible;
            }
            else
            {
                _calibrateButton.Visibility = Visibility.Collapsed;
                _loadSamplesButton.Visibility = Visibility.Visible;
                _loadCalibButton.Visibility = Visibility.Collapsed;
            }
        }

        #endregion
    }
}