using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Adas.Core.Camera;
using Adas.Ui.Wpf.Annotations;
using Emgu.CV.Structure;

namespace Adas.Ui.Wpf.ViewModels
{
    public class StereoImageViewModel: INotifyPropertyChanged
    {
        public string _title1;
        public string _title2;

        public StereoImageViewModel()
        {
            Title1 = "Image 1";
            Title2 = "Image 2";
        }

        public StereoImage<Bgr, byte> Image { get; set; }

        public string Title1 {
            get { return _title1; }
            set
            {
                if (_title1 == value) return;
                _title1 = value;
                OnPropertyChanged();
            }
        }
        public string Title2
        {
            get { return _title2; }
            set
            {
                if (_title2 == value) return;
                _title2 = value;
                OnPropertyChanged();
            }
        }

        public bool IsLive { get; set; }
        public bool ShowCustomize { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
