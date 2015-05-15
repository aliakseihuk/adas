using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
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
        public const int RealLineParentCount = 5;

        private readonly int parentCount_;

        public CacheLine(int parentCount)
        {
            parentCount_ = parentCount;
        }

        public CacheLine(LineSegment2D segment, int parentCount)
            : this(parentCount)
        {
            Segment = segment;
        }

        public CacheLine(DashLineSegment2D dashSegment, int parentCount)
            : this(parentCount)
        {
            DashSegment = dashSegment;
        }

        public LineSegment2D Segment { get; private set; }
        public DashLineSegment2D DashSegment { get; private set; }
        public bool HasChild { get; set; }

        public bool IsDash
        {
            get { return DashSegment != null; }
        }

        public int ParentsCount
        {
            get { return parentCount_; }
        }

        public bool IsReal
        {
            get { return parentCount_ >= RealLineParentCount; }
        }
    }

    public class CacheLineContainer
    {
        public const int ClearLevel = 10;

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
            var parent = AnalyzeParent(segment);
            var cacheLine = new CacheLine(segment, parent);
            levels_[0].Add(cacheLine);
        }

        public void AddDashLine(DashLineSegment2D dashSegment)
        {
            var parent = AnalyzeParent(dashSegment.AsSolid);
            var cacheLine = new CacheLine(dashSegment, parent);
            levels_[0].Add(cacheLine);
        }

        public HoughResult GetCachedResult()
        {
            var result = new HoughResult();
            //var realLines =
            //    levels_.Aggregate(
            //        (p1, p2) => new KeyValuePair<int, List<CacheLine>>(0, p1.Value.Union(p2.Value).ToList()))
            //        .Value.Where(_ => !_.HasChild && _.IsReal)
            //        .ToArray();
            var realLines = new List<CacheLine>();
            foreach(var level in levels_)
            {
                var valid = level.Value.Where(_ => !_.HasChild && _.IsReal);
                realLines.AddRange(valid);
            }
            result.SolidLines = realLines.Where(l => !l.IsDash).Select(l => l.Segment).ToArray();
            result.DashLines = realLines.Where(l => l.IsDash).Select(l => l.DashSegment).ToArray();
            return result;
        }

        public int AnalyzeParent(LineSegment2D segment)
        {
            for (var i = 1; i < ClearLevel; ++i)
            {
                var levelLines = levels_[i];
                var parents = 0;
                foreach (var element in levelLines.Where(l => !l.HasChild))
                {
                    var elementLine = element.IsDash ? element.DashSegment.AsSolid : element.Segment;
                    if (SegmentHelper.DirectionClose(elementLine.Direction, segment.Direction))
                    {
                        if (SegmentHelper.DistanceClose(elementLine, segment, true))
                        {
                            parents = element.ParentsCount + 1;
                            element.HasChild = true;
                            break;
                        }
                    }
                }
                if (parents > 0)
                    return parents;
            }
            return 0;
        }

        public void ShiftLevels()
        {
            for (var i = 1; i < ClearLevel; ++i)
            {
                levels_[i] = levels_[i - 1];
            }
            levels_[0] = new List<CacheLine>();
        }
    }
}