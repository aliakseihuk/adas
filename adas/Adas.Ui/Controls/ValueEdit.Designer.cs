namespace Adas.Ui.Controls
{
    partial class ValueEdit
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this._nameLabel = new System.Windows.Forms.Label();
            this._valueBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // _nameLabel
            // 
            this._nameLabel.AutoSize = true;
            this._nameLabel.Dock = System.Windows.Forms.DockStyle.Right;
            this._nameLabel.Location = new System.Drawing.Point(30, 0);
            this._nameLabel.Name = "_nameLabel";
            this._nameLabel.Size = new System.Drawing.Size(35, 13);
            this._nameLabel.TabIndex = 0;
            this._nameLabel.Text = "Name";
            // 
            // _valueBox
            // 
            this._valueBox.Dock = System.Windows.Forms.DockStyle.Right;
            this._valueBox.Location = new System.Drawing.Point(65, 0);
            this._valueBox.Name = "_valueBox";
            this._valueBox.Size = new System.Drawing.Size(51, 20);
            this._valueBox.TabIndex = 1;
            // 
            // ValueEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._nameLabel);
            this.Controls.Add(this._valueBox);
            this.Name = "ValueEdit";
            this.Size = new System.Drawing.Size(116, 21);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label _nameLabel;
        private System.Windows.Forms.TextBox _valueBox;
    }
}
