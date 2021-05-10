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

        [DefaultValue(false)]
        public bool IsDirty { get; set; } = false;

        [DefaultValue(@"Confirm form closing")]
        public string Caption { get; set; } = @"Confirm form closing";

        [Browsable(false)]
        public DialogResult Decision { get; set; }

        [DefaultValue(DialogResult.No)]
        public DialogResult KeepFormOpenedDecision { get; set; } = DialogResult.No;

        [DefaultValue(MessageBoxButtons.YesNo)]
        public MessageBoxButtons Buttons { get; set; } = MessageBoxButtons.YesNo;

        [DefaultValue(MessageBoxIcon.Question)]
        public MessageBoxIcon Icon { get; set; } = MessageBoxIcon.Question;

        [DefaultValue(MessageBoxDefaultButton.Button2)]
        public MessageBoxDefaultButton DefaultButton { get; set; } = MessageBoxDefaultButton.Button2;

        [DefaultValue("Your form content changed.")]
        public string TextLine1 { get; set; } = "Your form content changed.";

        [DefaultValue("If you close the form, you will lose changes to form content. If you want to close the form and lose the content, click Yes. If you want to keep the form opened, click No.")]
        public string TextLine2 { get; set; } = "If you close the form, you will lose changes to form content. If you want to close the form and lose the content, click Yes. If you want to keep the form opened, click No.";

        [DefaultValue("Are you sure you want to close the form?")]
        public string TextLine3 { get; set; } = "Are you sure you want to close the form?";

        public void ConfirmFormClosing(object sender, FormClosingEventArgs e)
        {
            if (IsDirty)
            {
                Form s = (Form)sender;
                if (s.DialogResult == DialogResult.Cancel || s.DialogResult == DialogResult.None)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendFormat(@"{1}{0}{0}{2}{0}{0}{3}", Environment.NewLine, TextLine1, TextLine2, TextLine3);
                    Decision = MessageBox.Show(sb.ToString(), Caption, Buttons, Icon, MessageBoxDefaultButton.Button2);
                    e.Cancel = (Decision == KeepFormOpenedDecision);
                }
            }
        }
    }
}
