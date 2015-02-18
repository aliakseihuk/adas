using System;
using Emgu.CV;
using Emgu.CV.Structure;

namespace Adas.Core.Algo
{
    public class Stereo
    {
        public static Image<Gray, byte> Compute(StereoSgbmModel model)
        {
            var disparity = new Image<Gray, short>(model.Image1.Size);
            using (var stereoSolver = new StereoSGBM(
                model.MinDisparity,
                model.NumDisparity,
                model.SadWindowSize,
                model.P1,
                model.P2,
                model.Disparity12MaxDiff,
                model.PreFilterCap,
                model.UniquenessRatio,
                model.SpeckleWindowSize,
                model.SpeckleRange,
                model.Mode))
            {
                stereoSolver.FindStereoCorrespondence(model.Image1, model.Image2, disparity);
            }
            return disparity.Convert<Gray, byte>();
        }
    }

    public class StereoSgbmModel : ICloneable
    {
        private int _numDisparity;
        private int _sadWindowSize;

        public StereoSgbmModel()
        {
            MinDisparity = 0;
            NumDisparity = 16;
            SadWindowSize = 1;
            Disparity12MaxDiff = -1;
            PreFilterCap = 0;
            UniquenessRatio = 5;
            SpeckleWindowSize = 0;
            SpeckleRange = 0;
            Mode = StereoSGBM.Mode.SGBM;
        }

        public Image<Gray, Byte> Image1 { get; set; }
        public Image<Gray, Byte> Image2 { get; set; }

        /// <summary>
        ///     Minimum possible disparity value.
        ///     Normally, it is zero but sometimes rectification algorithms can shift images,
        ///     so this parameter needs to be adjusted accordingly
        /// </summary>
        public int MinDisparity { get; set; }


        /// <summary>
        ///     Maximum disparity minus minimum disparity.
        ///     The value is always greater than zero.
        ///     In the current implementation, this parameter must be divisible by 16.
        /// </summary>
        public int NumDisparity
        {
            get { return _numDisparity; }
            set { _numDisparity = value > 16 ? value - (value%16) : 16; }
        }

        /// <summary>
        ///     Matched block size.
        ///     It must be an odd number >=1.
        ///     Normally, it should be somewhere in the 3..11 range
        /// </summary>
        public int SadWindowSize
        {
            get { return _sadWindowSize; }
            set { _sadWindowSize = value > 0 ? value - (value%2) : 1; }
        }

        /// <summary>
        ///     The first parameter controlling the disparity smoothness. See P2.
        /// </summary>
        public int P1
        {
            get
            {
                if (Image1 != null)
                    return 8*Image1.NumberOfChannels*SadWindowSize*SadWindowSize;
                return 0;
            }
        }

        /// <summary>
        ///     The second parameter controlling the disparity smoothness.
        ///     The larger the values are, the smoother the disparity is.
        ///     P1 is the penalty on the disparity change by plus or minus 1 between neighbor pixels.
        ///     P2 is the penalty on the disparity change by more than 1 between neighbor pixels.
        ///     The algorithm requires P2 > P1.
        ///     See stereo_match.cpp sample where some reasonably good P1 and P2 values are shown
        ///     (like 8*number_of_image_channels*SADWindowSize*SADWindowSize and
        ///     32*number_of_image_channels*SADWindowSize*SADWindowSize, respectively).
        /// </summary>
        public int P2
        {
            get
            {
                if (Image1 != null)
                    return 32*Image1.NumberOfChannels*SadWindowSize*SadWindowSize;
                return 1;
            }
        }

        /// <summary>
        ///     Maximum allowed difference (in integer pixel units) in the left-right disparity check.
        ///     Set it to a non-positive value to disable the check.
        /// </summary>
        public int Disparity12MaxDiff { get; set; }

        /// <summary>
        ///     Truncation value for the prefiltered image pixels.
        ///     The algorithm first computes x-derivative at each pixel and clips its value by [-preFilterCap, preFilterCap]
        ///     interval.
        ///     The result values are passed to the Birchfield-Tomasi pixel cost function.
        /// </summary>
        public int PreFilterCap { get; set; }

        /// <summary>
        ///     Margin in percentage by which the best (minimum)
        ///     computed cost function value should "win" the second best value
        ///     to consider the found match correct.
        ///     Normally, a value within the 5-15 range is good enough.
        /// </summary>
        public int UniquenessRatio { get; set; }

        /// <summary>
        ///     Maximum size of smooth disparity regions to consider their noise speckles and invalidate.
        ///     Set it to 0 to disable speckle filtering.
        ///     Otherwise, set it somewhere in the 50-200 range.
        /// </summary>
        public int SpeckleWindowSize { get; set; }

        /// <summary>
        ///     Maximum disparity variation within each connected component.
        ///     If you do speckle filtering, set the parameter to a positive value,
        ///     it will be implicitly multiplied by 16.
        ///     Normally, 1 or 2 is good enough.
        /// </summary>
        public int SpeckleRange { get; set; }

        /// <summary>
        ///     Set it to HH to run the full-scale two-pass dynamic programming algorithm.
        ///     It will consume O(W*H*numDisparities) bytes,
        ///     which is large for 640x480 stereo and huge for HD-size pictures.
        ///     By default, it is set to SGBM.
        /// </summary>
        public StereoSGBM.Mode Mode { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}