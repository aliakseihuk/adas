using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
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

        private void ProcessFrame()
        {
            progressBar_.Value = (current_*100)/images_.Length;
            statusLabel_.Text = string.Format("{0:N0}%", ((double) current_*100)/images_.Length);
            var testImage = images_[current_];
            Image<Bgr, byte> procImage = testImage.Image.Clone();
            using (var classifier = new CascadeClassifier(CascadPath))
            {
               // Rectangle[] signregions = DetectMultiScale(classifier, testImage.GrayImage);

                originalSigns_.Images.Clear();
                foreach (Rectangle rectangle in images_[current_].SignsAreas)
                {
                    testImage.Image.Draw(rectangle, new Bgr(Color.Chartreuse), 2);
                    Image<Bgr, byte> signimage = testImage.Image.GetSubRect(rectangle);
                    originalSigns_.Images.Add(signimage.ToBitmap());
                }

                detectedSigns_.Images.Clear();
                //foreach (Rectangle rectangle in signregions)
                //{
                //    procImage.Draw(rectangle, new Bgr(Color.Chartreuse), 2);
                //    Image<Bgr, byte> signimage = procImage.GetSubRect(rectangle);
                //    detectedSigns_.Images.Add(signimage.ToBitmap());
                //}
                UpdateImageList();
                originalBox_.Image = testImage.Image.ToBitmap();
                detectedBox_.Image = procImage.ToBitmap();
            }
            ++current_;
        }

        private void DoWork(object sender, DoWorkEventArgs e)
        {
            var worker = (BackgroundWorker)sender;
            var statistic = new Statistic();
            using (var classifier = new Classifier())
            {
                for (var i = 0; i < images_.Length / 5; ++i)
                {
                    var testimage = images_[i];
                    testimage.DetectedAreas.AddRange(classifier.Detect(testimage.GrayImage));
                    statistic.ProcessSigns(testimage.SignsAreas.ToArray(), testimage.DetectedAreas.ToArray());
                    worker.ReportProgress(i);
                    if(preview_.Checked)
                        Thread.Sleep(1000);
                }
            }
            e.Result = statistic;
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
            progressBar_.Value = e.ProgressPercentage * 100 / images_.Length;
            statusLabel_.Text = string.Format("{0:N0}%", e.ProgressPercentage);

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
            var statistic = (Statistic) e.Result;
            double p = statistic.GetPrecision()*100;
            double m = statistic.GetMistake()*100;
            MessageBox.Show(string.Format("P: {0:N2}, M: {1:N2}, T: {2}", p, m, statistic.Time));
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
            worker_ = new BackgroundWorker { WorkerReportsProgress = true };
            worker_.DoWork += DoWork;
            worker_.ProgressChanged += ProgressChanged;
            worker_.RunWorkerCompleted += RunWorkerCompleted;
            worker_.RunWorkerAsync();
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

        private void TimerTick(object sender, EventArgs e)
        {
            ProcessFrame();
        }
    }
}