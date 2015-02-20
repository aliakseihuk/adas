using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Adas.Ui.Wpf.Annotations;

namespace Adas.Ui.Wpf.ViewModels
{
    class SetupViewModel : INotifyPropertyChanged
    {
        private string message_;

        public string Message
        {
            get { return message_; }
            set
            {
                if (message_ == value) return;
                message_ = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public enum Stage
    {
        SetupSource,
        SelectCalibration,
        Calibrate,
    }
}
