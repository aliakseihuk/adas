namespace HaarPrecisionChecker
{
    partial class TestForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.label1 = new System.Windows.Forms.Label();
            this.originalBox_ = new System.Windows.Forms.PictureBox();
            this.originalList_ = new System.Windows.Forms.ListView();
            this.label2 = new System.Windows.Forms.Label();
            this.detectedBox_ = new System.Windows.Forms.PictureBox();
            this.detectedList_ = new System.Windows.Forms.ListView();
            this.status = new System.Windows.Forms.StatusStrip();
            this.progressBar_ = new System.Windows.Forms.ToolStripProgressBar();
            this.statusLabel_ = new System.Windows.Forms.ToolStripStatusLabel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.minnValueLabel_ = new System.Windows.Forms.Label();
            this.scaleValueLabel_ = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.scaleTrackBar_ = new System.Windows.Forms.TrackBar();
            this.minnTrackBar_ = new System.Windows.Forms.TrackBar();
            this.slowcheckbox_ = new System.Windows.Forms.CheckBox();
            this.runButton_ = new System.Windows.Forms.Button();
            this.preview_ = new System.Windows.Forms.CheckBox();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.originalBox_)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.detectedBox_)).BeginInit();
            this.status.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scaleTrackBar_)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.minnTrackBar_)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.BackColor = System.Drawing.Color.White;
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            this.splitContainer1.Panel1.Controls.Add(this.originalBox_);
            this.splitContainer1.Panel1.Controls.Add(this.originalList_);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.label2);
            this.splitContainer1.Panel2.Controls.Add(this.detectedBox_);
            this.splitContainer1.Panel2.Controls.Add(this.detectedList_);
            this.splitContainer1.Size = new System.Drawing.Size(610, 517);
            this.splitContainer1.SplitterDistance = 257;
            this.splitContainer1.SplitterWidth = 1;
            this.splitContainer1.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Black;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 21);
            this.label1.TabIndex = 1;
            this.label1.Text = "Original";
            // 
            // originalBox_
            // 
            this.originalBox_.BackColor = System.Drawing.Color.Black;
            this.originalBox_.Dock = System.Windows.Forms.DockStyle.Fill;
            this.originalBox_.Location = new System.Drawing.Point(0, 0);
            this.originalBox_.Name = "originalBox_";
            this.originalBox_.Size = new System.Drawing.Size(569, 253);
            this.originalBox_.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.originalBox_.TabIndex = 0;
            this.originalBox_.TabStop = false;
            // 
            // originalList_
            // 
            this.originalList_.BackColor = System.Drawing.Color.Black;
            this.originalList_.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.originalList_.Dock = System.Windows.Forms.DockStyle.Right;
            this.originalList_.Location = new System.Drawing.Point(569, 0);
            this.originalList_.Name = "originalList_";
            this.originalList_.Scrollable = false;
            this.originalList_.Size = new System.Drawing.Size(37, 253);
            this.originalList_.TabIndex = 2;
            this.originalList_.UseCompatibleStateImageBehavior = false;
            this.originalList_.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Black;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 21);
            this.label2.TabIndex = 1;
            this.label2.Text = "Detected";
            // 
            // detectedBox_
            // 
            this.detectedBox_.BackColor = System.Drawing.Color.Black;
            this.detectedBox_.Dock = System.Windows.Forms.DockStyle.Fill;
            this.detectedBox_.Location = new System.Drawing.Point(0, 0);
            this.detectedBox_.Name = "detectedBox_";
            this.detectedBox_.Size = new System.Drawing.Size(569, 255);
            this.detectedBox_.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.detectedBox_.TabIndex = 0;
            this.detectedBox_.TabStop = false;
            // 
            // detectedList_
            // 
            this.detectedList_.BackColor = System.Drawing.Color.Black;
            this.detectedList_.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.detectedList_.Dock = System.Windows.Forms.DockStyle.Right;
            this.detectedList_.ForeColor = System.Drawing.Color.White;
            this.detectedList_.Location = new System.Drawing.Point(569, 0);
            this.detectedList_.Name = "detectedList_";
            this.detectedList_.Scrollable = false;
            this.detectedList_.Size = new System.Drawing.Size(37, 255);
            this.detectedList_.TabIndex = 2;
            this.detectedList_.UseCompatibleStateImageBehavior = false;
            this.detectedList_.Visible = false;
            // 
            // status
            // 
            this.status.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.progressBar_,
            this.statusLabel_});
            this.status.Location = new System.Drawing.Point(0, 517);
            this.status.Name = "status";
            this.status.Size = new System.Drawing.Size(731, 22);
            this.status.TabIndex = 3;
            this.status.Text = "statusStrip1";
            // 
            // progressBar_
            // 
            this.progressBar_.MarqueeAnimationSpeed = 20;
            this.progressBar_.Name = "progressBar_";
            this.progressBar_.Size = new System.Drawing.Size(200, 16);
            this.progressBar_.Step = 1;
            this.progressBar_.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            // 
            // statusLabel_
            // 
            this.statusLabel_.Name = "statusLabel_";
            this.statusLabel_.Size = new System.Drawing.Size(0, 17);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Black;
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.slowcheckbox_);
            this.panel1.Controls.Add(this.runButton_);
            this.panel1.Controls.Add(this.preview_);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(610, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(121, 517);
            this.panel1.TabIndex = 3;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Black;
            this.panel2.Controls.Add(this.minnValueLabel_);
            this.panel2.Controls.Add(this.scaleValueLabel_);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.scaleTrackBar_);
            this.panel2.Controls.Add(this.minnTrackBar_);
            this.panel2.Location = new System.Drawing.Point(6, 12);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(107, 209);
            this.panel2.TabIndex = 6;
            // 
            // minnValueLabel_
            // 
            this.minnValueLabel_.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.minnValueLabel_.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.minnValueLabel_.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.minnValueLabel_.ForeColor = System.Drawing.Color.White;
            this.minnValueLabel_.Location = new System.Drawing.Point(61, 188);
            this.minnValueLabel_.Name = "minnValueLabel_";
            this.minnValueLabel_.Size = new System.Drawing.Size(39, 15);
            this.minnValueLabel_.TabIndex = 9;
            this.minnValueLabel_.Text = "1";
            this.minnValueLabel_.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // scaleValueLabel_
            // 
            this.scaleValueLabel_.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.scaleValueLabel_.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.scaleValueLabel_.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.scaleValueLabel_.ForeColor = System.Drawing.Color.White;
            this.scaleValueLabel_.Location = new System.Drawing.Point(7, 188);
            this.scaleValueLabel_.Name = "scaleValueLabel_";
            this.scaleValueLabel_.Size = new System.Drawing.Size(45, 15);
            this.scaleValueLabel_.TabIndex = 8;
            this.scaleValueLabel_.Text = "1.02";
            this.scaleValueLabel_.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(58, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(43, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Min N.";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(10, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(33, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Scale";
            // 
            // scaleTrackBar_
            // 
            this.scaleTrackBar_.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.scaleTrackBar_.Location = new System.Drawing.Point(7, 16);
            this.scaleTrackBar_.Maximum = 8;
            this.scaleTrackBar_.Name = "scaleTrackBar_";
            this.scaleTrackBar_.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.scaleTrackBar_.Size = new System.Drawing.Size(45, 174);
            this.scaleTrackBar_.TabIndex = 5;
            this.scaleTrackBar_.TickFrequency = 2;
            this.scaleTrackBar_.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.scaleTrackBar_.ValueChanged += new System.EventHandler(this.ScaleValueChanged);
            // 
            // minnTrackBar_
            // 
            this.minnTrackBar_.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.minnTrackBar_.Location = new System.Drawing.Point(58, 16);
            this.minnTrackBar_.Maximum = 20;
            this.minnTrackBar_.Name = "minnTrackBar_";
            this.minnTrackBar_.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.minnTrackBar_.Size = new System.Drawing.Size(45, 174);
            this.minnTrackBar_.TabIndex = 4;
            this.minnTrackBar_.TickFrequency = 2;
            this.minnTrackBar_.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.minnTrackBar_.ValueChanged += new System.EventHandler(this.MinNeighbValueChanged);
            // 
            // slowcheckbox_
            // 
            this.slowcheckbox_.AutoSize = true;
            this.slowcheckbox_.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.slowcheckbox_.ForeColor = System.Drawing.Color.White;
            this.slowcheckbox_.Location = new System.Drawing.Point(10, 288);
            this.slowcheckbox_.Name = "slowcheckbox_";
            this.slowcheckbox_.Size = new System.Drawing.Size(50, 17);
            this.slowcheckbox_.TabIndex = 2;
            this.slowcheckbox_.Text = "Wait";
            this.slowcheckbox_.UseVisualStyleBackColor = true;
            // 
            // runButton_
            // 
            this.runButton_.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.runButton_.Location = new System.Drawing.Point(6, 232);
            this.runButton_.Name = "runButton_";
            this.runButton_.Size = new System.Drawing.Size(107, 23);
            this.runButton_.TabIndex = 0;
            this.runButton_.Text = "Load";
            this.runButton_.UseVisualStyleBackColor = true;
            this.runButton_.Click += new System.EventHandler(this.RunClick);
            // 
            // preview_
            // 
            this.preview_.AutoSize = true;
            this.preview_.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.preview_.ForeColor = System.Drawing.Color.White;
            this.preview_.Location = new System.Drawing.Point(10, 265);
            this.preview_.Name = "preview_";
            this.preview_.Size = new System.Drawing.Size(99, 17);
            this.preview_.TabIndex = 1;
            this.preview_.Text = "Show preview";
            this.preview_.UseVisualStyleBackColor = true;
            // 
            // TestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(731, 539);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.status);
            this.Name = "TestForm";
            this.Text = "TestForm";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.originalBox_)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.detectedBox_)).EndInit();
            this.status.ResumeLayout(false);
            this.status.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scaleTrackBar_)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.minnTrackBar_)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.PictureBox originalBox_;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox detectedBox_;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListView originalList_;
        private System.Windows.Forms.ListView detectedList_;
        private System.Windows.Forms.StatusStrip status;
        private System.Windows.Forms.ToolStripProgressBar progressBar_;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel_;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button runButton_;
        private System.Windows.Forms.CheckBox preview_;
        private System.Windows.Forms.CheckBox slowcheckbox_;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label minnValueLabel_;
        private System.Windows.Forms.Label scaleValueLabel_;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TrackBar scaleTrackBar_;
        private System.Windows.Forms.TrackBar minnTrackBar_;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
    }
}

