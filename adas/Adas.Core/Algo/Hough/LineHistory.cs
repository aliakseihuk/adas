using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Emgu.CV.Structure;

namespace Adas.Core.Algo.Hough
{
    public class LineHistory
    {
        public const int RealLineHistoryCount = 6;

        public LineHistory(LineHistory parent)
        {
            Parent = parent;
            if (parent != null)
                Parent.Child = this;
        }

        public LineHistory(LineSegment2D segment, LineHistory parent)
            : this(parent)
        {
            Segment = segment;
        }

        public LineSegment2D Segment { get; private set; }
        public LineHistory Child { get; private set; }
        public LineHistory Parent { get; private set; }

        public bool HasChild
        {
            get { return Child != null; }
        }

        public static bool IsReal(int lineHistoryCount)
        {
            return lineHistoryCount >= RealLineHistoryCount;
        }

        public void ClearChildHistory()
        {
            if (HasChild)
            {
                Child.Parent = null;
                Child = null;
            }
        }

        public List<LineHistory> GetAllLineHistory()
        {
            var list = new List<LineHistory>();
            GetAllLineHistory(list);
            return list.Take(RealLineHistoryCount).ToList();
        }

        public PointF GetAverageDirection()
        {
            var history = GetAllLineHistory();
            var x = 0.0f;
            var y = 0.0f;
            foreach (var c in history)
            {
                if (c.Segment.Direction.X >= 0)
                {
                    x += c.Segment.Direction.X;
                    y += c.Segment.Direction.Y;
                }
                else
                {
                    x -= c.Segment.Direction.X;
                    y -= c.Segment.Direction.Y;
                }
            }
            var norm = (float) Math.Sqrt(x*x + y*y);
            return new PointF(x/norm, y/norm);
        }

        private void GetAllLineHistory(List<LineHistory> list)
        {
            list.Add(this);
            if (Parent != null)
                Parent.GetAllLineHistory(list);
        }
    }
}