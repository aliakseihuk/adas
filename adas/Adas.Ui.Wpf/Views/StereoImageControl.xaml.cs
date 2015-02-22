using System.ComponentModel;
using System.Drawing;
using System.Windows;
using Adas.Ui.Wpf.ViewModels;
using Emgu.CV.Structure;

namespace Adas.Ui.Wpf.Views
{
    /// <summary>
    ///     Interaction logic for StereoImageControl.xaml
    /// </summary>
    public partial class StereoImageControl
    {
        public readonly Bgr _bgrBlack;

        public StereoImageControl()
        {
            
            InitializeComponent();
            ViewModel = new StereoImageViewModel();
            DataContext = ViewModel;

            _bgrBlack = new Bgr(Color.Black);
            ViewModel.PropertyChanged += ViewModelOnPropertyChanged;
        }

        public StereoImageViewModel ViewModel { get; private set; }

        private void Refresh()
        {
            var hasImage = ViewModel.Image != null;
            LeftImageHolder.Source = hasImage && ViewModel.ShowLeft
                ? ViewModel.Image.LeftImage.Rotate(ViewModel.LeftAngle, _bgrBlack, false).ToBitmap().ToBitmapSource()
                : null;
            RightImageHolder.Source = hasImage && ViewModel.ShowRight
                ? ViewModel.Image.RightImage.Rotate(ViewModel.RightAngle, _bgrBlack, false).ToBitmap().ToBitmapSource()
                : null;
        }

        private void ViewModelOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            Refresh();
        }

        private void RotateLeft(object sender, RoutedEventArgs e)
        {
            ViewModel.LeftAngle = (ViewModel.LeftAngle + 90)%360;
        }

        private void RotateRight(object sender, RoutedEventArgs e)
        {
            ViewModel.RightAngle = (ViewModel.RightAngle + 90)%360;
        }
    }
}