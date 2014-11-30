using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
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

        private BackgroundWorker worker_;
        private readonly ImageList detectedSigns_ = new ImageList();
        private readonly ImageList originalSigns_ = new ImageList();
        

        private int current_;
        private TestImage[] images_;
        

        public TestForm()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            LoadImages();

            originalList_.View = View.SmallIcon;
            originalList_.SmallImageList = originalSigns_;
            originalList_.SmallImageList.ImageSize = new Size(32, 32);

            detectedList_.View = View.SmallIcon;
            detectedList_.SmallImageList = detectedSigns_;
            detectedList_.SmallImageList.ImageSize = new Size(32, 32);
        }

        private void LoadImages()
        {
            statusLabel_.Text = "Loading images";

            worker_ = new BackgroundWorker { WorkerReportsProgress = true };
            worker_.DoWork += LoadImagesAsync;
            worker_.RunWorkerCompleted += (sender, args) =>
            {
                runPanel_.Enabled = true;
                statusLabel_.Text = "Completed";
                progressBar_.Style = ProgressBarStyle.Continuous;
            };
            worker_.RunWorkerAsync();
        }

        private void DoWork(object sender, DoWorkEventArgs e)
        {
            const double minfactor = 1.02;
            const double maxfaxtor = 1.10;
            const double factorstep = 0.02;

            const int minminneighbours = 1;
            const int maxminneighbours = 11;
            const int neighbourstep = 2;

            //const double minfactor = 1.05;
            //const double maxfaxtor = 1.10;
            //const double factorstep = 0.05;

            //const int minminneighbours = 4;
            //const int maxminneighbours = 9;
            //const int neighbourstep = 5;

            var all = (bool) e.Argument;
            var worker = (BackgroundWorker) sender;
            using (var classifier = new Classifier())
            {
                if (all)
                {
                    for (var factor = minfactor; factor <= maxfaxtor; factor += factorstep)
                    {
                        for (var minn = minminneighbours; minn <= maxminneighbours; minn += neighbourstep)
                        {
                            classifier.ScaleFactor = factor;
                            classifier.MinNeighbours = minn;
                            statisticList_.Items.Add(CalculateStatistic(classifier, worker.ReportProgress));
                        }
                    }
                }
                else
                {
                    classifier.ScaleFactor = 1.10;
                    classifier.MinNeighbours = 6;
                    statisticList_.Items.Add(CalculateStatistic(classifier, worker.ReportProgress));
                }
            }
        }

        private Statistic CalculateStatistic(Classifier classifier, Action<int> report)
        {
            var statistic = new Statistic(classifier.ScaleFactor, classifier.MinNeighbours);
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            for (var i = 0; i < images_.Length; ++i)
            {
                var testimage = images_[i];
                testimage.DetectedAreas.AddRange(classifier.Detect(testimage.GrayImage));
                statistic.ProcessSigns(testimage.SignsAreas.ToArray(), testimage.DetectedAreas.ToArray());
                report(i);
                if(slowcheckbox_.Checked)
                    Thread.Sleep(1000);
            }
            stopWatch.Stop();
            statistic.Time = stopWatch.ElapsedMilliseconds/1000;
            return statistic;
        }

        private void LoadImagesAsync(object sender, DoWorkEventArgs e)
        {
            images_ = ParseGt();
            if (images_ == null)
                return;
            foreach (var testimage in images_)
                testimage.LoadImages(TestsPath);
        }

        private void ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            var percent = e.ProgressPercentage * 100 / images_.Length;
            progressBar_.Value = percent;
            statusLabel_.Text = string.Format("{0:N0}%", percent);

            if (preview_.Checked)
            {
                originalList_.Visible = true;
                detectedList_.Visible = true;
                ShowDetection(e.ProgressPercentage);
            }
            else
            {
                originalList_.Visible = false;
                detectedList_.Visible = false;
                originalBox_.Image = null;
                detectedBox_.Image = null;
            }
        }

        private void ShowDetection(int i)
        {
            var testimage = images_[i];
            var org = testimage.Image.Clone();
            var det = testimage.Image.Clone();

            originalSigns_.Images.Clear();
            foreach (var rectangle in testimage.SignsAreas)
            {
                org.Draw(rectangle, new Bgr(Color.Chartreuse), 2);
                Image<Bgr, byte> signimage = org.GetSubRect(rectangle);
                originalSigns_.Images.Add(signimage.ToBitmap());
            }

            detectedSigns_.Images.Clear();
            foreach (var rectangle in testimage.DetectedAreas)
            {
                det.Draw(rectangle, new Bgr(Color.Chartreuse), 2);
                Image<Bgr, byte> signimage = det.GetSubRect(rectangle);
                detectedSigns_.Images.Add(signimage.ToBitmap());
            }

            UpdateImageList();
            originalBox_.Image = org.ToBitmap();
            detectedBox_.Image = det.ToBitmap();
        }

        private void RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            runButton_.Enabled = true;
            runAllButton_.Enabled = true;
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

        private void RunClick(object sender, EventArgs e)
        {
            StartWorker(false);
        }

        private void RunAllClick(object sender, EventArgs e)
        {
            StartWorker(true);
        }

        private void StartWorker(bool all)
        {
            statisticList_.Items.Clear();
            runButton_.Enabled = false;
            runAllButton_.Enabled = false;
            
            worker_ = new BackgroundWorker { WorkerReportsProgress = true };
            worker_.DoWork += DoWork;
            worker_.ProgressChanged += ProgressChanged;
            worker_.RunWorkerCompleted += RunWorkerCompleted;
            worker_.RunWorkerAsync(all);
        }

        private TestImage[] ParseGt()
        {
            var dict = new Dictionary<string, TestImage>();
            string gtfullpath = Path.Combine(TestsPath, GtFilename);
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
    }
}