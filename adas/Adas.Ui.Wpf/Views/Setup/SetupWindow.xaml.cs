using System;
using System.Collections.Generic;
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
using Adas.Ui.Wpf.Annotations;
using Adas.Ui.Wpf.ViewModels;
using Adas.Ui.Wpf.Views.Calibration;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace Adas.Ui.Wpf.Views.Setup
{
    /// <summary>
    /// Interaction logic for SetupWindow.xaml
    /// </summary>
    public partial class SetupWindow : ISetupWindow
    {
        private readonly SetupViewModel viewModel_;
        private Stage stage_;

        public SetupWindow()
        {
            InitializeComponent();

            Controller = new AdasController();
            viewModel_ = new SetupViewModel();
            DataContext = viewModel_;

            GoToStage(Stage.SetupSource);
        }

        public AdasController Controller { get; private set; }

        public void SetMessage(string message)
        {
            viewModel_.Message = message;
        }

        public void GoNextStage()
        {
            switch (stage_)
            {
                case Stage.SetupSource:
                    if (Controller.Model.Mode == SourceMode.Camera)
                    {
                        GoToStage(Stage.SetupCameraParameters);
                    }
                    else
                    {
                        GoToStage(Stage.SelectCalibration);
                    }
                    break;
                case Stage.SetupCameraParameters:
                    GoToStage(Stage.SelectCalibration);
                    break;
                case Stage.SelectCalibration:
                    GoToStage(Stage.Calibrate);
                    stage_ = Stage.SelectCalibration;
                    _controlHolder.Children.Add(new SetupCalibrationControl(this));
                    break;
            }
        }

        public void GoToStage(Stage stage)
        {
            stage_ = stage;
            _controlHolder.Children.Clear();
            switch (stage_)
            {
                case Stage.SetupSource:
                    _controlHolder.Children.Add(new SetupSourceControl(this));
                    break;
                case Stage.SetupCameraParameters:
                    _controlHolder.Children.Add(new SetupCameraParametersControl(this));
                    break;
                case Stage.SelectCalibration:
                    _controlHolder.Children.Add(new SetupCalibrationControl(this));
                    break;
                case Stage.Calibrate:
                    var calibrateWindow = new CalibrationWindow(Controller);
                    calibrateWindow.Show();
                    Close();
                    break;
            }
            SetMessage(string.Empty);
        }
    }

    public interface ISetupWindow
    {
        AdasController Controller { get; }
        void SetMessage(string message);
        void GoNextStage();
    }
}
