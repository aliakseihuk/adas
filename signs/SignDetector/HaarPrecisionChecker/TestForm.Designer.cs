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
            this.statisticList_ = new System.Windows.Forms.ListBox();
            this.runPanel_ = new System.Windows.Forms.Panel();
            this.statisticLabel_ = new System.Windows.Forms.Label();
            this.runAllButton_ = new System.Windows.Forms.Button();
            this.slowcheckbox_ = new System.Windows.Forms.CheckBox();
            this.preview_ = new System.Windows.Forms.CheckBox();
            this.runButton_ = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.originalBox_)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.detectedBox_)).BeginInit();
            this.status.SuspendLayout();
            this.panel1.SuspendLayout();
            this.runPanel_.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.BackColor = System.Drawing.Color.White;
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
            this.splitContainer1.Size = new System.Drawing.Size(496, 517);
            this.splitContainer1.SplitterDistance = 257;
            this.splitContainer1.SplitterWidth = 3;
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
            this.originalBox_.Size = new System.Drawing.Size(459, 257);
            this.originalBox_.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.originalBox_.TabIndex = 0;
            this.originalBox_.TabStop = false;
            // 
            // originalList_
            // 
            this.originalList_.BackColor = System.Drawing.Color.Black;
            this.originalList_.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.originalList_.Dock = System.Windows.Forms.DockStyle.Right;
            this.originalList_.Location = new System.Drawing.Point(459, 0);
            this.originalList_.Name = "originalList_";
            this.originalList_.Scrollable = false;
            this.originalList_.Size = new System.Drawing.Size(37, 257);
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
            this.detectedBox_.Size = new System.Drawing.Size(459, 257);
            this.detectedBox_.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.detectedBox_.TabIndex = 0;
            this.detectedBox_.TabStop = false;
            // 
            // detectedList_
            // 
            this.detectedList_.BackColor = System.Drawing.Color.Black;
            this.detectedList_.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.detectedList_.Dock = System.Windows.Forms.DockStyle.Right;
            this.detectedList_.Location = new System.Drawing.Point(459, 0);
            this.detectedList_.Name = "detectedList_";
            this.detectedList_.Scrollable = false;
            this.detectedList_.Size = new System.Drawing.Size(37, 257);
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
            this.panel1.Controls.Add(this.statisticList_);
            this.panel1.Controls.Add(this.runPanel_);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(496, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(235, 517);
            this.panel1.TabIndex = 3;
            // 
            // statisticList_
            // 
            this.statisticList_.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.statisticList_.Dock = System.Windows.Forms.DockStyle.Fill;
            this.statisticList_.FormattingEnabled = true;
            this.statisticList_.Location = new System.Drawing.Point(0, 116);
            this.statisticList_.Name = "statisticList_";
            this.statisticList_.Size = new System.Drawing.Size(235, 401);
            this.statisticList_.TabIndex = 0;
            // 
            // runPanel_
            // 
            this.runPanel_.BackColor = System.Drawing.Color.Black;
            this.runPanel_.Controls.Add(this.statisticLabel_);
            this.runPanel_.Controls.Add(this.runAllButton_);
            this.runPanel_.Controls.Add(this.slowcheckbox_);
            this.runPanel_.Controls.Add(this.preview_);
            this.runPanel_.Controls.Add(this.runButton_);
            this.runPanel_.Dock = System.Windows.Forms.DockStyle.Top;
            this.runPanel_.Enabled = false;
            this.runPanel_.Location = new System.Drawing.Point(0, 0);
            this.runPanel_.Name = "runPanel_";
            this.runPanel_.Size = new System.Drawing.Size(235, 116);
            this.runPanel_.TabIndex = 1;
            // 
            // statisticLabel_
            // 
            this.statisticLabel_.AutoSize = true;
            this.statisticLabel_.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statisticLabel_.ForeColor = System.Drawing.Color.Transparent;
            this.statisticLabel_.Location = new System.Drawing.Point(6, 111);
            this.statisticLabel_.Name = "statisticLabel_";
            this.statisticLabel_.Size = new System.Drawing.Size(0, 13);
            this.statisticLabel_.TabIndex = 4;
            // 
            // runAllButton_
            // 
            this.runAllButton_.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.runAllButton_.Location = new System.Drawing.Point(6, 41);
            this.runAllButton_.Name = "runAllButton_";
            this.runAllButton_.Size = new System.Drawing.Size(84, 23);
            this.runAllButton_.TabIndex = 3;
            this.runAllButton_.Text = "Run All";
            this.runAllButton_.UseVisualStyleBackColor = true;
            this.runAllButton_.Click += new System.EventHandler(this.RunAllClick);
            // 
            // slowcheckbox_
            // 
            this.slowcheckbox_.AutoSize = true;
            this.slowcheckbox_.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.slowcheckbox_.ForeColor = System.Drawing.Color.White;
            this.slowcheckbox_.Location = new System.Drawing.Point(7, 93);
            this.slowcheckbox_.Name = "slowcheckbox_";
            this.slowcheckbox_.Size = new System.Drawing.Size(129, 17);
            this.slowcheckbox_.TabIndex = 2;
            this.slowcheckbox_.Text = "Wait after detection";
            this.slowcheckbox_.UseVisualStyleBackColor = true;
            // 
            // preview_
            // 
            this.preview_.AutoSize = true;
            this.preview_.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.preview_.ForeColor = System.Drawing.Color.White;
            this.preview_.Location = new System.Drawing.Point(7, 70);
            this.preview_.Name = "preview_";
            this.preview_.Size = new System.Drawing.Size(98, 17);
            this.preview_.TabIndex = 1;
            this.preview_.Text = "Show preview";
            this.preview_.UseVisualStyleBackColor = true;
            // 
            // runButton_
            // 
            this.runButton_.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.runButton_.Location = new System.Drawing.Point(6, 12);
            this.runButton_.Name = "runButton_";
            this.runButton_.Size = new System.Drawing.Size(84, 23);
            this.runButton_.TabIndex = 0;
            this.runButton_.Text = "Run";
            this.runButton_.UseVisualStyleBackColor = true;
            this.runButton_.Click += new System.EventHandler(this.RunClick);
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
            this.runPanel_.ResumeLayout(false);
            this.runPanel_.PerformLayout();
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
        private System.Windows.Forms.Panel runPanel_;
        private System.Windows.Forms.ListBox statisticList_;
        private System.Windows.Forms.Button runButton_;
        private System.Windows.Forms.CheckBox preview_;
        private System.Windows.Forms.CheckBox slowcheckbox_;
        private System.Windows.Forms.Button runAllButton_;
        private System.Windows.Forms.Label statisticLabel_;
    }
}

