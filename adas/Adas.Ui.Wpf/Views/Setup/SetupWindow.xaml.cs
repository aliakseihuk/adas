using Adas.Ui.Wpf.ViewModels;
using Adas.Ui.Wpf.Views.Calibration;

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
                    if (Controller.Model.SkipCalibration)
                    {
                        GoToStage(Stage.Main);
                    }
                    else
                    {
                        GoToStage(Stage.Calibrate);
                    }
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
                case Stage.Main:
                    var mainwindow = new MainWindow(Controller);
                    mainwindow.Show();
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
