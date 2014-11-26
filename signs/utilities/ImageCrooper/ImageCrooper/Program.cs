using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;

namespace ImageCrooper
{
    /// <summary>
    /// Program prepare bmp files(converted from *.ppm) for haar analysing
    /// 1.Reading images
    /// 2.Extract info from gt.txt
    /// 3.Crop signs
    /// 4.Generate areas without signs
    /// 5.Format Bad.dat and Good.dat
    /// gt.txt and image get from http://benchmark.ini.rub.de/?section=gtsdb&subsection=dataset
    /// </summary>
    class Program
    {
        public const string GtFilename = "gt.txt";
        public const string GoodDir = "Good\\";
        public const string GoodFilename = "Good.dat";
        public const string BadDir = "Bad\\";
        public const string BadFilename = "Bad.dat";

        public const int BadCountPerImage = 3;

        private static string SourceDir;
        private static string DestDir;

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Source directory is not specified");
                return;
            }
            if (args.Length == 1)
            {
                Console.WriteLine("Destination directory is not specified");
                return;
            }
            SourceDir = args[0];
            DestDir = args[1];
            Console.WriteLine("Source directory: {0}", SourceDir);
            Console.WriteLine("Destination directory: {0}", DestDir);
            var images = ParseGt();
            if (images == null)
                return;
            ProcessImages(images);
        }

        private static Image[] ParseGt()
        {
            Console.WriteLine("Begin export");

            var dict = new Dictionary<string, Image>();
            var gtfullpath = Path.Combine(SourceDir, GtFilename);
            Console.WriteLine("Gt full path: {0}", gtfullpath);

            if(!File.Exists(gtfullpath))
            {
                Console.WriteLine("Gt file doesn't exist");
                return null;
            }
            using (var reader = new StreamReader(new FileStream(gtfullpath, FileMode.Open, FileAccess.Read)))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (line == null)
                        continue;
                    var info = line.Split(new[] { ';' });
                    if (info.Length < 5)
                        continue;
                    var name = Path.GetFileNameWithoutExtension(info[0]);
                    var x1 = int.Parse(info[1]);
                    var y1 = int.Parse(info[2]);
                    var x2 = int.Parse(info[3]);
                    var y2 = int.Parse(info[4]);
                    if (!dict.ContainsKey(name))
                        dict[name] = new Image(name);
                    var area = new Rectangle(x1, y1, x2, y2);
                    dict[name].SignsAreas.Add(area);
                }
            }

            var counter = dict.Values.Sum(_ => _.SignsAreas.Count);
            Console.WriteLine("{0} images parsed", counter);
            return dict.Values.ToArray();
        }

        private static void ProcessImages(Image[] images)
        {
            var goodDirFullPath = Path.Combine(DestDir,GoodDir);
            var badDirFullPath = Path.Combine(DestDir,BadDir);
            CreateDirectory(goodDirFullPath);
            CreateDirectory(badDirFullPath);
            
            var random = new Random(0);
            var gooddatBuilder = new StringBuilder();
            var baddatBuilder = new StringBuilder();

            var goodcounter = 0;
            var badcounter = 0;
            var allcount = images.Count();
            foreach (var image in images)
            {
                Console.WriteLine("Processed: {0}", goodcounter);
                var i = 0;
                var imagesrcpath = string.Format("{0}.bmp", image.Name);
                imagesrcpath = Path.Combine(SourceDir, imagesrcpath);
                Bitmap srcimage = new Bitmap(imagesrcpath);
                

                foreach (var area in image.SignsAreas)
                {
                    
                    var filename = string.Format("{0}_{1}.bmp", image.Name, i);
                    var relativepath = Path.Combine(GoodDir, filename);
                    var destpath = Path.Combine(goodDirFullPath, filename);
                                    
                    var cropimage = CropImage(srcimage, destpath, area);
                    var srcwidth = srcimage.Width;
                    var width = cropimage.Width;
                    var srcheight = srcimage.Height;
                    var height = cropimage.Height;
                    cropimage.Dispose();

                    gooddatBuilder.AppendFormat("{0}  1  0 0 {1} {2}\n", relativepath, width, height);
                    
                    //genarate bad set of images with the same width and height
                    for(var j = 0; j < BadCountPerImage; ++j)
                    {
                        var badfilename = string.Format("{0}_{1}_{2}.bmp", image.Name, i, j);
                        relativepath = Path.Combine(BadDir, badfilename);
                        destpath = Path.Combine(badDirFullPath, badfilename);

                        //random coordinates of bad image
                        var x = random.Next(width, srcwidth - width);
                        var y = random.Next(height, srcheight - height);
                        var badarea = new Rectangle(x, y, x + width, y + height);
                        if (badarea.Intersect(area))
                            continue; //avoid intersetions of bad images with sign part
                        
                        cropimage = CropImage(srcimage, destpath, badarea);
                        cropimage.Dispose();

                        baddatBuilder.AppendLine(relativepath);
                        ++badcounter;
                    }
                    ++i;
                }
                srcimage.Dispose();
                goodcounter += i;
                ClearCurrentConsoleLine();
            }
            Console.WriteLine("{0} good images were processed", goodcounter);
            Console.WriteLine("{0} good images were processed", badcounter);

            var goodfilepath = Path.Combine(DestDir, GoodFilename);
            File.WriteAllText(goodfilepath, gooddatBuilder.ToString());
            Console.WriteLine("{0} file was created", GoodFilename);
            var badfilepath = Path.Combine(DestDir, BadFilename);
            File.WriteAllText(badfilepath, baddatBuilder.ToString());
            Console.WriteLine("{0} file was created", BadFilename);
        }

        private static Bitmap CropImage(Bitmap image, string destpath, Rectangle croparea)
        {
            var cropimage = image.Clone(croparea.ToStructRectangle(), image.PixelFormat);
            cropimage.Save(destpath);
            return cropimage;
        }

        private static void CreateDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                Console.WriteLine("Directory was created: {0}", path);
            }
            else
            {
                Console.WriteLine("Clear directory: {0}", path);
                var files = Directory.GetFiles(path, "*.*", SearchOption.TopDirectoryOnly);
                var i = 0;
                foreach (var file in files)
                {
                    File.Delete(file);
                    ++i;
                }
                Console.WriteLine("{0} files deleted", i);
            }
        }

        public static void ClearCurrentConsoleLine()
        {
            Console.SetCursorPosition(0, Console.CursorTop - 1);
            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentLineCursor);
        }
    }

    class Image
    {
        public string Name { get; private set; }
        public List<Rectangle> SignsAreas { get; private set; }
        public List<Rectangle> BadAreas { get; private set; }

        public Image(string name)
        {
            Name = name;
            SignsAreas = new List<Rectangle>();
            BadAreas = new List<Rectangle>();
        }
    }

    class Rectangle
    {
        public int X1 {get; private set;}
        public int Y1 {get; private set;}
        public int X2 {get; private set;}
        public int Y2 {get; private set;}

        public Rectangle(int x1, int y1, int x2, int y2)
        {
            X1 = x1;
            Y1 = y1;
            X2 = x2;
            Y2 = y2;
        }

        public bool Intersect(Rectangle r)
        {
            return !(X1 > r.X2 || X2 < r.X1 || Y1 > r.Y2 || Y2 < r.Y1);
        }

        public System.Drawing.Rectangle ToStructRectangle()
        {
            var rect = default(System.Drawing.Rectangle);
            rect.X = X1;
            rect.Y = Y1;
            rect.Width = X2 - X1;
            rect.Height = Y2 - Y1;
            return rect;
        }
    }
}
