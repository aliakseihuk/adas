using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Adas.Ui.Controls
{
    public partial class ValueEdit : UserControl
    {
        public ValueEdit()
        {
            InitializeComponent();
        }
        public string NameLabelText
        {
            get { return _nameLabel.Text; }
            set { _nameLabel.Text = value; }
        }
        
        public int? GetValue()
        {
            int value;
            if (int.TryParse(_valueBox.Text, out value))
                return value;
            MessageBox.Show(string.Format("Format of {0} is invalid", NameLabelText));
            return null;
        }

        public void SetDefaultValue(int defaultValue)
        {
            _valueBox.Text = defaultValue.ToString(CultureInfo.InvariantCulture);
        }
    }
}
