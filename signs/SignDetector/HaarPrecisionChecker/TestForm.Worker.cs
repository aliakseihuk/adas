using System.ComponentModel;
using System.Windows.Forms;

namespace HaarPrecisionChecker
{
    public partial class TestForm
    {
        private BackgroundWorker worker_;

        private void LoadImages()
        {
            CreateNewWorker();
            worker_.DoWork += LoadImagesAsync;
            worker_.RunWorkerAsync();
        }

        private void RunDetection()
        {
            state_ = RunState.Run;
            var configuration = new RunConfiguration
            {
                MinNeighbours = MinNeighbours,
                Scale = Scale
            };
            CreateNewWorker();
            worker_.DoWork += RunDetectionAsync;
            worker_.RunWorkerAsync(configuration);
        }

        private void RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            runButton_.Enabled = true;
            runButton_.Text = "Run";
            state_ = RunState.Wait;
            statusLabel_.Text = "Completed";
            progressBar_.Style = ProgressBarStyle.Marquee;
        }

        private void ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            var imageIndex = e.ProgressPercentage;
            var percent = imageIndex * 100 / images_.Length;

            progressBar_.Value = percent;
            statusLabel_.Text = string.Format(
                state_ == RunState.Load ? "{0}% loaded images" : "{0}% processed images",
                percent);

            if (state_ == RunState.Run)
            {
                ClearAreas();
                if (preview_.Checked)
                    ShowImageDetection(imageIndex);
            }
        }

        private void CreateNewWorker()
        {
            worker_ = new BackgroundWorker { WorkerReportsProgress = true };
            worker_.ProgressChanged += ProgressChanged;
            worker_.RunWorkerCompleted += RunWorkerCompleted;
        }
    }

    internal enum RunState
    {
        Load,
        Wait,
        Run
    }
}
