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

        public void MoveRoiResult(int x, int y)
        {
            SolidLines =
                SolidLines.Select(
                    solid =>
                        new LineSegment2D(new Point(solid.P1.X + x, solid.P1.Y + y),
                            new Point(solid.P2.X + x, solid.P2.Y + y))).ToArray();
            var dashLines = new List<DashLineSegment2D>();
            foreach (var dash in DashLines)
            {
                var array =
                    dash.Elements.Select(
                        element =>
                            new LineSegment2D(new Point(element.P1.X + x, element.P1.Y + y),
                                new Point(element.P2.X + x, element.P2.Y + y))).ToArray();
                dashLines.Add(new DashLineSegment2D {Elements = array});
            }
            DashLines = dashLines.ToArray();
        }
    }
}