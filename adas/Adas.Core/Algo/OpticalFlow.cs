using System;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

namespace Adas.Core.Algo
{
    public class OpticalFlow
    {
        public static Image<Gray, float> Compute(OpticalFlowModel model)
        {
            var flowX = new Image<Gray, float>(model.Image1.Size);
            var flowY = new Image<Gray, float>(model.Image1.Size);

            Emgu.CV.OpticalFlow.Farneback(
                model.Image1,
                model.Image2,
                flowX,
                flowY,
                model.PyramidScale,
                model.Levels,
                model.WindowSize,
                model.Iterations,
                model.PolyN,
                model.PolySigma,
                model.Flag);

            flowY = flowY.Mul(flowY);
            flowX = flowX.Mul(flowX);
            Image<Gray, float> flowResult = flowX.Mul(flowX) + flowY.Mul(flowY);
            CvInvoke.cvSqrt(flowResult, flowResult);

            return flowResult;
        }
    }

    public class OpticalFlowModel : ICloneable
    {
        public OpticalFlowModel()
        {
            PyramidScale = 0.5;
            Levels = 1;
            WindowSize = 10;
            Iterations = 2;
            Flag = OPTICALFLOW_FARNEBACK_FLAG.DEFAULT;
        }

        public Image<Gray, byte> Image1 { get; set; }
        public Image<Gray, byte> Image2 { get; set; }

        /// <summary>
        ///     Specifying the image scale to build pyramids for each image
        ///     and smaller than 1.
        ///     PyramidScale=0.5 means a classical pyramid,
        ///     where each next layer is twice smaller than the previous one.
        /// </summary>
        public double PyramidScale { get; set; }

        /// <summary>
        ///     Number of pyramid layers including the initial image.
        ///     Levels=1 means that no extra layers are created
        ///     and only the original images are used.
        /// </summary>
        public int Levels { get; set; }

        /// <summary>
        ///     Averaging window size.
        ///     Larger values increase the algorithm robustness to image noise
        ///     and give more chances for fast motion detection,
        ///     but yield more blurred motion field.
        /// </summary>
        public int WindowSize { get; set; }

        /// <summary>
        ///     Number of iterations the algorithm does at each pyramid level.
        /// </summary>
        public int Iterations { get; set; }

        /// <summary>
        ///     Size of the pixel neighborhood used to find polynomial expansion in each pixel.
        ///     Larger values mean that the image will be approximated with smoother surfaces,
        ///     yielding more robust algorithm and more blurred motion field,
        ///     typically poly_n =5 or 7.
        /// </summary>
        public int PolyN
        {
            get { return 7; }
        }

        /// <summary>
        ///     Standard deviation of the Gaussian that is used to
        ///     smooth derivatives used as a basis for the polynomial expansion.
        ///     For poly_n=5, you can set poly_sigma=1.1,
        ///     for poly_n=7, a good value would be poly_sigma=1.5
        /// </summary>
        public double PolySigma
        {
            get { return 1.5; }
        }

        public OPTICALFLOW_FARNEBACK_FLAG Flag { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}