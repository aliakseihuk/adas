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
using Adas.Ui.Wpf.ViewModels;

namespace Adas.Ui.Wpf.Views
{
    /// <summary>
    /// Interaction logic for StereoImageControl.xaml
    /// </summary>
    public partial class StereoImageControl : UserControl
    {
        public StereoImageControl()
        {
            InitializeComponent();
            ViewModel = new StereoImageViewModel();
            DataContext = ViewModel;
        }

        public StereoImageViewModel ViewModel { get; private set; }

        public void Refresh()
        {
            LeftImageHolder.Source = ViewModel.Image.LeftImage.ToBitmap().ToBitmapSource();
            RightImageHolder.Source = ViewModel.Image.RightImage.ToBitmap().ToBitmapSource();
        }
    }
}
