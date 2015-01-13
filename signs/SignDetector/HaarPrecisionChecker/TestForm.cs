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
        
        private readonly ImageList detectedSigns_ = new ImageList();
        private readonly ImageList originalSigns_ = new ImageList();

        private int current_;
        private TestImage[] images_;

        private RunState state_ = RunState.Load;

        public TestForm()
        {
            InitializeComponent();
            Init();
        }

        public double Scale
        {
            get { return RunConfiguration.MinScale + ((double)scaleTrackBar_.Value) / 100; }
        }

        public int MinNeighbours
        {
            get { return RunConfiguration.MinMinNeighbours + minnTrackBar_.Value; }
        }

        private void Init()
        {
            originalList_.View = View.SmallIcon;
            originalList_.SmallImageList = originalSigns_;
            originalList_.SmallImageList.ImageSize = new Size(32, 32);

            detectedList_.View = View.SmallIcon;
            detectedList_.SmallImageList = detectedSigns_;
            detectedList_.SmallImageList.ImageSize = new Size(32, 32);
        }

        private void RunClick(object sender, EventArgs e)
        {
            runButton_.Enabled = false;
            progressBar_.Style = ProgressBarStyle.Continuous;
            if (state_ == RunState.Load)
            {
                LoadImages();
            }
            else if (state_ == RunState.Wait)
            {
                RunDetection();
            }
        }

        private void LoadImagesAsync(object sender, DoWorkEventArgs e)
        {
            images_ = GtHelper.ParseGt(TestsPath);
            if (images_ == null)
                return;
            for (var i = 0; i < images_.Length; ++i)
            {
                images_[i].LoadImages(TestsPath);
                worker_.ReportProgress(i);
            }
        }

        private void RunDetectionAsync(object sender, DoWorkEventArgs e)
        {
            var worker = (BackgroundWorker)sender;
            var configuration = (RunConfiguration)e.Argument;
            using (var classifier = new Classifier())
            {
                classifier.ScaleFactor = configuration.Scale;
                classifier.MinNeighbours = configuration.MinNeighbours;
                CalculateStatistic(classifier, worker.ReportProgress);
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
                testimage.DetectedAreas.Clear();
                testimage.DetectedAreas.AddRange(classifier.Detect(testimage.GrayImage));
                statistic.ProcessSigns(testimage.SignsAreas.ToArray(), testimage.DetectedAreas.ToArray());
                report(i);
                if (slowcheckbox_.Checked)
                    Thread.Sleep(1000);
            }
            stopWatch.Stop();
            statistic.Time = stopWatch.ElapsedMilliseconds / 1000;
            return statistic;
        }

        private void ShowImageDetection(int i)
        {
            originalList_.Visible = true;
            detectedList_.Visible = true;

            var testimage = images_[i];
            var org = testimage.Image.Clone();
            var det = testimage.Image.Clone();

            foreach (var rectangle in testimage.SignsAreas)
            {
                org.Draw(rectangle, new Bgr(Color.Chartreuse), 2);
                Image<Bgr, byte> signimage = org.GetSubRect(rectangle);
                originalSigns_.Images.Add(signimage.ToBitmap());
            }

            foreach (var rectangle in testimage.DetectedAreas)
            {
                det.Draw(rectangle, new Bgr(Color.Chartreuse), 2);
                Image<Bgr, byte> signimage = det.GetSubRect(rectangle);
                detectedSigns_.Images.Add(signimage.ToBitmap());
            }

            UpdateImageLists();
            originalBox_.Image = org.ToBitmap();
            detectedBox_.Image = det.ToBitmap();
        }

        private void UpdateImageLists()
        {
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

        private void ClearAreas()
        {
            originalList_.Visible = false;
            detectedList_.Visible = false;
            originalList_.Items.Clear();
            detectedList_.Items.Clear();
            originalBox_.Image = null;
            detectedBox_.Image = null;
            originalSigns_.Images.Clear();
            detectedSigns_.Images.Clear();
        }

        private void ScaleValueChanged(object sender, EventArgs e)
        {
            scaleValueLabel_.Text = Scale.ToString();
        }

        private void MinNeighbValueChanged(object sender, EventArgs e)
        {
            minnValueLabel_.Text = MinNeighbours.ToString();
        }      
    }

    internal class RunConfiguration
    {
        public const int MinMinNeighbours = 1;
        public const double MinScale = 1.02;

        public RunConfiguration()
        {
            Scale = MinScale;
            MinNeighbours = MinMinNeighbours;
        }

        public double Scale { get; set; }
        public int MinNeighbours { get; set; }
    }

    
}