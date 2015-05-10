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
        public const int HoughThreshold = 50;
        public const double MinLineLenght = 5.0;
        public const double LinesGap = 1.0;

        public const double AngleThreshold = 0.50;

        public static HoughResult Compute(Image<Bgr, byte> image)
        {
            var grayImage = PreprocessImage(image);
            var lines =
                grayImage.HoughLinesBinary(RhoResolution, (ThetaResolution*Math.PI)/180, HoughThreshold, MinLineLenght,
                    LinesGap)[0];
            lines = lines.Where(l => Math.Abs(l.Direction.Y) > AngleThreshold).ToArray();
            return GroupSegments(lines);
        }

        private static Image<Gray, byte> PreprocessImage(Image<Bgr, byte> image)
        {
            var grayImage = image.Convert<Gray, byte>();
            var kernel = new Matrix<float>(new float[,] {{-1, 0, 1}, {-1, 0, 1}, {-1, 0, 1}});
            CvInvoke.cvFilter2D(grayImage, grayImage, kernel, new Point(-1, -1));

            grayImage = grayImage.ThresholdBinary(new Gray(100), new Gray(255));
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
                solidSegments.AddRange(allSegments.Item1);
                dashSegments.AddRange(allSegments.Item2);
            }
            return new HoughResult {SolidLines = solidSegments.ToArray(), DashLines = dashSegments.ToArray()};
        }

        private static List<Tuple<PointF, List<LineSegment2D>>> GroupDirectionSerment(LineSegment2D[] segments)
        {
            var chunks = new List<Tuple<PointF, List<LineSegment2D>>>();
            foreach (var segment in segments)
            {
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
                        var direction = new PointF(chunkDirection.X*(1 - weight) + segment.Direction.X*weight,
                            chunkDirection.Y*(1 - weight) + segment.Direction.Y*weight);
                        chunks.Add(new Tuple<PointF, List<LineSegment2D>>(direction, chunkItems));
                        break;
                    }
                }
                if (!isMerged)
                    chunks.Add(new Tuple<PointF, List<LineSegment2D>>(segment.Direction,
                        new List<LineSegment2D> {segment}));
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

        private static Tuple<LineSegment2D[], DashLineSegment2D[]> GroupDashSegment(List<LineSegment2D> segments)
        {
            var chunks = new List<DashLineSegment2D>();
            foreach (var segment in segments)
            {
                var isAdded = false;
                foreach (var chunk in chunks)
                {
                    var element = chunk.Elements.First();
                    if (SegmentHelper.DirectionClose(element.Direction, segment.Direction) &&
                        SegmentHelper.OneLineClose(element, segment))
                    {
                        chunk.Elements.Add(segment);
                        isAdded = true;
                        break;
                    }
                }
                if (!isAdded)
                {
                    var dash = new DashLineSegment2D();
                    dash.Elements.Add(segment);
                    chunks.Add(dash);
                }
            }
            chunks = chunks.Where(c => c.Elements.Count > 1).ToList();
            var dashElements = new HashSet<LineSegment2D>();
            foreach (var chunk in chunks)
            {
                dashElements.UnionWith(chunk.Elements);
            }
            var oldsegments = segments.Except(dashElements);
            return new Tuple<LineSegment2D[], DashLineSegment2D[]>(oldsegments.ToArray(), chunks.ToArray());
        }
    }
}