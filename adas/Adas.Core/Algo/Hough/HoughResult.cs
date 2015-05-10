using System.Collections.Generic;
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
            var temp = new List<LineSegment2D>();
            foreach (var dash in DashLines)
            {
                temp.AddRange(
                    dash.Elements.Select(element => new LineSegment2D(element.P1 + moveSize, element.P2 + moveSize)));
                dash.Elements.Clear();
                dash.Elements.AddRange(temp);
                temp.Clear();
            }
        }
    }
}