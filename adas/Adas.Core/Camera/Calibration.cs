using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

namespace Adas.Core.Camera
{
    public class CalibrationCorners
    {
        public CalibrationCorners(PointF[] leftCorners, PointF[] rightCorners)
        {
            LeftCorners = leftCorners;
            RightCorners = rightCorners;
        }
    
        public PointF[] LeftCorners { get; private set; }
        public PointF[] RightCorners { get; private set; }
    }

    public class CalibrationSettings: ICloneable
    {
        private Size _patternSize;
        private int _chessboardHeight;
        private int _chessboardWidth;

        public CalibrationSettings()
        {
            Count = 20;
            ChessboardHeight = 6;
            ChessboardWidth = 9;
            CellHeight = 20.0F;
            CellWidth = 20.0F;
            ImageSize = new Size(640, 480);
        }

        public int Count { get; set; }

        public int ChessboardHeight {
            get { return _chessboardHeight; }
            set
            {
                _chessboardHeight = value;
                _patternSize = new Size(_chessboardWidth, _chessboardHeight);
            }
        }
        public int ChessboardWidth { 
            get { return _chessboardWidth; }
            set
            {
                _chessboardWidth = value;
                _patternSize = new Size(_chessboardWidth, _chessboardHeight);
            }
        }
        
        public float CellWidth { get; set; }
        public float CellHeight { get; set; }

        public Size PatternSize
        {
            get { return _patternSize; }
        }

        public Size ImageSize { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }

    public class CalibrationCameraResult
    {
        public CalibrationCameraResult()
        {
            IntrinsicParameters = new IntrinsicCameraParameters();

            R = new Matrix<double>(3, 3);
            P = new Matrix<double>(3, 4);
        }

        public IntrinsicCameraParameters IntrinsicParameters { get; set; }

        public Matrix<double> R { get; set; }
        public Matrix<double> P { get; set; }

        public Rectangle Rectangle { get; set; }
    }

    public class CalibrationStereoResult: ICloneable
    {
        public CalibrationStereoResult(CalibrationSettings settings)
        {
            Settings = settings;

            Camera1Result = new CalibrationCameraResult();
            Camera2Result = new CalibrationCameraResult();

            Q = new Matrix<double>(4, 4);
        }

        public CalibrationSettings Settings { get; private set; }

        public PointF[][] CornersPointsLeft { get; set; }
        public PointF[][] CornersPointsRight { get; set; }

        public CalibrationCameraResult Camera1Result { get; set; }
        public CalibrationCameraResult Camera2Result { get; set; }

        public Matrix<double> Q { get; set; }


        public object Clone()
        {
            return MemberwiseClone();
        }
    }


    public class StereoCalibration
    {
        public static void RemapStereoImage(StereoImage<Bgr, byte> image, CalibrationStereoResult calibrationStereoResult)
        {
            Matrix<float> mapX;
            Matrix<float> mapY;

            InitUndistortRectifyMap(
                calibrationStereoResult.Camera1Result,
                calibrationStereoResult.Settings.ImageSize,
                out mapX, out mapY);
            image.LeftImage = Remap(image.LeftImage, mapX, mapY);

            InitUndistortRectifyMap(
                calibrationStereoResult.Camera2Result,
                calibrationStereoResult.Settings.ImageSize,
                out mapX, out mapY);
            image.RightImage = Remap(image.RightImage, mapX, mapY);
        }

        public static CalibrationStereoResult Calibrate(CalibrationSettings settings, CalibrationCorners[] corners)
        {
            var calibrationResult = new CalibrationStereoResult(settings);

            var points = new MCvPoint3D32f[calibrationResult.Settings.Count][];
            for (var k = 0; k < calibrationResult.Settings.Count; ++k)
            {
                var objects = new List<MCvPoint3D32f>();
                for (var i = 0; i < settings.ChessboardHeight; ++i)
                    for (var j = 0; j < settings.ChessboardWidth; ++j)
                        objects.Add(new MCvPoint3D32f(j*settings.CellWidth, i*settings.CellHeight, 0.0F));
                points[k] = objects.ToArray();
            }

            ExtrinsicCameraParameters extrinsicParameters;
            Matrix<double> fundamental;
            Matrix<double> essential;

            //If Emgu.CV.CvEnum.CALIB_TYPE == CV_CALIB_USE_INTRINSIC_GUESS and/or CV_CALIB_FIX_ASPECT_RATIO are specified, some or all of fx, fy, cx, cy must be initialized before calling the function
            //if you use FIX_ASPECT_RATIO and FIX_FOCAL_LEGNTH options, these values needs to be set in the intrinsic parameters before the CalibrateCamera function is called. Otherwise 0 values are used as default.
            CameraCalibration.StereoCalibrate(
                points,
                corners.Select(_ => _.LeftCorners).ToArray(),
                corners.Select(_ => _.RightCorners).ToArray(),
                calibrationResult.Camera1Result.IntrinsicParameters,
                calibrationResult.Camera2Result.IntrinsicParameters,
                calibrationResult.Settings.ImageSize,
                CALIB_TYPE.DEFAULT,
                new MCvTermCriteria(0.1e5),
                out extrinsicParameters,
                out fundamental,
                out essential);

            var rec1 = new Rectangle();
            var rec2 = new Rectangle();

            CvInvoke.cvStereoRectify(
                calibrationResult.Camera1Result.IntrinsicParameters.IntrinsicMatrix,
                calibrationResult.Camera2Result.IntrinsicParameters.IntrinsicMatrix,
                calibrationResult.Camera1Result.IntrinsicParameters.DistortionCoeffs,
                calibrationResult.Camera2Result.IntrinsicParameters.DistortionCoeffs,
                calibrationResult.Settings.ImageSize,
                extrinsicParameters.RotationVector.RotationMatrix,
                extrinsicParameters.TranslationVector,
                calibrationResult.Camera1Result.R,
                calibrationResult.Camera2Result.R,
                calibrationResult.Camera1Result.P,
                calibrationResult.Camera2Result.P,
                calibrationResult.Q,
                STEREO_RECTIFY_TYPE.DEFAULT,
                0,
                calibrationResult.Settings.ImageSize,
                ref rec1,
                ref rec2);

            calibrationResult.Camera1Result.Rectangle = rec1;
            calibrationResult.Camera2Result.Rectangle = rec2;

            return calibrationResult;
        }

        public static CalibrationCorners FindChessboardCorners(StereoImage<Bgr, byte> stereoImage, Size patternSize)
        {
            var grayStereoImage = stereoImage.Convert<Gray, byte>();
            var leftCorners = FindChessboardCorners(grayStereoImage.LeftImage, patternSize);
            var rightCorners = FindChessboardCorners(grayStereoImage.RightImage, patternSize);
            if (leftCorners != null && rightCorners != null)
                return new CalibrationCorners(leftCorners, rightCorners);
            return null;
        }

        private static PointF[] FindChessboardCorners(Image<Gray, byte> image, Size patternSize)
        {
            var corners = CameraCalibration.FindChessboardCorners(image, patternSize, CALIB_CB_TYPE.ADAPTIVE_THRESH);
            if (corners != null)
            {
                image.FindCornerSubPix(new[] {corners}, new Size(11, 11), new Size(-1, -1),
                    new MCvTermCriteria(30, 0.01));
            }
            return corners;
        }

        public static StereoImage<Bgr, byte> DrawChessboardCorners(StereoImage<Bgr, byte> stereoImage, CalibrationCorners corners)
        {
            var colors = new Bgr[corners.LeftCorners.Length];
            var random = new Random();
            for (var i = 0; i < colors.Length; ++i)
            {
                colors[i] = new Bgr(random.Next(0, 255), random.Next(0, 255), random.Next(0, 255));
            }

            DrawChessboardCorners(stereoImage.LeftImage, corners.LeftCorners, colors);
            DrawChessboardCorners(stereoImage.RightImage, corners.RightCorners, colors);

            return stereoImage;
        }

        private static void DrawChessboardCorners(Image<Bgr, byte> image, PointF[] corners, Bgr[] colors)
        {
            image.Draw(new CircleF(corners[0], 3), new Bgr(Color.Yellow), 1);

            for (var i = 1; i < corners.Length; ++i)
            {
                image.Draw(new LineSegment2DF(corners[i - 1], corners[i]), colors[i], 2);
                image.Draw(new CircleF(corners[i], 3), new Bgr(Color.Yellow), 1);
            }
        }

        private static Image<Bgr, byte> Remap(Image<Bgr, byte> image, Matrix<float> mapX, Matrix<float> mapY)
        {
            var resultImage = new Image<Bgr, byte>(image.Size);
            CvInvoke.cvRemap(image, resultImage, mapX, mapY,
                (int)INTER.CV_INTER_LINEAR | (int)WARP.CV_WARP_FILL_OUTLIERS, new MCvScalar(0));
            return resultImage;
        }
        
        private static void InitUndistortRectifyMap(CalibrationCameraResult calibrationResult, Size size,
            out Matrix<float> mapX, out Matrix<float> mapY)
        {
            mapX = new Matrix<float>(size);
            mapY = new Matrix<float>(size);
            CvInvoke.cvInitUndistortRectifyMap(
                calibrationResult.IntrinsicParameters.IntrinsicMatrix,
                calibrationResult.IntrinsicParameters.DistortionCoeffs,
                calibrationResult.R,
                calibrationResult.P,
                mapX, mapY);
        }
    }
}
