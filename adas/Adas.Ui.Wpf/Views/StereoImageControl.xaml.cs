using System;
using System.Collections.Generic;
using System.ComponentModel;
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

            ViewModel.PropertyChanged += ViewModelOnPropertyChanged;
        }
        
        public StereoImageViewModel ViewModel { get; private set; }

        private void Refresh()
        {
            if(ViewModel.ShowLeft)
                LeftImageHolder.Source = ViewModel.Image.LeftImage.ToBitmap().ToBitmapSource();
            else
            {
                LeftImageHolder.Source = null;
            }
            if(ViewModel.ShowRight)
                RightImageHolder.Source = ViewModel.Image.RightImage.ToBitmap().ToBitmapSource();
            else
            {
                RightImageHolder.Source = null;
            }
        }

        private void ViewModelOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            Refresh();
        }
    }
}
