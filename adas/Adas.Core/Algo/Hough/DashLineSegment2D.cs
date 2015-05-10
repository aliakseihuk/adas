using System.Collections.Generic;
using Emgu.CV.Structure;

namespace Adas.Core.Algo.Hough
{
    public class DashLineSegment2D
    {
        public DashLineSegment2D()
        {
            Elements = new List<LineSegment2D>();
        }

        public List<LineSegment2D> Elements { get; private set; }
    }
}