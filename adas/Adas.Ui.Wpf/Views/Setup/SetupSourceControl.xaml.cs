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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Adas.Ui.Wpf.Views.Setup
{
    /// <summary>
    /// Interaction logic for SetupSourceControl.xaml
    /// </summary>
    public partial class SetupSourceControl
    {
        private readonly ISetupWindow window_;

        public SetupSourceControl(ISetupWindow setupWindow)
        {
            window_ = setupWindow;
            InitializeComponent();
        }

        public AdasModel Model
        {
            get { return window_.Controller.Model; }
        }

        private void CameraModeClick(object sender, RoutedEventArgs e)
        {
            window_.Controller.CreateCamera();
            if (window_.Controller.CameraIsEnabled())
            {
                Model.Mode = SourceMode.Camera;
                window_.GoNextStage();
            }
            else
            {
                window_.SetMessage("Camera cannot be initialized. Check usb connection.");
            }
        }

        private void ImageModeClick(object sender, RoutedEventArgs e)
        {
            Model.Mode = SourceMode.Image;
            window_.GoNextStage();
        }
    }

    
}
