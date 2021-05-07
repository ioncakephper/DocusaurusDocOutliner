using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DocusaurusDocOutlinerControlLibrary
{
    public partial class WeblidityFormCloser : Component
    {
        public WeblidityFormCloser()
        {
            InitializeComponent();
        }

        public WeblidityFormCloser(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        public bool IsDirty { get; set; }
        public string Caption { get; set; } = @"Docusaurus Documentation Project Outliner";
        public DialogResult Decision { get; set; }
        public DialogResult CancelDecision { get; set; } = DialogResult.Yes;
        public MessageBoxButtons Buttons { get; set; } = MessageBoxButtons.YesNo;
        public MessageBoxIcon Icon { get; set; } = MessageBoxIcon.Question;
        public MessageBoxDefaultButton DefaultButton { get; set; } = MessageBoxDefaultButton.Button2;
        public string TextLine1 { get; set; } = "Your form content changed.";
        public object TextLine2 { get; set; }
        public object TextLine3 { get; set; } = "Are you sure you want to close the form?";

        public void ConfirmFormClosing(object sender, FormClosingEventArgs e)
        {
            if (IsDirty)
            {
                Form s = (Form)sender;
                if (s.DialogResult == DialogResult.Cancel || s.DialogResult == DialogResult.None)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendFormat(@"{1}{0}{2}{0}{3}", Environment.NewLine, TextLine1, TextLine2, TextLine3);
                    Decision = MessageBox.Show(sb.ToString(), Caption, Buttons, Icon, MessageBoxDefaultButton.Button2);
                    e.Cancel = (Decision != CancelDecision);
                }
            }
        }
    }
}
