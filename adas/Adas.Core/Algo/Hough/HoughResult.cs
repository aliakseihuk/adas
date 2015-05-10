using System.Drawing;
using System.Linq;
using Emgu.CV.Structure;

namespace Adas.Core.Algo.Hough
{
    public class HoughResult
    {
        public LineSegment2D[] SolidLines { get; set; }
        public DashLineSegment2D[] DashLines { get; set; }

        public void MoveRoiResult(Size moveSize)
        {
            SolidLines =
                SolidLines.Select(solid => new LineSegment2D(solid.P1 + moveSize, solid.P2 + moveSize)).ToArray();
            foreach (var dash in DashLines)
            {
                dash.Elements =
                    dash.Elements.Select(element => new LineSegment2D(element.P1 + moveSize, element.P2 + moveSize))
                        .ToArray();
            }
        }
    }
}