using System.ComponentModel;
using Adas.Ui.Wpf.ViewModels;

namespace Adas.Ui.Wpf.Views
{
    /// <summary>
    ///     Interaction logic for StereoImageControl.xaml
    /// </summary>
    public partial class StereoImageControl
    {
        public StereoImageControl()
        {
            InitializeComponent();
            ViewModel = new StereoImageViewModel();
            DataContext = ViewModel;

            ViewModel.PropertyChanged += ViewModelOnPropertyChanged;
        }

        public StereoImageViewModel ViewModel { get; private set; }

        private void Refresh()
        {
            LeftImageHolder.Source = ViewModel.ShowLeft ? ViewModel.Image.LeftImage.ToBitmap().ToBitmapSource() : null;
            RightImageHolder.Source = ViewModel.ShowRight
                ? ViewModel.Image.RightImage.ToBitmap().ToBitmapSource()
                : null;
        }

        private void ViewModelOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            Refresh();
        }
    }
}