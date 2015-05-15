using System.Collections.Generic;
using System.Linq;
using Emgu.CV.Structure;

namespace Adas.Core.Algo.Hough
{
    public class LineCache
    {
        public const int ClearLevel = 8;
        private readonly Dictionary<int, List<LineHistory>> levels_ = new Dictionary<int, List<LineHistory>>();

        public LineCache()
        {
            for (var i = 0; i < ClearLevel; ++i)
            {
                levels_[i] = new List<LineHistory>();
            }
        }

        public void AddResult(HoughResult result)
        {
            ShiftLevels();
            foreach (var solid in result.SolidLines)
            {
                AddLine(solid);
            }
            foreach (var dash in result.DashLines)
            {
                AddDashLine(dash);
            }
        }

        public void AddLine(LineSegment2D segment)
        {
            var parent = FindParent(segment);
            var cacheLine = new LineHistory(segment, parent);
            levels_[0].Add(cacheLine);
        }

        public void AddDashLine(DashLineSegment2D dashSegment)
        {
            AddLine(dashSegment.AsSolid);
        }

        public HoughResult GetCachedResult()
        {
            var result = new HoughResult();
            var realLines = new List<LineSegment2D>();
            foreach (var level in levels_)
            {
                var last = level.Value.Where(_ => !_.HasChild);
                foreach (var line in last)
                {
                    var history = line.GetAllLineHistory();
                    if (LineHistory.IsReal(history.Count))
                    {
                        //realLines.Add(history.First().Segment);
                        realLines.Add(SegmentHelper.MergeSegments(history.Select(c => c.Segment).ToArray()));
                    }
                }
            }
            result.SolidLines = realLines.ToArray();
            result.DashLines = new DashLineSegment2D[0];
            return result;
        }

        public LineHistory FindParent(LineSegment2D segment)
        {
            for (var i = 1; i < ClearLevel; ++i)
            {
                var levelLines = levels_[i];
                foreach (var element in levelLines.Where(l => !l.HasChild))
                {
                    var avgDirection = element.GetAverageDirection();
                    if (SegmentHelper.DirectionClose(avgDirection, segment.Direction))
                    {
                        if (SegmentHelper.DistanceClose(element.Segment, segment, true))
                        {
                            return element;
                        }
                    }
                }
            }
            return null;
        }

        public void ShiftLevels()
        {
            foreach (var element in levels_[ClearLevel - 1])
            {
                element.ClearChildHistory();
            }
            for (var i = ClearLevel - 1; i > 0; --i)
            {
                levels_[i] = levels_[i - 1];
            }
            levels_[0] = new List<LineHistory>();
        }
    }
}