﻿using System.ComponentModel;
using System.Runtime.CompilerServices;
using Adas.Core.Camera;
using Adas.Ui.Wpf.Annotations;
using Emgu.CV.Structure;

namespace Adas.Ui.Wpf.ViewModels
{
    public class StereoImageViewModel : INotifyPropertyChanged
    {
        private StereoImage<Bgr, byte> _image;

        private bool _showLeft;
        private bool _showRight;
        private bool _showCustomize;
        private int _leftAngle;
        private int _rightAngle;

        public StereoImageViewModel()
        {
            _showLeft = true;
            _showRight = true;
            _showCustomize = true;
            _leftAngle = 0;
            _rightAngle = 0;
        }

        public StereoImage<Bgr, byte> Image
        {
            get { return _image; }
            set
            {
                if (Equals(value, _image)) return;
                _image = value;
                OnPropertyChanged();
            }
        }

        public bool ShowLeft
        {
            get { return _showLeft; }
            set
            {
                if (_showLeft == value) return;
                _showLeft = value;
                OnPropertyChanged();
            }
        }

        public bool ShowRight
        {
            get { return _showRight; }
            set
            {
                if (_showRight == value) return;
                _showRight = value;
                OnPropertyChanged();
            }
        }

        public bool ShowCustomize
        {
            get { return _showCustomize; }
            set
            {
                if (_showCustomize == value) return;
                _showCustomize = value;
                OnPropertyChanged();
            }
        }

        public int LeftAngle
        {
            get { return _leftAngle; }
            set
            {
                if (value == _leftAngle) return;
                _leftAngle = value;
                OnPropertyChanged();
            }
        }

        public int RightAngle
        {
            get { return _rightAngle; }
            set
            {
                if (value == _rightAngle) return;
                _rightAngle = value;
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
}