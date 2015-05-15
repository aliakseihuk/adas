using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using DirectShowLib;
using Emgu.CV;
using Emgu.CV.Structure;

namespace Adas.Core.Algo.Hough
{
    public static class HoughLines
    {
        public const double ThetaResolution = 1.0;
        public const double RhoResolution = 1.0;
        public const int HoughThreshold = 40;
        public const double MinLineLenght = 5.0;
        public const double LinesGap = 1.0;
        public const double MinLineFullLenght = 70.0;

        public const double AngleThreshold = 0.30;

        public static HoughResult Compute(Image<Bgr, byte> image)
        {
            var grayImage = PreprocessImage(image);
            
            var lines =
                grayImage.HoughLinesBinary(RhoResolution, (ThetaResolution*Math.PI)/180, HoughThreshold, MinLineLenght,
                    LinesGap)[0];
            lines = lines.Where(l => Math.Abs(l.Direction.Y) > AngleThreshold).ToArray();
            return GroupSegments(lines);
        }

        public static Image<Gray, byte> PreprocessImage(Image<Bgr, byte> image)
        {
            var grayImage = image.Convert<Gray, byte>();
            var kernel = new Matrix<float>(new float[,] {{-1, 0, 1}, {-1, 0, 1}, {-1, 0, 1}});
            CvInvoke.cvFilter2D(grayImage, grayImage, kernel, new Point(-1, -1));

            grayImage = grayImage.ThresholdBinary(new Gray(30), new Gray(255));
            grayImage = grayImage.SmoothGaussian(3);
            grayImage = grayImage.Erode(1);
            return grayImage;
        }

        private static HoughResult GroupSegments(LineSegment2D[] segments)
        {
            var solidSegments = new List<LineSegment2D>();
            var dashSegments = new List<DashLineSegment2D>();
            var grouped = GroupDirectionSerment(segments);
            foreach (var group in grouped)
            {
                var fullSegments = GroupCloseSegment(group.Item2);
                var allSegments = GroupDashSegment(fullSegments);
                var dashSegment = GroupCloseDashSegment(allSegments.Item2);
                solidSegments.AddRange(allSegments.Item1.Where(l => DistanceHelper.Distance(l.P1, l.P2) > MinLineFullLenght));
                dashSegments.AddRange(dashSegment.Where(l => DistanceHelper.Distance(l.AsSolid.P1, l.AsSolid.P2) > MinLineFullLenght));
            }
            return new HoughResult {SolidLines = solidSegments.ToArray(), DashLines = dashSegments.ToArray()};
        }

        private static List<Tuple<PointF, List<LineSegment2D>>> GroupDirectionSerment(LineSegment2D[] segments)
        {
            var chunks = new List<Tuple<PointF, List<LineSegment2D>>>();
            foreach (var segment in segments)
            {
                var direction = segment.Direction.X >= 0
                            ? new PointF(segment.Direction.X, segment.Direction.Y)
                            : new PointF(-segment.Direction.X, -segment.Direction.Y);
                var isMerged = false;
                foreach (var chunk in chunks)
                {
                    var chunkDirection = chunk.Item1;
                    if (SegmentHelper.DirectionClose(chunkDirection, segment.Direction))
                    {
                        isMerged = true;
                        chunks.Remove(chunk);
                        var chunkItems = chunk.Item2;
                        chunkItems.Add(segment);

                        var weight = 1.0f/chunkItems.Count;
                        //all 
                        
                        direction = new PointF(chunkDirection.X*(1 - weight) + direction.X*weight,
                            chunkDirection.Y*(1 - weight) + direction.Y*weight);
                        var norm = (float) Math.Sqrt(direction.X*direction.X + direction.Y*direction.Y);
                        direction = new PointF(direction.X/norm, direction.Y/norm);
                        chunks.Add(new Tuple<PointF, List<LineSegment2D>>(direction, chunkItems));
                        break;
                    }
                }
                if (!isMerged)
                    chunks.Add(new Tuple<PointF, List<LineSegment2D>>(direction, new List<LineSegment2D> {segment}));
            }
            return chunks;
        }

        private static List<LineSegment2D> GroupCloseSegment(List<LineSegment2D> segments)
        {
            var chunks = new List<LineSegment2D>();
            foreach (var segment in segments)
            {
                var isMerged = false;
                foreach (var chunk in chunks)
                {
                    if (SegmentHelper.DistanceClose(chunk, segment))
                    {
                        var merged = SegmentHelper.MergeSegments(new[] {chunk, segment});
                        chunks.Remove(chunk);
                        chunks.Add(merged);
                        isMerged = true;
                        break;
                    }
                }
                if (!isMerged)
                    chunks.Add(segment);
            }
            return segments.Count == chunks.Count ? chunks : GroupCloseSegment(chunks);
        }

        private static List<DashLineSegment2D> GroupCloseDashSegment(DashLineSegment2D[] segments)
        {
            var chunks = new List<DashLineSegment2D>();
            foreach (var segment in segments)
            {
                var isMerged = false;
                foreach (var chunk in chunks)
                {
                    if (SegmentHelper.DistanceClose(chunk.AsSolid, segment.AsSolid))
                    {
                        var merged = new DashLineSegment2D {Elements = chunk.Elements.Union(segment.Elements).ToArray()};
                        chunks.Remove(chunk);
                        chunks.Add(merged);
                        isMerged = true;
                        break;
                    }
                }
                if (!isMerged)
                    chunks.Add(segment);
            }
            return segments.Length == chunks.Count ? chunks : GroupCloseDashSegment(chunks.ToArray());
        }

        private static Tuple<LineSegment2D[], DashLineSegment2D[]> GroupDashSegment(List<LineSegment2D> segments)
        {
            var chunks = new List<List<LineSegment2D>>();
            foreach (var segment in segments)
            {
                var isAdded = false;
                foreach (var chunk in chunks)
                {
                    var element = chunk.First();
                    if (SegmentHelper.OneLineClose(element, segment))
                    {
                        chunk.Add(segment);
                        isAdded = true;
                        break;
                    }
                }
                if (!isAdded)
                {
                    var dash = new List<LineSegment2D> {segment};
                    chunks.Add(dash);
                }
            }
            var dashlines =
                chunks.Where(c => c.Count > 1).Select(c => new DashLineSegment2D {Elements = c.ToArray()}).ToArray();
            var dashElements = new HashSet<LineSegment2D>();
            foreach (var chunk in dashlines)
            {
                dashElements.UnionWith(chunk.Elements);
            }
            var oldsegments = segments.Except(dashElements);
            return new Tuple<LineSegment2D[], DashLineSegment2D[]>(oldsegments.ToArray(), dashlines);
        }
    }

    public class CacheLine
    {
        public const int RealLineHistoryCount = 6;
        
        public CacheLine(CacheLine parent)
        {
            Parent = parent;
            if(parent != null)
                Parent.Child = this;
        }

        public CacheLine(LineSegment2D segment, CacheLine parent)
            : this(parent)
        {
            Segment = segment;
        }

        public LineSegment2D Segment { get; private set; }
        public CacheLine Child { get; private set; }
        public CacheLine Parent { get; private set; }

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

        public List<CacheLine> GetAllLineHistory()
        {
            var list = new List<CacheLine>();
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
            var norm = (float)Math.Sqrt(x * x + y * y);
            return new PointF(x / norm, y / norm);
        }

        private void GetAllLineHistory(List<CacheLine> list)
        {
            list.Add(this);
            if(Parent != null)
                Parent.GetAllLineHistory(list);
        }
    }

    public class CacheLineContainer
    {
        public const int ClearLevel = 8;

        private readonly Dictionary<int, List<CacheLine>> levels_ = new Dictionary<int, List<CacheLine>>();

        public CacheLineContainer()
        {
            for (var i = 0; i < ClearLevel; ++i)
            {
                levels_[i] = new List<CacheLine>();
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
            var cacheLine = new CacheLine(segment, parent);
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
            foreach(var level in levels_)
            {
                var last = level.Value.Where(_ => !_.HasChild);
                foreach (var line in last)
                {
                    var history = line.GetAllLineHistory();
                    if (CacheLine.IsReal(history.Count))
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

        public CacheLine FindParent(LineSegment2D segment)
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
            foreach (var element in levels_[ClearLevel-1])
            {
                element.ClearChildHistory();
            }
            for (var i = ClearLevel - 1; i > 0; --i)
            {
                levels_[i] = levels_[i - 1];
            }
            levels_[0] = new List<CacheLine>();
        }
    }
}