using System.ComponentModel;
using System.Runtime.CompilerServices;
using Adas.Ui.Wpf.Annotations;

namespace Adas.Ui.Wpf.ViewModels
{
    internal class SetupViewModel : INotifyPropertyChanged
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
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public enum Stage
    {
        SetupSource,
        SetupCameraParameters,
        SelectCalibration,
        Calibrate,
        Main
    }
}