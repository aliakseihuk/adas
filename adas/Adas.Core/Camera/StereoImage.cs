using System.IO;
using System.Xml.Serialization;
using Emgu.CV;
using Emgu.CV.Structure;

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
            return new StereoImage<TColor, TDepth>
            {
                Name = fileInfo.Name,
                LeftImage = new Image<TColor, TDepth>(Path.Combine(fileInfo.BasePath, fileInfo.LeftImagePath)),
                RightImage = new Image<TColor, TDepth>(Path.Combine(fileInfo.BasePath, fileInfo.RightImagePath)),
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
                Name = Name,
                LeftImage = LeftImage.Copy(),
                RightImage = RightImage.Copy()
            };
        }

        public override string ToString()
        {
            return Name;
        }
    }

    public class StereoImageFileInfo
    {
        //constructor for serialization
        public StereoImageFileInfo()
        {
        }

        public StereoImageFileInfo(StereoImage<Bgr, byte> image)
        {
            Name = image.Name;
            LeftImagePath = string.Format("left_{0}.png", Name);
            RightImagePath = string.Format("right_{0}.png", Name);
        }

        public string Name { get; set; }
        public string LeftImagePath { get; set; }
        public string RightImagePath { get; set; }

        [XmlIgnore]
        public string BasePath { get; set; }
    }
}
