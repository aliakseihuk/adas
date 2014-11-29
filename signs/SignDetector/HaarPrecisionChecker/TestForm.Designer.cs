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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.runToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.calculateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.slideshowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.status = new System.Windows.Forms.StatusStrip();
            this.progressBar_ = new System.Windows.Forms.ToolStripProgressBar();
            this.statusLabel_ = new System.Windows.Forms.ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.originalBox_)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.detectedBox_)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.status.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.BackColor = System.Drawing.Color.White;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 24);
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
            this.splitContainer1.Size = new System.Drawing.Size(603, 515);
            this.splitContainer1.SplitterDistance = 226;
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
            this.originalBox_.Size = new System.Drawing.Size(482, 226);
            this.originalBox_.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.originalBox_.TabIndex = 0;
            this.originalBox_.TabStop = false;
            // 
            // originalList_
            // 
            this.originalList_.Dock = System.Windows.Forms.DockStyle.Right;
            this.originalList_.Location = new System.Drawing.Point(482, 0);
            this.originalList_.Name = "originalList_";
            this.originalList_.Size = new System.Drawing.Size(121, 226);
            this.originalList_.TabIndex = 2;
            this.originalList_.UseCompatibleStateImageBehavior = false;
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
            this.detectedBox_.Size = new System.Drawing.Size(482, 286);
            this.detectedBox_.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.detectedBox_.TabIndex = 0;
            this.detectedBox_.TabStop = false;
            // 
            // detectedList_
            // 
            this.detectedList_.Dock = System.Windows.Forms.DockStyle.Right;
            this.detectedList_.Location = new System.Drawing.Point(482, 0);
            this.detectedList_.Name = "detectedList_";
            this.detectedList_.Size = new System.Drawing.Size(121, 286);
            this.detectedList_.TabIndex = 2;
            this.detectedList_.UseCompatibleStateImageBehavior = false;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.runToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(603, 24);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // runToolStripMenuItem
            // 
            this.runToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.calculateToolStripMenuItem,
            this.slideshowToolStripMenuItem});
            this.runToolStripMenuItem.Name = "runToolStripMenuItem";
            this.runToolStripMenuItem.Size = new System.Drawing.Size(40, 20);
            this.runToolStripMenuItem.Text = "Run";
            // 
            // calculateToolStripMenuItem
            // 
            this.calculateToolStripMenuItem.Name = "calculateToolStripMenuItem";
            this.calculateToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
            this.calculateToolStripMenuItem.Text = "Calculate";
            this.calculateToolStripMenuItem.Click += new System.EventHandler(this.CalculateClick);
            // 
            // slideshowToolStripMenuItem
            // 
            this.slideshowToolStripMenuItem.Name = "slideshowToolStripMenuItem";
            this.slideshowToolStripMenuItem.Size = new System.Drawing.Size(128, 22);
            this.slideshowToolStripMenuItem.Text = "SlideShow";
            this.slideshowToolStripMenuItem.Click += new System.EventHandler(this.SlideshowClick);
            // 
            // status
            // 
            this.status.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.progressBar_,
            this.statusLabel_});
            this.status.Location = new System.Drawing.Point(0, 517);
            this.status.Name = "status";
            this.status.Size = new System.Drawing.Size(603, 22);
            this.status.TabIndex = 3;
            this.status.Text = "statusStrip1";
            // 
            // progressBar_
            // 
            this.progressBar_.Name = "progressBar_";
            this.progressBar_.Size = new System.Drawing.Size(100, 16);
            // 
            // statusLabel_
            // 
            this.statusLabel_.Name = "statusLabel_";
            this.statusLabel_.Size = new System.Drawing.Size(118, 17);
            this.statusLabel_.Text = "toolStripStatusLabel1";
            // 
            // TestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(603, 539);
            this.Controls.Add(this.status);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
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
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.status.ResumeLayout(false);
            this.status.PerformLayout();
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
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem runToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem calculateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem slideshowToolStripMenuItem;
        private System.Windows.Forms.StatusStrip status;
        private System.Windows.Forms.ToolStripProgressBar progressBar_;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel_;
    }
}

