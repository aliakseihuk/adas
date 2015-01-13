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

        private static readonly HashSet<int> AllowedSignType = new HashSet<int>
        {
            0,  //speed limit 20 (prohibitory)
            1,  //speed limit 30 (prohibitory)
            2,  //speed limit 50 (prohibitory)
            3,  //speed limit 60 (prohibitory)
            4,  //speed limit 70 (prohibitory)
            5,  //speed limit 80 (prohibitory)
            7,  //speed limit 100 (prohibitory)
            8,  //speed limit 120 (prohibitory)
            9,  //no overtaking (prohibitory)
            10, //no overtaking (trucks) (prohibitory)
            15, //no traffic both ways (prohibitory)
            16  //no trucks (prohibitory)
        };

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
                    var signClass = int.Parse(info[5]);
                    if (AllowedSignType.Contains(signClass))
                    {
                        if (!dict.ContainsKey(name))
                            dict[name] = new TestImage(name);
                        var area = new Rectangle(x1, y1, x2 - x1, y2 - y1);
                        dict[name].SignsAreas.Add(area);
                    }
                }
            }
            return dict.Values.ToArray();
        }
    }
}
