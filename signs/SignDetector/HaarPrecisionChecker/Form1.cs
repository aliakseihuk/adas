using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.OCR;
using Emgu.Util;
using Emgu.CV.Structure;
using Emgu.CV.Features2D;
using Emgu.CV.CvEnum;
using Emgu.CV.Util;
using Emgu.CV.UI;

namespace HaarPrecisionChecker
{
    public partial class Form1 : Form
    {
        private readonly Timer timer_ = new Timer();

        public const string CascadPath = @"..\..\cascad\fakecascad\cascade.xml";
        public const string TestsPath = @"..\..\pictures\test_fullsize\";
        public const string GtFilename = "gt.txt";

        private TestImage[] images_;
        private ImageList originalSigns_ = new ImageList();
        private ImageList detectedSigns_ = new ImageList();

        private int current_ = 0;
        
        public Form1()
        {
            InitializeComponent();
            Init(); //Запускаем камеру

            timer_.Start();          
        }

        void TimerTick(object sender, EventArgs e)
        {
            ProcessFrame();
        }

        private void Init()
        {
            images_ = ParseGt();
            if(images_ == null)
                Close();

            //listView1.SmallImageList = originalSigns_;
            //detectedList_.DataSource = detectedSigns_;

            timer_.Interval = 1000;
            timer_.Tick += TimerTick;
        }


        void ProcessFrame()
        {
            if (current_ >= images_.Length)
            {
                timer_.Stop();
            }
            var imagepath = Path.Combine(TestsPath, images_[current_].Name);
            var image = new Image<Bgr, byte>(imagepath);
            var procImage = image.Clone();
            using (var face = new CascadeClassifier(CascadPath))
            {
                using (Image<Gray, Byte> grayimage = image.Convert<Gray, Byte>())
                {
                    var signregions = face.DetectMultiScale(grayimage, 1.1, 6, new Size(10, 10), Size.Empty);

                    originalSigns_.Images.Clear();
                    foreach (var rectangle in images_[current_].SignsAreas)
                    {
                        image.Draw(rectangle, new Bgr(Color.Chartreuse), 2);
                        //var signimage = image.GetSubRect(rectangle);
                        //originalSigns_.Images.Add(signimage.ToBitmap());
                    }

                    detectedSigns_.Images.Clear();
                    foreach (var rectangle in signregions)
                    {
                        procImage.Draw(rectangle, new Bgr(Color.Chartreuse), 2);
                        //var signimage = procImage.GetSubRect(rectangle);
                        //detectedSigns_.Images.Add(signimage.ToBitmap());
                    }

                    originalBox_.Image = image.ToBitmap();
                    detectedBox_.Image = procImage.ToBitmap();
                }
            }
            ++current_;
        }


        private TestImage[] ParseGt()
        {
            var dict = new Dictionary<string, TestImage>();
            var gtfullpath = Path.Combine(TestsPath, GtFilename);
            if (!File.Exists(gtfullpath))
            {
                MessageBox.Show("Gt file doesn't exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    var name = info[0];
                    var x1 = int.Parse(info[1]);
                    var y1 = int.Parse(info[2]);
                    var x2 = int.Parse(info[3]);
                    var y2 = int.Parse(info[4]);
                    if (!dict.ContainsKey(name))
                        dict[name] = new TestImage(name);
                    var area = new Rectangle(x1, y1, x2 - x1, y2 - y1);
                    dict[name].SignsAreas.Add(area);
                }
            }
            return dict.Values.ToArray();
        }
    }

    class TestImage
    {
        public string Name { get; private set; }
        public List<Rectangle> SignsAreas { get; private set; }

        public TestImage(string name)
        {
            Name = name;
            SignsAreas = new List<Rectangle>();
        }
    }
}

