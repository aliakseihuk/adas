using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;

namespace HaarPrecisionChecker
{
    public partial class TestForm : Form
    {
        public const string CascadPath = @"..\..\cascad\haarcascade\cascade.xml";
        public const string TestsPath = @"..\..\pictures\test_fullsize\";
        public const string GtFilename = "gt.txt";

        private readonly Timer timer_ = new Timer();
        private readonly ImageList detectedSigns_ = new ImageList();
        private readonly ImageList originalSigns_ = new ImageList();
        

        private int current_;
        private TestImage[] images_;
        private BackgroundWorker worker_;

        public TestForm()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            images_ = ParseGt();
            if (images_ == null)
                Close();

            originalList_.View = View.SmallIcon;
            originalList_.SmallImageList = originalSigns_;
            originalList_.SmallImageList.ImageSize = new Size(32, 32);

            detectedList_.View = View.SmallIcon;
            detectedList_.SmallImageList = detectedSigns_;
            detectedList_.SmallImageList.ImageSize = new Size(32, 32);

            progressBar_.Maximum = 100;
            progressBar_.Step = 1;
            progressBar_.Value = 0;

            statusLabel_.Text = "0.00%";

            timer_.Interval = 2000;
            timer_.Tick += TimerTick;
        }

        private Rectangle[] DetectMultiScale(CascadeClassifier classifier, Image<Gray, Byte> image)
        {
            return classifier.DetectMultiScale(image, 1.02, 1, new Size(10, 10), new Size(120, 120));
        }

        private void ProcessFrame()
        {
            if (current_ >= images_.Length)
            {
                timer_.Stop();
            }
            progressBar_.Value = (current_*100)/images_.Length;
            statusLabel_.Text = string.Format("{0:N0}%", ((double) current_*100)/images_.Length);
            string imagepath = Path.Combine(TestsPath, images_[current_].Name);
            var image = new Image<Bgr, byte>(imagepath);
            Image<Bgr, byte> procImage = image.Clone();
            using (var classifier = new CascadeClassifier(CascadPath))
            {
                using (Image<Gray, byte> grayimage = image.Convert<Gray, Byte>())
                {
                    Rectangle[] signregions = DetectMultiScale(classifier, grayimage);

                    originalSigns_.Images.Clear();
                    foreach (Rectangle rectangle in images_[current_].SignsAreas)
                    {
                        image.Draw(rectangle, new Bgr(Color.Chartreuse), 2);
                        Image<Bgr, byte> signimage = image.GetSubRect(rectangle);
                        originalSigns_.Images.Add(signimage.ToBitmap());
                    }

                    detectedSigns_.Images.Clear();
                    foreach (Rectangle rectangle in signregions)
                    {
                        procImage.Draw(rectangle, new Bgr(Color.Chartreuse), 2);
                        Image<Bgr, byte> signimage = procImage.GetSubRect(rectangle);
                        detectedSigns_.Images.Add(signimage.ToBitmap());
                    }
                    UpdateImageList();
                    originalBox_.Image = image.ToBitmap();
                    detectedBox_.Image = procImage.ToBitmap();
                }
            }
            ++current_;
        }

        private void DoWork(object sender, DoWorkEventArgs e)
        {
            var worker = (BackgroundWorker) sender;
            var statistic = new Statistic();
            for (int i = 0; i < images_.Length; ++i)
            {
                string imagepath = Path.Combine(TestsPath, images_[i].Name);
                var image = new Image<Bgr, byte>(imagepath);
                using (var classifier = new CascadeClassifier(CascadPath))
                {
                    using (Image<Gray, byte> grayimage = image.Convert<Gray, Byte>())
                    {
                        Rectangle[] detectedregions = DetectMultiScale(classifier, grayimage);
                        statistic.ProcessSigns(images_[i].SignsAreas.ToArray(), detectedregions);
                    }
                }
                worker.ReportProgress((i*100)/images_.Length);
            }
            e.Result = statistic;
        }

        private void ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar_.Value = e.ProgressPercentage;
            statusLabel_.Text = string.Format("{0:N0}%", e.ProgressPercentage);
        }

        private void RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var statistic = (Statistic) e.Result;
            double p = statistic.GetPrecision()*100;
            double m = statistic.GetMistake()*100;
            MessageBox.Show(string.Format("P: {0:N2}, M: {1:N2}", p, m));
        }

        private void UpdateImageList()
        {
            originalList_.Clear();
            detectedList_.Clear();

            for (int i = 0; i < originalSigns_.Images.Count; ++i)
            {
                var item = new ListViewItem { ImageIndex = i };
                originalList_.Items.Add(item);
            }
            for (int i = 0; i < detectedSigns_.Images.Count; ++i)
            {
                var item = new ListViewItem { ImageIndex = i };
                detectedList_.Items.Add(item);
            }
        }

        private void CalculateClick(object sender, EventArgs e)
        {
            worker_ = new BackgroundWorker { WorkerReportsProgress = true };
            worker_.DoWork += DoWork;
            worker_.ProgressChanged += ProgressChanged;
            worker_.RunWorkerCompleted += RunWorkerCompleted;
            worker_.RunWorkerAsync();
        }

        private void SlideshowClick(object sender, EventArgs e)
        {
            timer_.Start();
        }

        private TestImage[] ParseGt()
        {
            var dict = new Dictionary<string, TestImage>();
            string gtfullpath = Path.Combine(TestsPath, GtFilename);
            if (!File.Exists(gtfullpath))
            {
                MessageBox.Show("Gt file doesn't exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            using (var reader = new StreamReader(new FileStream(gtfullpath, FileMode.Open, FileAccess.Read)))
            {
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    if (line == null)
                        continue;
                    string[] info = line.Split(new[] {';'});
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

        private void TimerTick(object sender, EventArgs e)
        {
            ProcessFrame();
        }
    }
}