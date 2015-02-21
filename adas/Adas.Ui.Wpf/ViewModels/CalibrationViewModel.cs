using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using Adas.Core.Camera;
using Adas.Ui.Wpf.Annotations;
using Emgu.CV.Structure;

namespace Adas.Ui.Wpf.ViewModels
{
    public class CalibrationViewModel : INotifyPropertyChanged
    {
        private readonly Stopwatch stopwatch_ = new Stopwatch();

        private float _cellHeight;
        private float _cellWidth;
        private int _chessboardHeight;
        private int _chessboardWidth;
        private int _count;
        private int _delay;
        private Size _patternSize;

        public CalibrationViewModel()
        {
            Count = 20;
            ChessboardHeight = 6;
            ChessboardWidth = 9;
            CellHeight = 20.0F;
            CellWidth = 20.0F;

            Delay = 1000;
            Samples = new List<CalibrationSample>();
        }

        public CalibrationViewModel(CalibrationSettings settings) : this()
        {
            Count = settings.Count;
            ChessboardHeight = settings.ChessboardHeight;
            ChessboardWidth = settings.ChessboardWidth;
            CellHeight = settings.CellHeight;
            CellWidth = settings.CellWidth;
        }

        public int Count
        {
            get { return _count; }
            set
            {
                if (value == _count) return;
                _count = value;
                OnPropertyChanged();
            }
        }

        public int ChessboardHeight
        {
            get { return _chessboardHeight; }
            set
            {
                if (value == _chessboardHeight) return;
                _chessboardHeight = value;
                _patternSize = new Size(_chessboardWidth, _chessboardHeight);
            }
        }

        public int ChessboardWidth
        {
            get { return _chessboardWidth; }
            set
            {
                if (value == _chessboardWidth) return;
                _chessboardWidth = value;
                _patternSize = new Size(_chessboardWidth, _chessboardHeight);
            }
        }

        public Size PatternSize
        {
            get { return _patternSize; }
        }

        public float CellHeight
        {
            get { return _cellHeight; }
            set
            {
                if (value.Equals(_cellHeight)) return;
                _cellHeight = value;
                OnPropertyChanged();
            }
        }

        public float CellWidth
        {
            get { return _cellWidth; }
            set
            {
                if (value.Equals(_cellWidth)) return;
                _cellWidth = value;
                OnPropertyChanged();
            }
        }

        public int Delay
        {
            get { return _delay; }
            set
            {
                if (value == _delay) return;
                _delay = value;
                OnPropertyChanged();
            }
        }

        public CalibrationMode Mode { get; set; }
        public CalibrationStereoResult CalibrationResult { get; set; }

        public List<CalibrationSample> Samples { get; private set; }
        public bool InvalidateSamples { get; set; }

        public bool AllowSaveCorners
        {
            get
            {
                bool allow = !stopwatch_.IsRunning || stopwatch_.ElapsedMilliseconds > Delay;
                if (allow)
                    stopwatch_.Restart();
                return allow;
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

    public class CalibrationSample
    {
        public CalibrationSample(StereoImageFileInfo fileInfo)
        {
            StereoImage = StereoImage<Bgr, byte>.Load(fileInfo);
        }

        public CalibrationSample(StereoImage<Bgr, byte> image, CalibrationCorners corners)
        {
            StereoImage = image;
            Corners = corners;
            IsCornersInitialized = true;
        }

        public StereoImage<Bgr, byte> StereoImage { get; private set; }
        public CalibrationCorners Corners { get; set; }

        public bool IsCornersInitialized { get; set; }

        public override string ToString()
        {
            return StereoImage.Name;
        }
    }

    public enum CalibrationMode
    {
        CollectingSamples,
        ReadyCalibrating,
        ShowNotCalibrated,
        Calibrating,
        ShowCalibrated,
    }
}