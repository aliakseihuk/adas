using Emgu.CV;

namespace Adas.Core.Camera
{
    public class StereoImage<TColor, TDepth>
        where TColor : struct, IColor
        where TDepth : new()
    {
        public string Name { get; set; }
        public Image<TColor, TDepth> LeftImage { get; set; }
        public Image<TColor, TDepth> RightImage { get; set; }

        public static StereoImage<TColor, TDepth> Load(StereoImageFileInfo fileInfo)
        {
            return new StereoImage<TColor, TDepth>()
            {
                Name = fileInfo.Name,
                LeftImage = new Image<TColor, TDepth>(fileInfo.LeftImagePath),
                RightImage = new Image<TColor, TDepth>(fileInfo.LeftImagePath),
            };
        }

        public StereoImage<TOtherColor, TOtherDepth> Convert<TOtherColor, TOtherDepth>()
            where TOtherColor : struct, IColor
            where TOtherDepth : new()
        {
            return new StereoImage<TOtherColor, TOtherDepth>
            {
                LeftImage = LeftImage.Convert<TOtherColor, TOtherDepth>(),
                RightImage = RightImage.Convert<TOtherColor, TOtherDepth>()
            };
        }

        public StereoImage<TColor, TDepth> Copy()
        {
            return new StereoImage<TColor, TDepth>
            {
                LeftImage = LeftImage.Copy(),
                RightImage = RightImage.Copy()
            };
        }
    }

    public class StereoImageFileInfo
    {
        public string Name { get; set; }
        public string LeftImagePath { get; set; }
        public string RightImagePath { get; set; }
    }
}
