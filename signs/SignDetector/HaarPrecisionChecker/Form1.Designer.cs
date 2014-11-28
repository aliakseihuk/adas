namespace HaarPrecisionChecker
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            this.orignalList_ = new System.Windows.Forms.ListBox();
            this.detectedList_ = new System.Windows.Forms.ListBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.label1 = new System.Windows.Forms.Label();
            this.originalBox_ = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.detectedBox_ = new System.Windows.Forms.PictureBox();
            this.appStylistRuntime1 = new Infragistics.Win.AppStyling.Runtime.AppStylistRuntime(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.originalBox_)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.detectedBox_)).BeginInit();
            this.SuspendLayout();
            // 
            // orignalList_
            // 
            this.orignalList_.Dock = System.Windows.Forms.DockStyle.Right;
            this.orignalList_.FormattingEnabled = true;
            this.orignalList_.Location = new System.Drawing.Point(497, 0);
            this.orignalList_.Name = "orignalList_";
            this.orignalList_.Size = new System.Drawing.Size(186, 516);
            this.orignalList_.TabIndex = 1;
            // 
            // detectedList_
            // 
            this.detectedList_.Dock = System.Windows.Forms.DockStyle.Right;
            this.detectedList_.FormattingEnabled = true;
            this.detectedList_.Location = new System.Drawing.Point(683, 0);
            this.detectedList_.Name = "detectedList_";
            this.detectedList_.Size = new System.Drawing.Size(186, 516);
            this.detectedList_.TabIndex = 2;
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 516);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(869, 23);
            this.panel1.TabIndex = 3;
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
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.label2);
            this.splitContainer1.Panel2.Controls.Add(this.detectedBox_);
            this.splitContainer1.Size = new System.Drawing.Size(497, 516);
            this.splitContainer1.SplitterDistance = 227;
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
            this.originalBox_.Size = new System.Drawing.Size(497, 227);
            this.originalBox_.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.originalBox_.TabIndex = 0;
            this.originalBox_.TabStop = false;
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
            this.detectedBox_.Size = new System.Drawing.Size(497, 286);
            this.detectedBox_.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.detectedBox_.TabIndex = 0;
            this.detectedBox_.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(869, 539);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.orignalList_);
            this.Controls.Add(this.detectedList_);
            this.Controls.Add(this.panel1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.originalBox_)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.detectedBox_)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox orignalList_;
        private System.Windows.Forms.ListBox detectedList_;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.PictureBox originalBox_;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox detectedBox_;
        private System.Windows.Forms.Label label2;
        private Infragistics.Win.AppStyling.Runtime.AppStylistRuntime appStylistRuntime1;
    }
}

