using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Adas.Ui
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            
            InitValueEdits();
        }

        private void InitValueEdits()
        {
            _minDisparityEdit.NameLabelText = "Min disparity";
            _minDisparityEdit.SetDefaultValue(0);

            _numDisparityEdit.NameLabelText = "Num disparity(x16)";
            _numDisparityEdit.SetDefaultValue(0);

            _sadWindowSizeEdit.NameLabelText = "Min disparity";
            _sadWindowSizeEdit.SetDefaultValue(0);

            _p1Edit.NameLabelText = "P1";
            _p1Edit.SetDefaultValue(0);

            _p2Edit.NameLabelText = "P2";
            _p2Edit.SetDefaultValue(0);

            _minDisparityEdit.NameLabelText = "Min disparity";
            _minDisparityEdit.SetDefaultValue(0);

            _minDisparityEdit.NameLabelText = "Min disparity";
            _minDisparityEdit.SetDefaultValue(0);

            _minDisparityEdit.NameLabelText = "Min disparity";
            _minDisparityEdit.SetDefaultValue(0);

            _minDisparityEdit.NameLabelText = "Min disparity";
            _minDisparityEdit.SetDefaultValue(0);

            _minDisparityEdit.NameLabelText = "Min disparity";
            _minDisparityEdit.SetDefaultValue(0);

        }
    }
}
