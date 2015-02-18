using Adas.Ui.Controls;

namespace Adas.Ui
{
    partial class MainForm
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this._p2Edit = new Adas.Ui.Controls.ValueEdit();
            this._p1Edit = new Adas.Ui.Controls.ValueEdit();
            this._sadWindowSizeEdit = new Adas.Ui.Controls.ValueEdit();
            this._numDisparityEdit = new Adas.Ui.Controls.ValueEdit();
            this._minDisparityEdit = new Adas.Ui.Controls.ValueEdit();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(179, 430);
            this.panel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.groupBox1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Margin = new System.Windows.Forms.Padding(0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(179, 206);
            this.panel2.TabIndex = 4;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this._p2Edit);
            this.groupBox1.Controls.Add(this._p1Edit);
            this.groupBox1.Controls.Add(this._sadWindowSizeEdit);
            this.groupBox1.Controls.Add(this._numDisparityEdit);
            this.groupBox1.Controls.Add(this._minDisparityEdit);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(179, 206);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "groupBox1";
            // 
            // _p2Edit
            // 
            this._p2Edit.Dock = System.Windows.Forms.DockStyle.Top;
            this._p2Edit.Location = new System.Drawing.Point(3, 100);
            this._p2Edit.Name = "_p2Edit";
            this._p2Edit.Size = new System.Drawing.Size(173, 21);
            this._p2Edit.TabIndex = 4;
            // 
            // _p1Edit
            // 
            this._p1Edit.Dock = System.Windows.Forms.DockStyle.Top;
            this._p1Edit.Location = new System.Drawing.Point(3, 79);
            this._p1Edit.Name = "_p1Edit";
            this._p1Edit.Size = new System.Drawing.Size(173, 21);
            this._p1Edit.TabIndex = 3;
            // 
            // _sadWindowSizeEdit
            // 
            this._sadWindowSizeEdit.Dock = System.Windows.Forms.DockStyle.Top;
            this._sadWindowSizeEdit.Location = new System.Drawing.Point(3, 58);
            this._sadWindowSizeEdit.Name = "_sadWindowSizeEdit";
            this._sadWindowSizeEdit.Size = new System.Drawing.Size(173, 21);
            this._sadWindowSizeEdit.TabIndex = 2;
            // 
            // _numDisparityEdit
            // 
            this._numDisparityEdit.Dock = System.Windows.Forms.DockStyle.Top;
            this._numDisparityEdit.Location = new System.Drawing.Point(3, 37);
            this._numDisparityEdit.Name = "_numDisparityEdit";
            this._numDisparityEdit.Size = new System.Drawing.Size(173, 21);
            this._numDisparityEdit.TabIndex = 1;
            // 
            // _minDisparityEdit
            // 
            this._minDisparityEdit.Dock = System.Windows.Forms.DockStyle.Top;
            this._minDisparityEdit.Location = new System.Drawing.Point(3, 16);
            this._minDisparityEdit.Name = "_minDisparityEdit";
            this._minDisparityEdit.Size = new System.Drawing.Size(173, 21);
            this._minDisparityEdit.TabIndex = 0;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 430);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(689, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(689, 452);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.statusStrip1);
            this.Name = "MainForm";
            this.Text = "Form1";
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private ValueEdit _minDisparityEdit;
        private System.Windows.Forms.Panel panel2;
        private ValueEdit _p1Edit;
        private ValueEdit _sadWindowSizeEdit;
        private ValueEdit _numDisparityEdit;
        private System.Windows.Forms.GroupBox groupBox1;
        private ValueEdit _p2Edit;
    }
}

