using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace HaarPrecisionChecker
{
    static class GtHelper
    {
        public const string GtFilename = "gt.txt";

        public static TestImage[] ParseGt(string testPath)
        {
            var dict = new Dictionary<string, TestImage>();
            string gtfullpath = Path.Combine(testPath, GtFilename);
            if (!File.Exists(gtfullpath))
            {
                return null;
            }
            using (var reader = new StreamReader(new FileStream(gtfullpath, FileMode.Open, FileAccess.Read)))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    if (line == null)
                        continue;
                    string[] info = line.Split(new[] { ';' });
                    if (info.Length < 5)
                        continue;
                    string name = info[0];
                    int x1 = int.Parse(info[1]);
                    int y1 = int.Parse(info[2]);
                    int x2 = int.Parse(info[3]);
                    int y2 = int.Parse(info[4]);
                    if (!dict.ContainsKey(name))
                        dict[name] = new TestImage(name);
                    var area = new Rectangle(x1, y1, x2 - x1, y2 - y1);
                    dict[name].SignsAreas.Add(area);
                }
            }
            return dict.Values.ToArray();
        }
    }
}
